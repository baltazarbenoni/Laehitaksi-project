using UnityEngine;
using PriceData;

public class ExtraPaymentData : DataType
{
    int price;
    public int Price { get{ return price; } }
    string name;
    public string Name { get{ return name; } }
    int index;
    public int Index { get{ return index; } }
    public ExtraPaymentData(DataManager manager, int categoryClass, int index) : base(manager)
    {
        this.typeName = "additional payment";
        this.index = index;
        GetData(categoryClass, index, out this.price, out this.name);
    }
    void GetData(int categoryClass, int index, out int price, out string name)
    {
        string categoryName = categoryClass.ToString() + "3: lisämaksut";
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            name = "";
            price = -1;
            return;
        }
        //Assign price.
        string priceKey = "hinta" + index.ToString();
        string priceString = category.ReadContent(priceKey);
        price = Conversion.StringToInt(priceString);

        //Assign name.
        string nameKey = "luokka" + index.ToString();
        name = category.ReadContent(nameKey);
    }
    public static int GetAmount(DataManager manager, int categoryClass)
    {
        string categoryName = categoryClass.ToString() + "3: lisämaksut";
        PriceCategory category = manager.GetPriceCategory(categoryName);
        if (category == null)
        {
            Debug.LogWarning("Category not found!");
            return -1;
        }
        int amount = category.GetCount("hinta");
        return amount;
    }
}