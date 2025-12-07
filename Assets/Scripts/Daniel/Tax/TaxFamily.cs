using Instantiation;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

public static class TaxFamilyManager
{
    public static int[] GetCategoryClassesByCurrent(int categoryClass)
    {
        return GetComissionFamily(categoryClass).CategoryClasses;
    }
    public static int[] GetCategoryClassesByPage(TaxPage page)
    {
        return TaxFamilyFreeState.GetClassesByPage((int)page);
    }
    static TaxFamilyComission GetComissionFamily(int categoryClass)
    {
        int familyId = TaxFamily.GetFamilyId(categoryClass);
        return new TaxFamilyComission(familyId);;
    }
    static TaxFamilyFreeState GetFreeStateFamily(int categoryClass)
    {
        int familyId = TaxFamily.GetFamilyId(categoryClass);
        return new TaxFamilyFreeState(familyId);;
    }
}
public class TaxFamily
{
    public TaxFamily(int familyId)
    {
        this.familyId = familyId;
        this.type = (TaxFamilyType)familyId;
        this.categoryClasses = GetClasses();
    }
    protected int familyId;
    public int FamilyId { get { return familyId; } }
    protected int[] categoryClasses;
    public int[] CategoryClasses { get { return categoryClasses; } }
    protected TaxFamilyType type;
    public TaxFamilyType Type { get { return type; } }
    protected virtual int[] GetClasses()
    {
        return new int[] { 0 };
    }
    public static int GetFamilyId(int categoryClass)
    {
        if(categoryClass == 3)
        {
            return 3;
        }
        else if(categoryClass < 10)
        {
            return 1;
        }
        else
        {
            return categoryClass;
        }
    }
}
public class TaxFamilyFreeState : TaxFamily
{
    public TaxFamilyFreeState(int familyId) : base (familyId){}
    //Returns the 'category classes', that is, the id numbers at the start of each price category's name in the JSON-file.
    protected override int[] GetClasses()
    {
        switch(familyId)
        {
            case 1:
                return new int[] { 11, 61, 71, 100 };
            case 3:
                return new int[] { 31 };
            case 11:
                return new int[] { 111, 115 };
            case 12:
                return new int[] { 121, 125 };
            case 13:
                return new int[] { 131, 135 };
            default:
                return new int[] { };
        }
    }
    //Returns the 'category classes', that is, the id numbers at the start of each price category's name in the JSON-file.
    static internal int[] GetClassesByPage(int page)
    {
        switch(page)
        {
            case 0:
                return new int[] { 11, 61, 71, 100 };
            case 1:
                return new int[] { 31, 111, 115 };
            case 2:
                return new int[] { 121, 125, 131, 135 };
            default:
                return new int[] { };
        }
    }
}
public class TaxFamilyComission : TaxFamily
{
    public TaxFamilyComission(int familyId) : base (familyId){}
    //Returns the 'category classes', that is, the id numbers at the start of each price category's name in the JSON-file.
    protected override int[] GetClasses()
    {
        switch(familyId)
        {
            case 1:
                return new int[] { 11, 15, 61, 71 };
            case 3:
                return new int[] { 31, 35 };
            case 10:
                return new int[] { 100 };
            case 11:
                return new int[] { 111, 115 };
            case 12:
                return new int[] { 121, 125 };
            case 13:
                return new int[] { 131, 135 };
            default:
                return new int[] { };
        }
    }
}
public enum TaxFamilyType
{
    None,
    L = 1,
    VK = 3,
    LENTO = 10,
    KELAU = 11,
    KELAKH = 12,
    KELALA = 13
}
