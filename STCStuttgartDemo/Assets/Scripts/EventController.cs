using UnityEngine;

public delegate void HoloInputGestureDelegate(InputGestureEventArgs args);
public delegate void HoloGazeFocusChangedDelegate(GameObject focusedObject);
public delegate void KeywordRecognizedDelegate(string phrase);
public delegate void HandDetectionChangedDelegate(bool handDetected);

public class EventController : MonoBehaviour
{
    /// <summary>
    /// Event invoked whenever any of the input controllers detects an input action.
    /// </summary>
    public event HoloInputGestureDelegate OnHoloInputGesture;

    /// <summary>
    /// Event invoked whenever a keyword is recognized by the <see cref="SpeechController"/>.
    /// </summary>
    public event KeywordRecognizedDelegate OnKeywordRecognized;

    /// <summary>
    /// Event invoked whenever the <see cref="GazeController"/> detects a focus change in the user's gaze.
    /// </summary>
    public event HoloGazeFocusChangedDelegate OnGazeFocusChanged;

    /// <summary>
    /// Event invoked whenever the <see cref="HandController"/> detects or loses the user's hand.
    /// </summary>
    public event HandDetectionChangedDelegate OnHandDetectionChanged;

    /// <summary>
    /// Notify all subscribed listeners about a hand detection change.
    /// </summary>
    /// <param name="handDetected">The new state of the hand detection.</param>
    public void NotifyHandDetectionChanged(bool handDetected)
    {
        if (OnHandDetectionChanged != null)
            OnHandDetectionChanged(handDetected);
    }

    /// <summary>
    /// Notify all subscribed listeners about a gaze focus change.
    /// </summary>
    /// <param name="focusObject">The now focused game object.</param>
    public void NotifyGazeFocusChanged(GameObject focusObject)
    {
        if (OnGazeFocusChanged != null)
            OnGazeFocusChanged(focusObject);
    }

    /// <summary>
    /// Notify all subscribed listeners about a recognized speech keyword.
    /// </summary>
    /// <param name="keyword">The recognized keyword.</param>
    public void NotifyKeywordRecognized(string keyword)
    {
        if (OnKeywordRecognized != null)
            OnKeywordRecognized(keyword);
    }

    /// <summary>
    /// Notify all subscribed listeners about a registered input action.
    /// </summary>
    /// <param name="args">Input arguments with additional information about the input action.</param>
    public void NotifyHoloInputGesture(InputGestureEventArgs args)
    {
        if (OnHoloInputGesture != null)
            OnHoloInputGesture(args);
    }
}
