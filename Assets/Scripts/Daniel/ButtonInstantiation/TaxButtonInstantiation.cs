using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Instantiation
{
    public interface ITaxInstantiation
    {
        public Vector2 GetButtonPosition(XY size, int buttonNum);
    }
    public class TaxButtonInstantiation : ButtonInstantiation, ITaxInstantiation
    {
        public TaxButtonInstantiation(XY anchor, XY limit, XY spacing) : base(anchor, limit)
        {
            this.spacing = spacing;
            previous = new ButtonData(0, 0);
        }
        public TaxButtonInstantiation(XY spacing) : base()
        {
            this.spacing = spacing;
            previous = new ButtonData(0, 0);
        }
        ButtonData previous;
        public ButtonData Previous { get { return previous; } }
        protected XY spacing;
        public XY Spacing { get { return spacing; } }

        public void AdjustPrevious(float pos, float size)
        {
            previous.ChangeData(pos, size);
        }

        public Vector2 GetButtonPosition(XY size, int buttonNum)
        {
            float x = new TaxButtonX(this, size.X).GetPosition();
            float y = new TaxButtonY(this, size.Y).GetPosition();
            return new Vector2(x, y);
        }
    }
    
    public class TaxButtonXY
    {
        protected TaxButtonInstantiation manager;
        protected ButtonData previous;
        protected XY spacing;
        protected float size;
        public TaxButtonXY(TaxButtonInstantiation manager, float size)
        {
            this.manager = manager;
            this.previous = manager.Previous;
            this.spacing = manager.Spacing;
            this.size = size;
        }
        public virtual float GetPosition()
        {
            return 0;
        }
    }
    public class TaxButtonX : TaxButtonXY
    {
        public TaxButtonX(TaxButtonInstantiation manager, float size) : base(manager, size){}

        //Get the x-position of the current button and update manager. 
        public override float GetPosition()
        {
            float pos = previous.Pos + previous.Size + spacing.X + size / 2f;
            //If position would be over the set boundary, go to a new row.
            if(pos > manager.Grid.X)
            {
                Debug.Log("Moving to a new row");
                pos = spacing.X + size / 2f;
                manager.AddRow();
            }
            manager.AdjustPrevious(pos, size / 2f);
            Debug.Log(pos + manager.Anchor.X);
            return pos + manager.Anchor.X;
        }
    }
    public class TaxButtonY : TaxButtonXY
    {
        public TaxButtonY(TaxButtonInstantiation manager, float size) : base(manager, size){}
        public override float GetPosition()
        {
            float pos = manager.Anchor.Y - manager.RowNumber * (size + spacing.Y);
            return pos;
        }
    }
    public class TaxButtonInstantiationComission : ButtonInstantiation, ITaxInstantiation
    {
        //Use this to get rowlike placement for buttons.
        public TaxButtonInstantiationComission(XY anchor, XY limit) : base(anchor, limit)
        {
            ConvertValues(ref this.anchor, ref this.grid);
        }
        public Vector2 GetButtonPosition(XY size, int buttonNum)
        {
            Debug.Log("Instantiating button num " + buttonNum);
            float x = new ButtonX(this, buttonNum, 3).GetPosition();
            float y = new ButtonY(this, buttonNum, 3).GetPosition();
            return new Vector2(x, y);
        }
        //Convert anchor and limit values to ones that suit different instantiation method.
        //When not on comission, tax buttons are instantiated according to buttonsize.
        //Now ignore button size and organize buttons to columns.
        void ConvertValues(ref XY anchor, ref XY limit)
        {
            anchor = new XY(anchor.X - 750f, anchor.Y);
            limit = new XY(limit.X - 900f, limit.Y + 175f);
            Debug.Log($"Anchor {anchor.X}, {anchor.Y}, limit {limit.X}, {limit.Y}");
        }
    }
}
