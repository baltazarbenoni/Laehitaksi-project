using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

[Serializable]
public class GetJsonText
{
    public class PricePack
    {

        public static Dictionary<string, PriceCategory> CreatePack()
        {

            TextAsset jsonTextFile = Resources.Load<TextAsset>("Hinnasto");

            if (jsonTextFile != null)
            {
                return JsonConvert.DeserializeObject<Dictionary<string, PriceCategory>>(jsonTextFile.text);
            }
            else
            {
                Debug.LogError("JSON FILE NOT FOUND");
                return null;
            }
        }
    }
    //A class to map onto the JSON-object from the Languages.json-file.
    public class PriceCategory
    {
        public string luokka1 { get; set; }
        public string luokka2 { get; set; }
        public string luokka3 { get; set; }
        public string luokka4 { get; set; }
        public string luokka5 { get; set; }
        public string luokka6 { get; set; }
        public string luokka7 { get; set; }
        public string luokka8 { get; set; }
        public string luokka9 { get; set; }
        public string luokka10 { get; set; }
        public string hinta1 { get; set; }
        public string hinta2 { get; set; }
        public string hinta3 { get; set; }
        public string hinta4 { get; set; }
        public string hinta5 { get; set; }
        public string hinta6 { get; set; }
        public string hinta7 { get; set; }
        public string hinta8 { get; set; }
        public string hinta9 { get; set; }
        public string hinta10 { get; set; }
        public string tunniste1 { get; set; }
        public string tunniste2 { get; set; }
        public string tunniste3 { get; set; }
        public string tunniste4 { get; set; }
        public string tunniste5 { get; set; }
        public string tunniste6 { get; set; }
        public string tunniste7 { get; set; }
        public string tunniste8 { get; set; }
        public string tunniste9 { get; set; }
        public string tunniste10 { get; set; }
    }
}
