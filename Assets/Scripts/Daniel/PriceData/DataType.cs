using UnityEngine;
using PriceData;

public interface IData
{
    public string GetTax(int categoryClass, int index);
    public string GetName();
}
public class DataType
{
    public DataType(DataManager manager)
    {
        this.manager = manager;
        this.eveningOrSunday = manager.IsEveningOrSunday;
    }
    protected string typeName = "";
    protected int typeSpecifier = 0;
    protected bool eveningOrSunday;
    protected DataManager manager;
    public string GetName()
    {
        return typeName;
    }
    protected string GetCategoryName(int categoryClass, TaxType type)
    {
        string nameEnding = PriceCategory.GetCategoryNameEnding((int)type);
        string categoryName = categoryClass.ToString() + nameEnding;
        return categoryName;
    }
}
#region Km
public class KmTaxData : DataType, IData
{
    public KmTaxData(DataManager manager) : base(manager)
    {
        this.typeName = "kilometer tax";
    }
    public string GetTax(int categoryClass, int index)
    {
        int thisIndex = 0;
        string categoryName = GetCategoryName(categoryClass, TaxType.Kilometer);
        PriceCategory category = this.manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning($"Couldn't get {typeName} for category " + categoryName);
            return "";
        }
        string key = "";
        if (eveningOrSunday && categoryClass < 11)
        {
            //Create the key for the holiday tax.
            thisIndex = index + category.Count;
        }
        //Holiday or evening status doesn't affect 'Kela' prices.
        else
        {
            thisIndex = index;
        }
        string indexString = thisIndex.ToString();
        key = "hinta" + indexString;
        return category.ReadContent(key);
    }
}
#endregion Km
#region Time
public class TimeTaxData : DataType, IData
{
    public TimeTaxData(DataManager manager) : base (manager)
    {
        this.typeName = "time tax";
    }
    public string GetTax(int categoryClass, int index)
    {

        string categoryName = GetCategoryName(categoryClass, TaxType.Time);
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning($"Couldn't get {typeName} for category " + category);
            return "";
        }
        //Create a key for the tax according to whether it's evening time or sunday.
        string key = eveningOrSunday ? "hinta2" : "hinta1";
        return category.ReadContent(key);
    }
}
#endregion Time
#region Initial
public class InitialTaxData : DataType, IData
{
    public InitialTaxData(DataManager manager) : base (manager)
    {
        this.typeName = "initial tax";
    }
    public string GetTax(int categoryClass, int index)
    {
        int thisIndex = 0;
        string categoryName = GetCategoryName(categoryClass, TaxType.Initial);
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning($"Couldn't get {typeName} for category " + categoryName);
            return "";
        }
        string key = "";
        //Use this when an initial Kela tax should be fetched. 
        if (categoryClass > 10)
        {
            thisIndex = eveningOrSunday ? 1 : 2;
        }
        else if (eveningOrSunday)
        {
            //Create the key for the holiday tax.
            thisIndex = index + category.Count;
        }
        else
        {
            thisIndex = index;
        }
        string indexString = thisIndex.ToString();
        key = "hinta" + indexString;
        return category.ReadContent(key);
    }
}
#endregion Initial
#region Simple
public class SimpleData : DataType, IData
{
    public enum Type
    {
        Minimal = 4,
        Wait = 5
    }
    public SimpleData(DataManager manager, int specifier) : base(manager)
    {
        this.typeSpecifier = specifier;
        this.typeName = PriceCategory.GetCategoryNameEnding(typeSpecifier) + " tax";
    }
    public string GetTax(int categoryClass, int index)
    {
        string nameEnding = PriceCategory.GetCategoryNameEnding(this.typeSpecifier);
        string categoryName = categoryClass.ToString() + nameEnding;
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning($"Couldn't get {typeName} for category " + categoryName);
            return "";
        }
        string key = "hinta1";
        return category.ReadContent(key);
    }
}
#endregion Simple
#region Wait tax
public class WaitTaxData : DataType, IData
{
    public WaitTaxData(DataManager manager) : base(manager)
    {
        this.typeName = "wait tax";
    }
    public string GetTax(int categoryClass, int index)
    {
        string categoryName = GetCategoryName(categoryClass, TaxType.Wait);
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning($"Couldn't get {typeName} for category " + categoryName);
            return "";
        }
        string key = "hinta" + index.ToString();
        Debug.Log("Key for search is " + key);
        return category.ReadContent(key);
    }
}
#endregion
#region Fixed
public class FixedPriceData : DataType, IData
{
    public FixedPriceData(DataManager manager) : base(manager)
    {
        this.typeName = "Fixed price";
    }
    public string GetTax(int categoryClass, int index)
    {
        string key = "hinta" + index.ToString();
        string price = manager.GetDataFromCategory(categoryClass, key);
        return price;
    }
}
#endregion Fixed
#region Airport
public class AirportTaxData : DataType, IData
{
    public AirportTaxData(DataManager manager) : base(manager)
    {
        this.typeName = "Airport tax";
    }
    public string GetTax(int categoryClass, int index)
    {
        string key = "hinta1";
        string price = manager.GetDataFromCategory(categoryClass, key);
        return price;
    }
}
#endregion Airport


