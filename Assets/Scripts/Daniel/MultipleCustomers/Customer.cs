using UnityEngine;
using System;

public class Customer
{
    float sum;
    public float Sum
    {
        get
        {
            return sum;
        }
        set
        {
            sum = value;
        }
    }
    int id;
    public int Id { get { return id; }  }
    Type status;
    public Type Status; 
    PriceStatus snapshot;
    public Customer(int id, float sum)
    {
        this.status = id == 0 ? Type.Initial : Type.Added;
        this.id = id;
        this.sum = sum;
    }
    public Customer(int id)
    {
        this.status = id == 0 ? Type.Initial : Type.Added;
        this.id = id;
        this.sum = 0;
    }       
    public enum Type
    {
        Initial,
        Added
    }
}