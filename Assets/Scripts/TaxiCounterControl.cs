using UnityEngine;
using TMPro; // Required for TextMeshPro

/// <summary>
/// Manages a simple counter that is updated by a UI button.
/// Plays a click sound with increasing pitch and resets after 10.
/// Optimized for WebGL by avoiding Update() and caching references.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TaxiCounterControl : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The TextMeshPro UI element that displays the count.")]
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Counter Settings")]
    [Tooltip("The number at which the counter will reset on the next click.")]
    [SerializeField] private int maxCount = 10;

    [Header("Audio Settings")]
    [Tooltip("The audio clip to play on each button click.")]
    [SerializeField] private AudioClip clickSound;
    [Tooltip("The range for the pitch increase. X = Min, Y = Max.")]
    [SerializeField] private Vector2 pitchRange = new(1.0f, 2.0f);

    // Internal variables
    private AudioSource audioSource;
    private int currentCount = 0;
    private float currentPitch;
    private float pitchStep;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Used here for initial setup and caching.
    /// </summary>
    private void Awake()
    {
        // Cache the AudioSource component.
        audioSource = GetComponent<AudioSource>();

        // Error checks for required components.
        if (counterText == null)
        {
            Debug.LogError("Counter Text is not assigned in the TaxiCounterControl script!", this);
            this.enabled = false;
            return;
        }
        if (clickSound == null)
        {
            Debug.LogWarning("Click Sound is not assigned in the TaxiCounterControl script.", this);
        }

        // Initialize the counter and pitch.
        ResetCounterAndPitch();
    }

    /// <summary>
    /// This public method is designed to be called by a UI Button's OnClick event.
    /// It increments the counter, plays the sound, and handles the reset logic.
    /// </summary>
    public void IncrementCounter()
    {
        // First, check if the counter has reached its maximum.
        if (currentCount >= maxCount)
        {
            // If so, reset the counter and pitch values *before* doing anything else.
            ResetCounterAndPitch();
        }
        else
        {
            // Otherwise, increment the count and the pitch for the next sound.
            currentCount++;
            currentPitch += pitchStep;
        }

        // Now that the values are correct for the current state, play the sound.
        PlayClickSound();

        // Finally, update the text display.
        UpdateCounterText();
    }

    /// <summary>
    /// Plays the assigned click sound with the current pitch value.
    /// </summary>
    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            // Set the pitch on the AudioSource.
            audioSource.pitch = currentPitch;
            // Play the sound. PlayOneShot is ideal for rapid, non-interrupting sounds.
            audioSource.PlayOneShot(clickSound);

        }
    }

    /// <summary>
    /// Resets the counter to zero and the pitch to its minimum value.
    /// </summary>
    private void ResetCounterAndPitch()
    {
        currentCount = 0;
        currentPitch = pitchRange.x;
        // Calculate how much to increase the pitch on each step to reach the max pitch at the max count.
        pitchStep = (pitchRange.y - pitchRange.x) / maxCount;

    }

    /// <summary>
    /// Updates the TextMeshPro text to display the current count.
    /// </summary>
    private void UpdateCounterText()
    {
        counterText.SetText(currentCount.ToString());
    }
}