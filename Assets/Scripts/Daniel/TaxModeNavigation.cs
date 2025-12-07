using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation 
{

public class TaxModeNavigation : MonoBehaviour
{
    List<GameObject> navButtons = new();
    NavigButton script;

    void Awake()
    {

    }
    void OnEnable()
    {
        InitNavButtons();
        //Actions.TaxModeOptions += OptionsPressed;
    }
    void Start()
    {

    }
    void InitNavButtons()
    {
        Debug.Log("Adding children to list");
        foreach (Transform child in transform)
        {
            Debug.Log("Added child : " + child.gameObject.name);
            navButtons.Add(child.gameObject);
        }
    }
    void OptionsPressed()
    {
        foreach (GameObject go in navButtons)
        {
            Debug.Log("Checking nav buttons");
            script = go.GetComponent<NavigButton>();
            if (script == null)
            {
                Debug.LogWarning("Couldn't find script!");
                continue;
            }
            else
            {
                UpdateNavigButton(script);
                script.UpdateText();
            }
        }
    }
    void UpdateNavigButton(NavigButton script)
    {
        Debug.Log("Checking nav button : " + script.type);
        script.type = GetNewType(script.type);
    }
    ButtonType GetNewType(ButtonType type)
    {
        return type switch
        {
            ButtonType.Tax => ButtonType.Hold,
            ButtonType.Options => ButtonType.MaxPrice,
            ButtonType.AddCustomer => ButtonType.MaxTime,
            ButtonType.Kassa => ButtonType.KustPk,
            _ => ButtonType.None,
        };
    }
}
}