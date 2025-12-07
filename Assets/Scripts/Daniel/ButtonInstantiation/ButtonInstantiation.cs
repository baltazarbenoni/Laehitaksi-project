using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Instantiation
{
    /// <summary>
    /// Manages grid-based button instantiation with anchor and grid spacing configuration.
    /// Tracks button positioning within a grid layout system.
    /// </summary>
    public class ButtonInstantiation
    {
        /// <summary>
        /// Initializes button instantiation with specified anchor point and grid spacing.
        /// </summary>
        /// <param name="anchor">The starting position (top-left corner) of the grid</param>
        /// <param name="grid">The spacing between grid cells (X and Y distance)</param>
        public ButtonInstantiation(XY anchor, XY grid)
        {
            this.anchor = anchor;
            this.grid = grid;
        }

        /// <summary>
        /// Initializes button instantiation with default zero values.
        /// </summary>
        public ButtonInstantiation()
        {
            anchor = new XY(0, 0);
            grid = new XY(0, 0);
        }

        protected XY anchor; // Grid anchor/starting position
        /// <summary>Gets the grid anchor point.</summary>
        public XY Anchor { get { return anchor; } }

        protected XY grid; // Spacing between grid cells
        /// <summary>Gets the grid cell spacing.</summary>
        public XY Grid { get { return grid; } }

        protected int columnCount; // Number of columns in the grid
        float column; // Current column index
        /// <summary>Gets the current column index.</summary>
        public float Column { get { return column; } }

        float rowNumber; // Current row index
        /// <summary>Gets the current row index.</summary>
        public float RowNumber { get { return rowNumber; } }

        /// <summary>Increments the row counter.</summary>
        public void AddRow()
        {
            this.rowNumber++;
        }

        /// <summary>Increments the column counter.</summary>
        public void AddColumn()
        {
            this.column++;
        }

        /// <summary>Resets the column counter to zero.</summary>
        public void ZeroColumn()
        {
            this.column = 0;
        }
    }

    #region XY
    /// <summary>
    /// Represents a 2D coordinate with float precision.
    /// Used for grid positioning and spacing.
    /// </summary>
    public struct XY
    {
        /// <summary>Initializes a new XY coordinate.</summary>
        public XY(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        float x; // X coordinate
        /// <summary>Gets the X coordinate.</summary>
        public float X { get { return x; } }

        float y; // Y coordinate
        /// <summary>Gets the Y coordinate.</summary>
        public float Y { get { return y; } }
    }
    #endregion

    #region Number
    /// <summary>
    /// Represents a 2D grid index with integer precision.
    /// Used for button numbering in the grid.
    /// </summary>
    public struct ButtonNumber
    {
        /// <summary>Initializes a new button grid index.</summary>
        public ButtonNumber(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        int x; // X index
        /// <summary>Gets the X index.</summary>
        public int X { get { return x; } }

        int y; // Y index
        /// <summary>Gets the Y index.</summary>
        public int Y { get { return y; } }
    }
    #endregion

    #region Data
    /// <summary>
    /// Stores button positioning and sizing data.
    /// </summary>
    public class ButtonData
    {
        /// <summary>
        /// Initializes button data with position and size.
        /// </summary>
        /// <param name="pos">Button position</param>
        /// <param name="size">Button size</param>
        public ButtonData(float pos, float size)
        {
            this.pos = pos;
            this.size = size;
        }

        float pos; // Button position
        /// <summary>Gets the button position.</summary>
        public float Pos { get { return pos; } }

        float size; // Button size
        /// <summary>Gets the button size.</summary>
        public float Size { get { return size; } }

        /// <summary>
        /// Updates button size and position.
        /// </summary>
        /// <param name="size">New button size</param>
        /// <param name="pos">New button position</param>
        public void ChangeData(float size, float pos)
        {
            this.size = size;
            this.pos = pos;
        }
    }
    #endregion

    #region XY
    /// <summary>
    /// Base class for calculating button positions within a grid.
    /// Manages button index and grid dimensions.
    /// </summary>
    public class ButtonXY
    {
        /// <summary>
        /// Initializes button position calculator with manager, index, and column count.
        /// </summary>
        public ButtonXY(ButtonInstantiation manager, int index, int columnCount)
        {
            this.index = index;
            this.manager = manager;
            this.columnCount = columnCount;
        }

        /// <summary>
        /// Initializes button position calculator with default 2-column grid.
        /// </summary>
        public ButtonXY(ButtonInstantiation manager, int index)
        {
            this.index = index;
            this.manager = manager;
            this.columnCount = 2;
        }

        protected int index; // Button index in grid
        protected int columnCount; // Grid column count
        protected ButtonInstantiation manager; // Reference to grid manager
    }
    #endregion

    #region X
    /// <summary>
    /// Calculates horizontal (X) position for a button based on its grid index.
    /// </summary>
    public class ButtonX : ButtonXY
    {
        /// <summary>Initializes X position calculator with custom column count.</summary>
        public ButtonX(ButtonInstantiation manager, int index, int columnCount) : base(manager, index, columnCount) { }

        /// <summary>Initializes X position calculator with default 2-column grid.</summary>
        public ButtonX(ButtonInstantiation manager, int index) : base(manager, index) { }

        /// <summary>
        /// Calculates the X position based on button index and grid spacing.
        /// </summary>
        /// <returns>The calculated X position relative to grid anchor</returns>
        public float GetPosition()
        {
            float column = index % columnCount + 1; // Calculate which column this button occupies
            float pos = column * manager.Grid.X + manager.Anchor.X; // Apply grid spacing and anchor offset
            return pos;
        }
    }
    #endregion

    #region Y
    /// <summary>
    /// Calculates vertical (Y) position for a button based on its grid index.
    /// </summary>
    public class ButtonY : ButtonXY
    {
        /// <summary>Initializes Y position calculator with custom column count.</summary>
        public ButtonY(ButtonInstantiation manager, int index, int columnCount) : base(manager, index, columnCount) { }

        /// <summary>Initializes Y position calculator with default 2-column grid.</summary>
        public ButtonY(ButtonInstantiation manager, int index) : base(manager, index) { }

        /// <summary>
        /// Calculates the Y position based on button index and grid spacing.
        /// Positions decrease downward from anchor.
        /// </summary>
        /// <returns>The calculated Y position relative to grid anchor</returns>
        public float GetPosition()
        {
            float row = Mathf.CeilToInt(this.index / columnCount); // Calculate which row this button occupies
            float pos = manager.Anchor.Y - row * manager.Grid.Y; // Apply grid spacing downward from anchor
            return pos;
        }
    }
    #endregion
}