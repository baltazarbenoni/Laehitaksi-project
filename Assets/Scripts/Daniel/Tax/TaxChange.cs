using UnityEngine;
using System;
using PriceData;
using Fetching;
using Navigation;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Taxes 
{
    public class TaxChange : MonoBehaviour
    {
        Tax currentTax;
        //Price manager game object.
        [SerializeField] GameObject managerObject;
        [SerializeField] GameObject statusManagerObject;
        DataManager dataManager;
        PriceStatusManager statusManager;
        bool initialTaxAdded;
        void Awake()
        {
            dataManager = References.GetRef(gameObject, managerObject, dataManager);
            statusManager= References.GetRef(gameObject, statusManagerObject, statusManager);
            InitializeEvents();
        }
        void InitializeEvents()
        {
            Actions.TaxSelect += UpdateCurrent;
            Actions.ComissionCompleted += ToggleInitialTaxStatus; 
        }
        //GENERAL FUNCTION TO EXECUTE EVERY TIME A NEW TAX IS SELECTED.
        //Upon selecting new tax, run this function. Updates tax and sends the new tax to other classes.
        void UpdateCurrent(TaxButton button)
        {
            IUpdate update = GetUpdateType(button.FullCategory);
            currentTax = update.GetNewTax(dataManager, button, !initialTaxAdded);
            Actions.NewTax(currentTax);
            initialTaxAdded = true;
        }
        void ToggleInitialTaxStatus()
        {
            Debug.Log("Initial tax added : false");
            initialTaxAdded = false;
        }
        IUpdate GetUpdateType(int fullCategory)
        {
            if(Mathd.LastDigit(fullCategory) == 5)
            {
                Debug.Log("New wait tax!");
                return new WaitTaxUpdate();
            }
            else if(fullCategory <= 90)
            {
                return new NormalUpdate();
            }
            else if(fullCategory > 100)
            {
                Debug.Log("New kela tax!");
                return new KelaUpdate();
            }
            else
            {
                return new AirportUpdate();
            }
        }
    }
    public interface IUpdate
    {
        public Tax GetNewTax(DataManager dataManager, TaxButton button, bool initialNeeded);
    }
    public class NormalUpdate : IUpdate
    {
        public Tax GetNewTax(DataManager manager, TaxButton button, bool initialNeeded)
        {
            int cat = button.Category;
            int index = button.Index;
            //Check if it's evening or sunday.
            manager.IsEveningOrSunday = DataManager.IsNightOrHoliday(DateTime.Now);
            //Fetch values by creating instances of the IData interface.
            int kmTax = FetchTax.Fetch(new KmTaxData(manager), cat, index);
            int timeTax = FetchTax.Fetch(new TimeTaxData(manager), cat, index);
            int minPrice = FetchTax.Fetch(new SimpleData(manager, (int)SimpleData.Type.Minimal), cat, index);
            //Fetch initial tax only if necessary.
            int initialPrice = initialNeeded ? FetchTax.Fetch(new InitialTaxData(manager), cat, index) : 0;

            //Create a new 'Tax' instance from the fetched values and return it.
            Tax tax = new Tax(button.Name, cat, kmTax, timeTax, initialPrice, minPrice);
            return tax;
        }
    }
    public class KelaUpdate : IUpdate
    {
        public Tax GetNewTax(DataManager manager, TaxButton button, bool initialNeeded)
        {
            int cat = button.Category;
            int index = button.Index;
            //Check if it's evening or sunday.
            manager.IsEveningOrSunday = DataManager.IsNightOrHoliday(DateTime.Now);
            //Fetch values by creating instances of the IData interface.
            int kmTax = FetchTax.Fetch(new KmTaxData(manager), cat, index);
            //Fetch initial tax only if necessary.
            int initialPrice = initialNeeded ? FetchTax.Fetch(new InitialTaxData(manager), cat, index) : 0;
            //Create a new 'Tax' instance from the fetched values. Assign it to the 'currentTax field'.
            Tax tax = new Tax(button.Name, cat, kmTax, 0, initialPrice, 0);
            return tax;
        }
    }
    public class WaitTaxUpdate : IUpdate
    {
        public Tax GetNewTax(DataManager manager, TaxButton button, bool initialNeeded)
        {

            int cat = button.Category;
            Debug.Log("category for wait tax is " + cat);
            int index = button.Index;
            Debug.Log("Index for wait tax is " + index);
            //Check if it's evening or sunday.
            manager.IsEveningOrSunday = DataManager.IsNightOrHoliday(DateTime.Now);
            int waitTax = FetchTax.Fetch(new WaitTaxData(manager), cat, index);
            int minPrice = GetMinPrice(manager, button);
            //Fetch initial tax only if necessary.
            int initialPrice = initialNeeded ? FetchTax.Fetch(new InitialTaxData(manager), cat, index) : 0;
            //Create a new 'Tax' instance from the fetched values. Assign it to the 'currentTax field'.
            Tax tax = new Tax(button.Name, cat, 0, 0, initialPrice, minPrice);
            tax.AddWaitTax(waitTax);
            return tax;
        }
        int GetMinPrice(DataManager manager, TaxButton button)
        {
            if(button.Category > 10)
            {
                return 0;
            }
            else
            {
                int minPrice = FetchTax.Fetch(new SimpleData(manager, (int)SimpleData.Type.Minimal), button.Category, button.Index);
                return minPrice;
            }
        }
    }
    public class AirportUpdate : IUpdate
    {
        public Tax GetNewTax(DataManager manager, TaxButton button, bool initialNeeded)
        {
            int cat = button.Category;
            int index = button.Index;
            //Check if it's evening or sunday.
            manager.IsEveningOrSunday = DataManager.IsNightOrHoliday(DateTime.Now);
            //If button index over 1, using fixed price.
            if(index > 1)
            {
                FixedPriceData fixedPriceData = new FixedPriceData(manager);
                //Fetch data.
                int fixedPrice = FetchTax.Fetch(fixedPriceData, cat, index);
                //Create a tax with zeros for everything except the fixed price.
                Tax tax = new Tax(button.Name, cat, 0, 0, 0, 0);
                //Assign fixed price.
                tax.AddFixedPrice(fixedPrice);
                return tax;
            }
            //Else using airport kilometer tax.
            else
            {
                AirportTaxData airportTaxData = new AirportTaxData(manager);
                //Fetch data.
                int kmTax = FetchTax.Fetch(airportTaxData, cat, index);
                Tax tax = new Tax(button.Name, cat, kmTax, 0, 0, 0);
                return tax;
            }
        }
    }
}