using System.Collections.Generic;
using Navigation;
using UnityEngine;
using Input;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class Customers : MonoBehaviour
{
    List<Customer> customers = new();
    internal List<Customer> CustomerList { get { return customers; } }
    [SerializeField] GameObject statusManagerObj;
    PriceStatusManager statusManager;
    void Awake()
    {
        Actions.Input += CheckInput;
        statusManager = References.GetRef(gameObject, statusManagerObj, statusManager);
    }
    void CheckInput(InputMode type, int customerId)
    {
        if(type.inputType == InputType.Customer)
        {
            AddCustomer(customerId);
        }
    }
    void AddCustomer(int customerNum)
    {
        float sum = statusManager.BasicPrice;
        //If first addition, create 2 new customers: the past one and the current one.
        if (customers.Count == 0)
        {
            Customer firstCustomer = new Customer(0, sum);
            customers.Add(firstCustomer);
            Debug.Log("Added customer with number: " + customerNum);
            Customer newCustomer = new Customer(customerNum);
            customers.Add(newCustomer);
        }
        //When creating a new customer, assign the price for the previous one.
        else
        {
            float previousSum = sum - customers[customers.Count - 2].Sum;
            customers[customers.Count - 1].Sum = previousSum;
            Debug.Log("Added customer with number: " + customerNum);
            Customer newCustomer = new Customer(customerNum);
            customers.Add(newCustomer);
        }
        statusManager.UpdateCustomers(customers);
    }   
}
