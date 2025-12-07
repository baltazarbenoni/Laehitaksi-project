using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace PriceData
{
    //The data from the json-file (containing prices) is stored into objects of this class. Each 'price category' is its own object.
    public class PriceCategory
    {
        public PriceCategory(string name)
        {
            this.name = name;
        }
        internal string name;
        internal string Name { get { return name; } }
        //The digits (all of them) in the beginning of each category's name are it's id.
        int id;
        public int Id { get; set; }
        //The first digits are the id of this 'category class'.
        public int ClassId { get { return GetClassId(); } }
        //Dictionary stores the data of the object.
        Dictionary<string, string> contents = new();
        public Dictionary<string, string> Content { get { return contents; } }
        public int Count
        {
            get { return this.GetElementCount(); }
        }
        internal void AddContent(string key, string element)
        {
            this.contents.Add(key, element);
        }
        internal string ReadContent(string key)
        {
            if (contents.ContainsKey(key))
            {
                return contents[key];
            }
            else
            {
                Debug.Log("Unable to fetch content.");
                return "";
            }
        }
        //Prints everything in the 'contents' dictionary.
        internal void PrintContents()
        {
            foreach (KeyValuePair<string, string> kvp in contents)
            {
                if (kvp.Value != "")
                {
                    Debug.Log($"Key {kvp.Key}, element: {kvp.Value}");
                }
            }
        }
        //Get the amount of different fees for this category (by counting the 'tunniste' entries, indexes from 0-9).
        internal int GetCount()
        {
            int amount = 0;
            for (int i = 1; i < 10; i++)
            {
                string item = this.GetElementAt("tunniste", i);
                if (item != "")
                {
                    amount++;
                }
                else
                {
                    break;
                }
            }
            return amount;
        }
        //Get the amount of entries in this category by counting the entries specified by 'key' (for example 'hinta' or 'luokka' or 'tunniste').
        internal int GetCount(string key)
        {
            int amount = 0;
            if (key == "")
            {
                return -1;
            }
            for (int i = 1; i < 10; i++)
            {
                bool hasElement = HasElementAt(key, i);
                if (hasElement)
                {
                    amount++;
                }
                else
                {
                    break;
                }
            }
            return amount;
        }
        public Dictionary<string, string> GetContents()
        {
            return this.contents;
        }
        #region Get elements
        bool HasElementAt(string keyType, int index)
        {
            string key = keyType + index.ToString();
            if (contents.ContainsKey(key) == false)
            {
                return false;
            }
            else if (contents[key] == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string GetElementAt(string keyType, int index)
        {
            string key = keyType + index.ToString();
            if (this.contents.ContainsKey(key))
            {
                return contents[key];
            }
            else
            {
                return "";
            }
        }
        public int GetElementCount()
        {
            int counter = 0;
            string result = "";

            //while...
            for (int i = 1; i < 11; i++)
            {
                result = this.GetElementAt("tunniste", i);
                if (result != "")
                {
                    counter++;
                }
                else
                {
                    break;
                }
            }
            return counter;
        }
        #endregion Get elements
        #region Id functions
        int GetClassId()
        {
            int newId = (int)(this.id / 10);
            Debug.Log("Returning class id " + newId);
            return newId;
        }
        public void AssignId(string name)
        {
            this.id = int.Parse(GetIdNumber(name));
            this.Id = id;
        }
        #endregion Id functions
        //Get the id number of a category with it's name.
        public static string GetIdNumber(string id)
        {
            string pattern = $"\\d+";
            Match match = Regex.Match(id, pattern);
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                Debug.Log("Couldn't use id to get other data");
                return "";
            }
        }
        //Get the category name with its id.
        public static string GetCategoryByClassId(int id)
        {
            return id switch
            {
                1 => "11: matkataksa",
                3 => "31: matkataksa",
                6 => "61: matkataksa",
                7 => "71: matkataksa",
                _ => ""
            };
        }
        public static string GetCategoryNameEnding(int index)
        {
            return index switch
            {
                0 => "0: lähtömaksu",
                1 => "1: matkataksa",
                2 => "2: aikataksa",
                3 => "3: lisämaksut",
                4 => "4: minimimaksu",
                5 => "5: odotustaksa",
                _ => ""
            };
        }
    }
}