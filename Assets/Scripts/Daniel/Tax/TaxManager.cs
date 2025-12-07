using UnityEngine;

namespace Taxes
{
    public class TaxManager
    {
        public TaxManager(){}
        public TaxManager(PriceUpdate manager)
        {
            this.manager = manager;
        }
        PriceUpdate manager;
        Tax previous = new();
        public Tax Previous => previous;
        Tax current = new();
        public Tax Current => current;
        TaxMode mode;
        public TaxMode Mode => mode;
        Tax baseTax = new();
        Tax fixedTax = new();
        Tax waitTax = new();
        bool baseIsSet; 
        public void Change(Tax newTax, Tax old)
        {
            if(old == null)
            {
                Debug.Log($"Old tax is null {old == null}");
                Change(newTax);
            }
            else
            {
                current = newTax;
                previous = old;
                UpdateTaxes(newTax);
            }            
        }
        public void Change(Tax newTax)
        {
            if(newTax == null)
            {
                Debug.LogWarning($"New tax is null: {newTax == null}");
                return;
            }
            Debug.Log($"New tax is {newTax.Name}");
            current = newTax;
            UpdateTaxes(newTax);
        }

        //This needed to check if moving back to comission mode at checkout on  fixed price;
        public Tax GetBase()
        {
            if(mode == TaxMode.Normal)
            {
                Debug.Log("tax mode normal, returning current as base");
                Debug.Log($"base tax is: {current.Name}");
                return current;
            }
            else
            {
                Debug.Log("tax mode not normal, returning previous as base");
                return baseTax;
            }
        }
        public bool GetBaseStatus()
        {
            Debug.Log($"Base tax is set {baseIsSet}");
            return baseIsSet;
        }
        void UpdateTaxes(Tax tax)
        {
            Debug.Log($"New tax for update is {tax.Name}");
            mode = GetTaxMode(tax);
            if(mode == TaxMode.Normal)
            {
                baseIsSet = true;
                Debug.Log($"Base tax is set: {baseIsSet}");
                baseTax = tax;
            }
            if(mode == TaxMode.Wait)
            {
                baseTax = previous;
                waitTax = tax;
            }
            if(mode == TaxMode.Fixed)
            {
                baseTax = previous;
                fixedTax = tax;
            }
            Debug.Log($"Base tax is set: {baseIsSet}");
        }
        TaxMode GetTaxMode(Tax tax)
        {
            if(tax.isWait)
            {
                Debug.Log("Tax mode is wait");
                return TaxMode.Wait;
            }
            else if(tax.isFixed)
            {
                Debug.Log("Tax mode is fixed");
                return TaxMode.Fixed;
            }
            else
            {
                Debug.Log("Tax mode is normal");
                return TaxMode.Normal;
            }
        }
        public float GetFixed()
        {
            if (current.isFixed)
            {
                return current.FixedPrice;
            }
            else if (previous.isFixed)
            {
                return previous.FixedPrice;
            }
            return 0;
        }
    }
}