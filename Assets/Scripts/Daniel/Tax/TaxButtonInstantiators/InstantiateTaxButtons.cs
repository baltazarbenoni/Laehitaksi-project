using UnityEngine;
using System.Collections;
using PriceData;
using Instantiation;
using UnityEngine.UI;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class InstantiateTaxButtons : MonoBehaviour
{
    //
    //General references.
    [SerializeField] GameObject taxButton;
    [SerializeField] GameObject taxButtonOnComission;
    GameObject instantiationObject;
    [SerializeField] GameObject dataParent;
    //Script to store data for scripts attatched to child game objects.
    DataForTaxCanvases data;
    public DataForTaxCanvases Data { get { return data; } }
    DataManager dataManager;
    //Variables associated with button instantiation.
    [SerializeField] float screenWidth = 1800f / 2;
    //float screenHeight = 100f;
    public XY Limit { get { return new XY(screenWidth, 0); } }
    [SerializeField] float spacingX = 50f;
    [SerializeField] float spacingY = 50f;
    public XY Spacing { get { return new XY(spacingX, spacingY); } }
    [SerializeField] float xAnchor = 0f;
    [SerializeField] float yAnchor = 0f;
    public XY Anchor { get { return new XY(xAnchor, yAnchor); } }
    //
    //Variables associated with what buttons to instantiate.
    [SerializeField] TaxPage page = TaxPage.One;
    public TaxPage Page { get { return page; } }
    TaxButtonInstantiator instantiator;
    bool instantiated;
    //List to store instantiated buttons.
    List<GameObject> childList = new();
    #region Initialize
    void Awake()
    {
        GetReferences();
    }
    void OnEnable()
    {
        if (!instantiated)
        {
            Debug.Log($"For instantiation, creating buttons for page {this.page}");
            InitializeInstantiator();
            InstantiateButtons();
        }
    }
    void GetReferences()
    {
        data = References.GetRef(gameObject, dataParent, data);
        dataManager = data.DataManager;
        //Subscribe to list of the dataParent-object. It updates the values of this script when it is inactive.
        data.SubscribeToList(this);
        if(taxButton == null || taxButtonOnComission == null)
        {
            Debug.Log("Assign prefabs to tax button instantiation!");
            return;
        }
        instantiationObject = page == TaxPage.OnComission ? taxButtonOnComission : taxButton;
    }
    void InitializeInstantiator()
    {
        instantiator = new TaxButtonInstantiator(this);
    }
    #endregion
    #region General
    void InstantiateButtons()
    {
        if (dataManager == null)
        {
            Debug.LogWarning("Assign data manager reference to tax button instantiation!");
            return;
        }
        foreach (int id in instantiator.Categories)
        {
            PriceCategory category = dataManager.GetCategoryById(id);
            if (category != null)
            {
                CreateButtons(category);
            }
            else
            {
                Debug.LogWarning("Category is null!?");
            }
        }
        instantiated = true;
    }
    #endregion

    #region Create many
    void CreateButtons(PriceCategory category)
    {
        if (category.Count == 0 || category == null)
        {
            Debug.Log("Category is empty!");
            return;
        }
        Debug.Log("Category size is " + category.Count);
        Debug.Log($"Category name is {category.Name}");
        for (int i = 1; i <= category.Count; i++)
        {
            StartCoroutine(CreateButton(category, i));
        }
    }
    #endregion
    #region Create one
    IEnumerator CreateButton(PriceCategory category, int i)
    {
        //Get the data to assign to the button.
        string tunniste = category.GetElementAt("tunniste", i);
        Debug.Log($"Button id: {tunniste}");
        
        //Instantiate button and assign it's properties.
        GameObject newButton = Instantiate(instantiationObject);
        newButton.transform.localScale = new Vector3(1, 1, 1);
        newButton.transform.SetParent(this.transform, false);
        RectTransform trans = newButton.GetComponent<RectTransform>();

        //Add button to child list.
        childList.Add(newButton);

        //Assign data to button.
        TaxButton buttonScript = newButton.GetComponent<TaxButton>();
        //Category class is the first digit (or first two digits) of the category's name in the json-file. Category id is the full id of this category. Variable 'i' is the index of this button in the category. It's used later to fetch other relevant data.
        buttonScript.SetData(category.ClassId, category.Id, tunniste, i);
        //Wait for two frames to adjust button position.
        yield return null;
        yield return null;
        trans.anchoredPosition = instantiator.GetButtonPosition(trans);
    }
    //Create new class to instantiate buttons on comission mode
    //Access variables: turn content size fitter to preferred?
    #endregion
    public void ChangeInstantiationStatus(bool val)
    {
        if(this.page != TaxPage.OnComission)
        {
            return;
        }
        instantiated = val;
        if(childList.Count == 0)
        {
            return;
        }
        foreach (var child in childList)
        {
            Debug.Log($"Destroying child {child.gameObject.name}");
            Destroy(child);
        }
        childList.Clear();
    }
}
