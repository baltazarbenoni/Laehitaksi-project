using UnityEngine;
using Taxes;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Instantiation
{
    //Class to use in instantiation of tax buttons.
    public class TaxButtonInstantiator
    {
        public TaxButtonInstantiator(InstantiateTaxButtons manager)
        {
            this.manager = manager;
            categoriesToInstantiate = GetCategoriesToInstatiate(manager.Page);
            instantiator = GetInstantiator(manager);
        }
        int[] categoriesToInstantiate;
        public int[] Categories { get { return categoriesToInstantiate; } }
        int buttonIndex;
        //Interface which assigns positions for the buttons. 'OnComission' tax page needs a button placement which differs from the other pages.
        ITaxInstantiation instantiator;
        InstantiateTaxButtons manager;
        ITaxInstantiation GetInstantiator(InstantiateTaxButtons manager)
        {
            if (manager.Page == TaxPage.OnComission)
            {

                return new FixedSizeInstantiation(manager.Anchor, manager.Limit, manager.Spacing, 3);
            }
            else
            {
                return new TaxButtonInstantiation(manager.Anchor, manager.Limit, manager.Spacing);
            }
        }
        int[] GetCategoriesToInstatiate(TaxPage page)
        {
            if (page == TaxPage.OnComission)
            {
                Tax tax = manager.Data.TaxInUse;
                int currentCategory = tax != null ? tax.Category : -1;
                return TaxFamilyManager.GetCategoryClassesByCurrent(currentCategory);
            }
            else
            {
                return TaxFamilyManager.GetCategoryClassesByPage(page);
            }
        }
        public Vector2 GetButtonPosition(RectTransform trans)
        {
            XY buttonSize = new XY(trans.sizeDelta.x, trans.sizeDelta.y);
            Vector2 pos = instantiator.GetButtonPosition(buttonSize, buttonIndex);
            Debug.Log("Instantiated button at " + pos.x + ", " + pos.y);
            buttonIndex++;
            return pos;
        }
    }
}