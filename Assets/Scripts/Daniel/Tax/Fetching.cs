using UnityEngine;
using PriceData;
using System;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Fetching 
{
    public class FetchTax
    {
        public static int Fetch(IData obj, int categoryClass, int index)
        {
            string strTax = obj.GetTax(categoryClass, index);
            Debug.Log(obj.GetName() + " is: " + strTax);
            if (strTax == "")
            {
                Debug.Log("Couldn't fetch " + obj.GetName());
                return -1;
            }
            int price = Conversion.StringToInt(strTax);
            return price;
        }
        public static List<int> FetchAll(List<IData> objects, int categoryClass, int index)
        {
            List<int> values = new();
            foreach(IData obj in objects)
            {
                int price = Fetch(obj, categoryClass, index);
                values.Add(price);
            }
            return values;
        }
    }
    public class FetchExtra
    {

        public static List<ExtraPaymentData> FetchAllExtra(DataManager manager, int categoryClass)
        {
            List<ExtraPaymentData> datas = new();
            int amount = ExtraPaymentData.GetAmount(manager, categoryClass);
            for(int i = 1; i <= amount; i++)
            {
                ExtraPaymentData data = new ExtraPaymentData(manager, categoryClass, i);
                datas.Add(data);
            }
            return datas;
        }
        public static ExtraPaymentData FetchAdditionalPayment(DataManager manager, int categoryClass, int index)
        {
            ExtraPaymentData data = new ExtraPaymentData(manager, categoryClass, index);
            return data;
        }
    }
}