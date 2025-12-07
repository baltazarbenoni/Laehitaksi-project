using Navigation;
using Taxes;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Input
{
    public class InputFormatManager
    {
        InputFormat timeFormat = new InputFormat("min", Format.MIN);
        InputFormat moneyFormat = new MoneyFormat("€", Format.EURO);
        InputFormat percentageFormat = new InputFormat("%", Format.PERCENT);
        InputFormat normalFormat = new InputFormat("", Format.NORMAL);

        public InputFormat[] GetFormats(InputType type)
        {
            if(type == InputType.MaxTime)
            {
                return new InputFormat[] { timeFormat, moneyFormat };
            }
            else if(type == InputType.MaxPrice || type == InputType.Addition)
            {
                return new InputFormat[] { moneyFormat };
            }
            else if(type == InputType.Partial)
            {
                return new InputFormat[] { moneyFormat, percentageFormat };
            }
            else
            {
                return new InputFormat[] { normalFormat };
            }
        }
    }
    public class InputMode
    {
        public InputMode(InputType type)
        {
            this.type = type;
            this.formats = new InputFormatManager().GetFormats(type);
            this.selectedFormat = formats[0];
            this.baseText = GetBaseText();
        }
        public InputMode(){}
        InputType type;
        internal InputType inputType => type;
        InputFormat[] formats = new InputFormat[2];
        InputFormat selectedFormat;
        public string Id => GetId();
        public InputFormat Current => selectedFormat;
        int index;
        string baseText;
        public string BaseText => baseText;
        public void ChangeFormat()
        {
            selectedFormat = Iteration.GetNext(formats, ref index);
            baseText = GetBaseText();
        }
        public void SetCurrent(int newIndex)
        {
            index = newIndex; 
            selectedFormat = Iteration.GetAt(formats, ref index);
        }
        public static InputMode GetMode(InputType type)
        {
            return new InputMode(type);
        }
        public static bool HasMultipleFormats(InputType type)
        {
            InputMode mode = new InputMode(type);
            bool hasMany = mode.formats.Length > 1;
            Debug.Log($"Input mode {mode.inputType} has multiple formats: {hasMany}");
            return hasMany;
        }
        string GetId()
        {
            Debug.Log($"Getting id for format {type}");
            string id = "";
            int counter = 0;
            foreach(var format in formats)
            {
                string addition = formats.Length>1 && counter<formats.Length-1 ? "/" : "";
                id += format.Id + addition;
                counter++;
            }
            return id;
        }
        public static string GetId(InputType type)
        {
            string id = new InputMode(type).Id;
            return id;
        }
        string GetBaseText()
        {
            Debug.Log($"Updating text for format: {selectedFormat.Format}");
            return selectedFormat.Format switch
            {
                Format.NORMAL => "0",
                Format.PERCENT => "0",
                Format.MIN => "0",
                Format.EURO => "0,00",
                _ => "0"
            };
        }

    }
}
