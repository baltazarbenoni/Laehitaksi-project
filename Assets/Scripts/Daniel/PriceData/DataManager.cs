using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
//C 2025 Daniel Snapir alias Baltazar Benoni
namespace PriceData
{
    public class DataManager : MonoBehaviour
    {
        public Dictionary<string, GetJsonText.PriceCategory> PriceTable;
        public GetJsonText.PriceCategory currentTable;
        PriceCategory priceCategory;
        List<PriceCategory> priceCategories = new();
        public bool IsEveningOrSunday = false;
        void Awake()
        {
            GetPriceTable();
            InitializeCategories();
        }
        void Start()
        {
            foreach (var item in priceCategories)
            {
                Debug.Log("Name : " + item.name + "\n");
                item.PrintContents();
                Debug.Log("\n");
            }
            GetButtonAmount();
        }
        //Get prices from json-file and create a dictionary-object.
        void GetPriceTable()
        {
            PriceTable = GetJsonText.PricePack.CreatePack();
            if (PriceTable == null)
            {
                Debug.LogWarning("Not able to read price table!");
            }
        }
        #region Initialize
        //INITIALIZE THE PRICE CATEGORY -OBJECTS FROM SERIALIZED JSON.
        void InitializeCategories()
        {
            //Iterate through all the entries (objects consist of keys and price categories) in the dictionary.
            foreach (KeyValuePair<string, GetJsonText.PriceCategory> kvp in PriceTable)
            {
                //Create price categories and add them to list.
                priceCategory = new PriceCategory(kvp.Key);
                priceCategory.AssignId(kvp.Key);
                priceCategories.Add(priceCategory);

                //Get the current table.
                currentTable = PriceTable[kvp.Key];

                //Iterate through all the properties of the entry. Add property names and values to dictionary.
                foreach (var prop in currentTable.GetType().GetProperties())
                {
                    if (prop.GetValue(currentTable, null) == null)
                    {
                        continue;
                    }
                    else
                    {
                        var element = prop.GetValue(currentTable, null).ToString();
                        var key = prop.Name;
                        priceCategory.AddContent(key, element);
                    }
                }
            }
        }
        #endregion Initialize
        #region Fetch general data
        internal string GetDataFromCategory(string categoryName, string key)
        {
            PriceCategory category = GetPriceCategory(categoryName);
            if (category == null)
            {
                return "";
            }
            string data = category.ReadContent(key);
            return data;
        }
        internal string GetDataFromCategory(int categoryClass, string key)
        {
            PriceCategory category = GetCategoryById(categoryClass);
            if (category == null)
            {
                return "";
            }
            string data = category.ReadContent(key);
            return data;
        }
        internal List<PriceCategory> GetCategoryList()
        {
            return priceCategories;
        }
        public PriceCategory GetPriceCategory(string key)
        {
            foreach (var item in priceCategories)
            {
                if (item.name == key)
                {
                    return item;
                }
            }
            return null;
        }
        int GetButtonAmount()
        {
            int amount = 0;
            foreach (var item in priceCategories)
            {
                amount += item.GetCount();
            }
            return amount;
        }
        public PriceCategory GetCategoryById(int num)
        {
            foreach (var cat in priceCategories)
            {
                if (cat.Id == num)
                {
                    return cat;
                }
            }
            return null;
        }
    #endregion
    #region Check night
        //Check if current time and date necessitates a special tax.
        public static bool IsNightOrHoliday(DateTime now)
        {
            TimeSpan currentTime = now.TimeOfDay;
            bool isSunday = now.DayOfWeek == DayOfWeek.Sunday;
            TimeSpan morningLimit = new TimeSpan(6, 0, 0);
            TimeSpan eveningLimit = new TimeSpan(18, 0, 0);

            //If sunday or nighttime, return true, else false.
            bool specialTax = isSunday || currentTime > eveningLimit || currentTime < morningLimit;
            return specialTax;
        }
    }
    #endregion
}