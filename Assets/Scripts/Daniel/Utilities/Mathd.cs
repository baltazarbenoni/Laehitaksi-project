using UnityEngine;
using System;
using System.Text.RegularExpressions;

//C 2025 Daniel Snapir alias Baltazar Benoni

public class Mathd
{
    internal static float RoundSpec(float num, int digits)
    {
        float a = Mathf.Pow(10f, digits);
        num *= a;
        num = Mathf.RoundToInt(num);
        return num / a;
    }
    internal static int LastDigit(int num)
    {
        return num % 10;
    }
}

