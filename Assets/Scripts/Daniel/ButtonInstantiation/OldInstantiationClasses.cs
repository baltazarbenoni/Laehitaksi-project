using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni
namespace Instantiation
{
    public class ExtraFeeButtonXY : ButtonXY
    {
        public ExtraFeeButtonXY(ButtonInstantiation manager, int index) : base(manager, index){}
        public ExtraFeeButtonXY(ButtonInstantiation manager, int index, int columnCount) : base (manager, index, columnCount) {}
    }
    public class ExtraFeeButtonX : ButtonX
    {
        public ExtraFeeButtonX(ExtraFeeButtonInstantiation manager, int index, int columnCount) : base(manager, index, columnCount){}
    }
    public class ExtraFeeButtonY : ButtonY
    {
        public ExtraFeeButtonY(ExtraFeeButtonInstantiation manager, int index, int columnCount) : base(manager, index, columnCount) { }
    }
    public class NumberButtonXY
    {
        public NumberButtonXY(NumberButtonInstantiation manager, int index)
        {
            this.index = index;
            this.manager = manager;
        }
        protected NumberButtonInstantiation manager;
        protected int index;
        public int Index { get { return index; } }
    }
    public class NumberButtonX : NumberButtonXY
    {
        public NumberButtonX(NumberButtonInstantiation manager, int index) : base(manager, index){}
        public float GetPosition()
        {
            float column = this.index % 3 + 1;
            float pos = column * manager.Grid.X + manager.Anchor.X;
            return pos;
        }
    }
    public class NumberButtonY : NumberButtonXY
    {
        public NumberButtonY(NumberButtonInstantiation manager, int index) : base(manager, index){}
        public float GetPosition()
        {
            float row = Mathf.CeilToInt(this.index / 3);
            float pos = manager.Anchor.Y - row * manager.Grid.Y;
            return pos;
        }
    }   
}