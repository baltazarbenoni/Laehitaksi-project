using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles sprite animation for UI Image components.
/// Cycles through an array of sprites at configurable intervals with optional looping.
/// </summary>
public class ImageAnimation : MonoBehaviour {

    [SerializeField] Sprite[] sprites; // Array of sprites to animate through
    [SerializeField] float timeBetweenSprites; // Delay between sprite changes (in seconds)
    float timer; // Tracks elapsed time since last sprite change
    public bool loop = false; // Whether to restart animation after completion
    public bool disableOnEnd = false; // Whether to deactivate the GameObject when animation ends
    int index = 0; // Current sprite index
    Image image; // Reference to the UI Image component

    /// <summary>
    /// Initializes the Image component reference.
    /// </summary>
    void Awake() {
        image = GetComponent<Image> ();
    }

    /// <summary>
    /// Updates sprite animation each frame.
    /// </summary>
    void Update () {
        // Exit early if animation is not looping and has completed
        if (!loop && index == sprites.Length)
        {
            return;
        }
        
        timer += Time.deltaTime;
        if(timer < GetTimerLimit()) return;

        // Apply current sprite and reset timer
        image.sprite = sprites[index];
        timer = 0;
        index ++;

        // Handle animation completion
        if (index >= sprites.Length)
        {
            Actions.PaymentAnimationFinished?.Invoke();
            OnEnd();
        }
    }

    /// <summary>
    /// Handles animation completion logic (looping or disabling).
    /// </summary>
    void OnEnd()
    {
        if (loop) index = 0;
        if (disableOnEnd)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Returns the time delay before the next sprite change.
    /// First and second-to-last sprites have 2x the normal delay.
    /// </summary>
    float GetTimerLimit()
    {
        if(index == 0 || index == sprites.Length - 2)
        {
            return timeBetweenSprites * 2f;
        }
        return timeBetweenSprites;
    }
}
