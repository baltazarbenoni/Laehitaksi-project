using Navigation;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Input
{
    public class InputFormat
    {
        public InputFormat(string abbrev, Format format)
        {
            this.id = abbrev;
            this.format = format;
        }
        protected string id = "";
        public string Id => id; 
        protected Format format;
        public Format Format => format;
        public virtual string GetFormattedText(int sum)
        {
            string text = $"{sum}{id}";
            return text;
       }
    }
    public class MoneyFormat : InputFormat
    {
        public MoneyFormat(string abbrev, Format format) : base(abbrev, format){}
        public override string GetFormattedText(int sum)
        {
            int aboveZero = sum / 100;
            int decimals = sum - aboveZero * 100;
            string addition1 = Conversion.GetZerosToAdd(decimals);
            string txt = $"{aboveZero},{addition1}{decimals}{id}";
            return txt;
        }
    }
}