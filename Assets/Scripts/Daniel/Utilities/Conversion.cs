using UnityEngine;
using System;
//C 2025 Daniel Snapir alias Baltazar Benoni

public static class Conversion
{
     #region String to fixed point
    //Convert a string representing a decimal number to a fixed point integer (1 for 0,01).
    internal static int StringToInt(string str)
    {
        int sum = 0;
        int multiplicator = 0;
        int dotIndex = str.IndexOf(',') > -1 ? str.IndexOf(',') : str.IndexOf('.');
        //If string contains no '.' or ',' --> try parse. Then return.
        if (dotIndex < 0)
        {
            try
            {
                sum = Int32.Parse(str);
                return sum;
            }
            catch (Exception e)
            {

                Debug.Log(e.Message);
                Debug.Log("Cannot change string to number, invalid format");
                return -1;
            }
        }
        //Count up digits before the decimal point.
        for (int i = 0; i < dotIndex; i++)
        {
            multiplicator = (int)Mathf.Pow(10f, dotIndex - i);
            try
            {
                int num = (int)Char.GetNumericValue(str[i]);
                int result = num * 10 * multiplicator;
                sum += result;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("Cannot change string to number, invalid format");
                return -1;
            }
        }
        //Count up the digits after the decimal point.
        //If there are no decimal numbers, return.
        if (dotIndex + 1 >= str.Length)
        {
            return sum;
        }
        for (int i = dotIndex + 1; i < str.Length; i++)
        {
            int place = str.Length - i - 1;
            if (place < 0)
            {
                return -1;
            }
            multiplicator = (int)Mathf.Pow(10, place);
            try
            {
                int num = (int)Char.GetNumericValue(str[i]);
                sum += num * multiplicator;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log("Cannot change string to number, invalid format");
                return -1;
            }
        }
        Debug.Log("Sum is " + sum);
        return sum;
    }
    #endregion
    #region Num to string
    //Create a string representation of an integer representing a decimal number with 2 decimals (0,00).
    internal static string IntToString(int num)
    {
        int aboveZero = num / 100;
        int decimals = num - aboveZero * 100;
        string addition2 = GetZerosToAdd(decimals);
        string txt = $"{aboveZero},{addition2}{decimals}";
        return txt;
    }
    //Create a string representation of an integer (in float type) representing a decimal number with 2 decimals (0,00).
    internal static string FloatToString(float number)
    {
        int num = Mathf.RoundToInt(number); 
        int aboveZero = num / 100;
        int decimals = num - aboveZero * 100;
        string addition2 = GetZerosToAdd(decimals);
        string txt = $"{aboveZero},{addition2}{decimals}";
        return txt;
    }
    //Get the amount of zeros to add to the representation.
    public static string GetZerosToAdd(int a)
    {
        if (a >= 10)
        {
            return "";
        }
        else
        {
            return "0";
        }
    }
    #endregion
    #region Seconds from hours, minutes, seconds integer
    public static int GetSeconds(int num)
    {
        int secSum = 0;
        //If num has over 3 digits, get hours.
        int hours = num >= 10000 ? num / 10000 : 0; 
        //If num has over 2 digits, get minutes.
        int minutes = num >= 100 ? num / 100 - hours * 1000 : 0;
        //Get seconds.
        int seconds = num - minutes * 100 - hours * 10000;
        //Add up the results.
        secSum = seconds + minutes * 60 + hours * 3600;
        Debug.Log($"Conversion: seconds {seconds}, minutes {minutes}, hours {hours}");
        return secSum;
    }
    public static string GetFormattedTime(float seconds)
    {
        float hours = 0;
        float mins =  0;
        float secs = seconds;
        if(secs >= 3600f)
        {
            hours = Mathf.Floor(seconds / 3600f);
            secs -= 3600f * hours;
        }
        if(secs >= 60f)
        {
            mins = Mathf.Floor(seconds / 60f);
            secs -= 60f * mins;
        }
        string h = hours > 0 ? hours.ToString() + "h" : ""; 
        string m = mins > 0 ? mins.ToString() + "min" : "";
        string s = secs.ToString() + "s";
        string result = $"{h} {m} {s}";
        return result;
    }
    #endregion   
}
