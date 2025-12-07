using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Instantiation
{
 public class NumberButtonInstantiation : ButtonInstantiation
    {
        public NumberButtonInstantiation(XY anchor, XY grid) : base(anchor, grid){}
        public Vector2 GetButtonPosition(int buttonNumber)
        {
            ButtonNumber index = GetIndex(buttonNumber, out columnCount);
            float x = new ButtonX(this, index.X, columnCount).GetPosition();
            float y = new ButtonY(this, index.Y, columnCount).GetPosition();
            return new Vector2(x, y);
        }
        ButtonNumber GetIndex(int num, out int columnCount)
        {
            //Numbers in the 'ButtonNumber'-constructor represent columns and rows for instantiation.
            columnCount = 3;
            //Placing 0 center and down.
            if(num == 11)
            {
                columnCount = 4;
                return new ButtonNumber(3, 11);
                //return new ButtonNumber(10, 11);
            }
            //Placing the 'C' symbol and the 'OK' symbol.
            //C.
            else if(num == 9)
            {
                return new ButtonNumber(-1, 7);
            }
            //OK.
            else if(num == 10)
            {
                columnCount = 4;
                return new ButtonNumber(3, 1);
            }
            //Placing format change button upper left side.
            else if(num == 12)
            {
                return new ButtonNumber(-1, 1);
            }
            //Else according to numerical value;
            else
            {
                return new ButtonNumber(num, num);
            }
        }
    }
}
