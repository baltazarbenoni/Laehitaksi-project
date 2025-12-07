using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Taxes
{
    public enum Mode
    {
        Normal,
        Kela,
        Airport,
        Fixed
    }
    public class Tax
    {
        public Tax()
        {
            this.name = "";
        }
        public Tax(string name, int category, int rate, int timeTax, int initialPrice, int minimumPrice)
        {
            this.name = name;
            this.category = category;
            this.rate = rate;
            this.taxByMin = timeTax;
            this.taxBySec = timeTax / 60f;
            this.initialTax = initialPrice;
            this.minPrice = minimumPrice;
            this.mode = GetMode(category);
        }
        Mode mode;
        public Mode taxMode => mode;
        int category;
        internal int Category { get { return category; } }
        int rate;
        internal int Rate { get { return rate; } }
        int initialTax;
        internal int InitialTax { get { return initialTax; } }
        float taxByMin = 0f;
        internal float ByMin { get { return taxByMin; } }
        float taxBySec;
        internal float BySec { get { return taxBySec; } }
        string name = "";
        internal string Name { get { return name; } }
        int minPrice;
        internal int MinPrice { get { return minPrice; } }
        int waitTaxRaw;
        float waitTax;
        internal float WaitTax { get { return waitTax; } }
        int maxPrice;
        internal int MaxPrice { get { return maxPrice; } }
        int maxWaitTime;
        internal int MaxWaitTime { get { return maxWaitTime; } }
        int maxWaitPrice;
        internal int MaxWaitPrice { get { return maxWaitPrice; } }
        WaitTimer waitTimer;
        internal WaitTimer WaitTimer => waitTimer;
        int fixedPrice;
        internal int FixedPrice { get { return fixedPrice; } }
        internal bool InitialTaxAdded;
        public bool isFixed { get; private set; }
        public bool isWait { get { return waitTax != 0; } }
        public void AddMaxPrice(int price)
        {
            this.maxPrice = price;
        }
        public void AddMaxWaitTime(int price)
        {
            Debug.Log("ADDED MAX WAIT!");
            this.maxWaitTime = price;
        }
        public void AddMaxWaitPrice(int price)
        {
            Debug.Log("ADDED MAX WAIT PRICE!");
            this.maxWaitPrice = price;
        }
        public void AddWaitTax(int price)
        {
            this.waitTaxRaw = price;
            Debug.Log("Wait tax raw is " + waitTaxRaw);
            this.waitTax = this.mode == Mode.Kela ? waitTaxRaw / 3600f : waitTaxRaw / 60f;
            Debug.Log("Wait tax " + waitTax);
            waitTimer = new WaitTimer();
        }
        //PROBLEM: SOME WAIT TAXES IN JSON PER HOUR, OTHERS PER MINUTE?? 
        public void AddWaitTax(Tax otherTax)
        {
            float incoming = otherTax.WaitTax;
            //Kela tax wait is in e/H format, not e/min like in other cases.
            float wait = otherTax.taxMode == Mode.Kela ? incoming / 3600 : incoming / 60; 
            waitTax = wait;
            Debug.Log($"Adding new wait tax {otherTax.waitTaxRaw}");
            waitTimer = otherTax.WaitTimer != null ? otherTax.WaitTimer : new WaitTimer();
        }
        public void MonitorWait()
        {
            if(waitTax == 0 || waitTimer == null)
            {
                return;
            }
            if(maxWaitTime != 0)
            {
                MonitorWaitTime();
            }
            if(maxWaitPrice != 0)
            {
                MonitorWaitPrice();
            }
        }
        void MonitorWaitTime()
        {
            Debug.Log($"Max wait is {maxWaitTime}, wait time is {waitTimer.Time}");
            if(waitTimer.Time >= maxWaitTime)
            {
                Debug.Log("Wait time exceeds max wait, wait has ended");
                StopWait();
                return;
            }
            Debug.Log($"Wait price not exceeding maximum");
        }
        void MonitorWaitPrice()
        {
            Debug.Log($"Max wait price {maxWaitPrice}");
            float price = waitTimer.Time * waitTax;
            Debug.Log($"Current wait price is {price} ({waitTimer.Time} multiplied by tax {waitTax})");
            if(price >= maxWaitPrice)
            {
                Debug.Log("Wait price exceeds maximum, wait has ended");
                StopWait();
                return;
            }
            Debug.Log($"Wait price not exceeding maximum");
        }
        public void StopWait()
        {
            Debug.LogWarning("Max wait reached, moving to checkout!");
            waitTimer.WaitHasEnded = true;
        }
        public void NullTimer()
        {
            waitTimer = null;
        }
        public void AddFixedPrice(int price)
        {
            this.isFixed = true;
            this.fixedPrice = price;
        }
        static Mode GetMode(int category)
        {
            if (category < 10)
            {
                return Mode.Normal;
            }
            else if (category == 10)
            {
                return Mode.Airport;
            }
            else if (category > 10)
            {
                return Mode.Kela;
            }
            else
            {
                return Mode.Normal;
            }
        }
    }
}