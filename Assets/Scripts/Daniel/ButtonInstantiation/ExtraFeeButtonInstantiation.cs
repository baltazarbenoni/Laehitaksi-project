using Instantiation;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

/// <summary>
/// Extends ButtonInstantiation to provide Vector2 button positioning for UI elements.
/// Calculates grid-based positions for extra fee buttons with a fixed 2-column layout.
/// </summary>
public class ExtraFeeButtonInstantiation : ButtonInstantiation 
{
    /// <summary>
    /// Initializes extra fee button instantiation with anchor point and grid spacing.
    /// </summary>
    /// <param name="anchor">The starting position (top-left corner) of the button grid</param>
    /// <param name="grid">The spacing between grid cells (X and Y distance)</param>
    public ExtraFeeButtonInstantiation(XY anchor, XY grid) : base(anchor, grid) { }

    /// <summary>
    /// Calculates the combined Vector2 position for a button at the specified index.
    /// Uses a fixed 2-column grid layout.
    /// </summary>
    /// <param name="buttonNumber">The button's index in the grid (0-based)</param>
    /// <returns>A Vector2 containing the calculated X and Y position</returns>
    public Vector2 GetButtonPosition(int buttonNumber)
    {
        float x = new ButtonX(this, buttonNumber, 2).GetPosition();
        float y = new ButtonY(this, buttonNumber, 2).GetPosition();
        return new Vector2(x, y);
    }
}