using UnityEngine;
using UnityEngine.VR.WSA.Input;

public enum InteractionSourceType
{
    Tap,
    ManipulationStarted,
    ManipulationUpdated,
    ManipulationCompleted,
    ManipulationCanceled,
    NavigationStarted,
    NavigationUpdated,
    NavigationCompleted,
    NavigationCanceled
}

public class InputGestureEventArgs
{
    /// <summary>
    /// Gets the type of the input gesture.
    /// </summary>
    public InteractionSourceType Type { get; set; }

    /// <summary>
    /// Gets the object that was focused when the gesture occured.
    /// </summary>
    public GameObject FocusedObject { get; set; }

    /// <summary>
    /// Gets the source kind of the gesture.
    /// </summary>
    public InteractionSourceKind Kind { get; set; }

    /// <summary>
    /// Gets the number of taps recorded.
    /// </summary>
    public int TapCount { get; set; }

    /// <summary>
    /// Gets the manipulation delta vector that indicates how far the user's hand has moved.
    /// <remarks>Only relevant when <see cref="Type"/> of gesture is manipulation.</remarks>
    /// </summary>
    public Vector3 ManipulationDelta { get; set; }

    /// <summary>
    /// Gets the relative position of the user's hand to the object the gesture was applied to.
    /// <remarks>Only relevant when <see cref="Type"/> of gesture is navigation.</remarks>
    /// </summary>
    public Vector3 NavigationPosition { get; set; }

    /// <summary>
    /// Gets the ray that hit the focused object at gesture occurrence.
    /// </summary>
    public Ray Ray { get; set; }
}
