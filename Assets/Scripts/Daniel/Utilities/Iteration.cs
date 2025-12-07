using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Iteration
{
    public static T GetNext<T>(IList<T> collection, ref int currentIndex)
    {
        int index = currentIndex + 1 < collection.Count ? currentIndex + 1 : 0;
        currentIndex = index;
        return collection[index];
    }
    public static T GetAt<T>(IList<T> collection, ref int index)
    {
        index = index < collection.Count ? index : 0;
        return collection[index];
    }
}