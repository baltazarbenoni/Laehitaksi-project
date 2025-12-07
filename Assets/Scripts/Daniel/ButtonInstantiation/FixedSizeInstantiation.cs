using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Instantiation
{
    public class FixedSizeInstantiation : ButtonInstantiation, ITaxInstantiation
    {
        XY spacing;
        public XY Spacing => spacing;
        public FixedSizeInstantiation(XY anchor, XY limit, XY spacing, int columnCount) : base (anchor, limit)
        {
            this.spacing = spacing;
            this.columnCount = columnCount;
        }
        public Vector2 GetButtonPosition(XY size, int buttonNum)
        {
            Debug.Log("Instantiating button num " + buttonNum);
            float x = new FixedButtonX(this, columnCount).GetPosition(buttonNum);
            float y = new FixedButtonY(this, columnCount).GetPosition(buttonNum);
            return new Vector2(x, y);
        }   
        public Vector2 GetButtonPosition(int buttonNum)
        {
            Debug.Log("Instantiating button num " + buttonNum);
            float x = new FixedButtonX(this, columnCount).GetPosition(buttonNum);
            float y = new FixedButtonY(this, columnCount).GetPosition(buttonNum);
            return new Vector2(x, y);
        }   
 
    }
    public class FixedButtonXY
    {
        protected FixedSizeInstantiation manager;
        protected XY spacing;
        protected int columnCount;
        public FixedButtonXY(FixedSizeInstantiation manager, int columnCount)
        {
            this.manager = manager;
            this.spacing = manager.Spacing;
            this.columnCount = columnCount;
        }
    }
    public class FixedButtonX : FixedButtonXY 
    {
        float buttonPos;
        float startPos;
        public FixedButtonX(FixedSizeInstantiation manager, int columnCount) : base(manager, columnCount)
        {
            buttonPos = Screen.width / columnCount * 0.5f;
            startPos = -Screen.width / 2f; 
        }
        public float GetPosition(int index)
        {
            int column = index % columnCount + 1;
            int fullColumn = column + (column - 1);
            float pos = startPos + fullColumn * buttonPos;
            return pos;
        }
    }
    public class FixedButtonY : FixedButtonXY
    {
        public FixedButtonY(FixedSizeInstantiation manager, int columnCount) : base(manager, columnCount){}
        public float GetPosition(int index)
        {
            Debug.Log($"Y Spacing is : {spacing.Y}");
            float row = Mathf.CeilToInt(index / columnCount);
            float pos = manager.Anchor.Y - row * spacing.Y;
            return pos;
        }
    }
}