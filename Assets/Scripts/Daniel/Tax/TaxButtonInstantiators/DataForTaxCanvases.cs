using Taxes;
using UnityEngine;
using PriceData;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

//Script to store data for tax button instantiation. Simplifies inspector view and is needed when updating status while canvases are inactive.
public class DataForTaxCanvases : MonoBehaviour
{
    [SerializeField] GameObject statusManagerObject;
    [SerializeField] GameObject dataManagerObject;
    DataManager dataManager;
    public DataManager DataManager { get { return dataManager; } }
    PriceStatusManager statusManager;
    public PriceStatusManager StatusManager { get { return statusManager; } }
    public Tax TaxInUse { get { Debug.Log($"Current tax for instantiation: {statusManager.currentStatus.TaxInUse.Name}"); return statusManager.currentStatus.TaxInUse; } }
    List<InstantiateTaxButtons> childList = new();
    void Awake()
    {
        statusManager = References.GetRef(gameObject, statusManagerObject, statusManager);
        dataManager = References.GetRef(gameObject, dataManagerObject, dataManager);
        InitializeEvents();
    }
    void InitializeEvents()
    {
        Actions.ComissionCompleted += UpdateStatus;
    }
    void UpdateStatus()
    {
        foreach (var child in childList)
        {
            child.ChangeInstantiationStatus(false);
        }
    }
    public void SubscribeToList(InstantiateTaxButtons instance)
    {
        if (!childList.Contains(instance))
        {
            childList.Add(instance);
        }
    }
}
