using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Taxes
{
    public class ExtraFee 
    {
        public ExtraFee(ExtraPaymentData data)
        {
            this.data = data;
            this.price = data.Price;
            this.name = data.Name;
        }
        int price;
        public int Price { get { return price; } }
        string name;
        public string Name { get { return name; } }
        ExtraPaymentData data;
        public ExtraPaymentData Data { get { return data; } }
    }
}