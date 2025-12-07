using PriceData;
using UnityEngine;
using Taxes;
using Fetching;
using System.Collections;
using System.Collections.Generic;
using Instantiation;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class InstantiateExtraFeeButtons : MonoBehaviour
{
    [SerializeField] GameObject preFab;
    [SerializeField] GameObject dataManager;
    DataManager dataInstance;
    [SerializeField] GameObject priceManager;
    PriceUpdate priceInstance;
    int category;
    List<ExtraPaymentData> dataList = new();
    ExtraFeeButtonInstantiation instantiator;
    //Variables associated with button instantiation.
    [SerializeField] float width = 800f;
    [SerializeField] float height = 200f;
    //float screenHeight = 100f;
    [SerializeField] float xAnchor = -1500f;
    [SerializeField] float yAnchor = 350f;
    void Awake()
    {
        //Actions.ActivateAdditionManager += GetData;
        GetRefs();
        InitializeInstantiator();
    }
    void OnEnable()
    {
        InstantiateButtons();
    }
    void GetRefs()
    {
        dataInstance = References.GetRef(gameObject, dataManager, dataInstance);
        priceInstance = References.GetRef(gameObject, priceManager, priceInstance);
        if(priceInstance != null)
        {
            category = priceInstance.CurrentTax.Category;
            dataList = FetchExtra.FetchAllExtra(dataInstance, category);
        }
        else
        {
            Debug.LogWarning("Couldn't fetch data!");
        }
    }
    void InitializeInstantiator()
    {
        XY anchor = new XY(xAnchor, yAnchor);
        XY grid = new XY(width, height);
        instantiator = new ExtraFeeButtonInstantiation(anchor, grid);
    }
    void InstantiateButtons()
    {
        int i = 0;
        foreach(var item in dataList)
        {
            StartCoroutine(CreateButton(item, i));
            i++;
        }
    }
    IEnumerator CreateButton(ExtraPaymentData data, int i)
    {
        //Instantiate button and assign it's position.
        GameObject newButton = Instantiate(preFab);
        newButton.transform.localScale = new Vector3(1, 1, 1);
        newButton.transform.SetParent(this.transform, false);
        RectTransform trans = newButton.GetComponent<RectTransform>();
        //Assign data to button.
        ExtraFeeButton buttonScript = newButton.GetComponent<ExtraFeeButton>();
        ExtraFee buttonData = new ExtraFee(data);
        buttonScript.SetData(buttonData);
        //Wait for two frames to adjust button position.
        yield return null;
        yield return null;
        trans.anchoredPosition = GetButtonPosition(i);
    }
    Vector2 GetButtonPosition(int index)
    {
        Vector2 pos = instantiator.GetButtonPosition(index);
        Debug.Log("Instantiated button at " + pos.x + ", " + pos.y);
        return pos;
    }
}