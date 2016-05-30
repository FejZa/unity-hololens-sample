using UnityEngine;

/// <summary>
/// This component enables horizontal rotation along the y-axis for the game object
/// it is attached to. It requires a proper <see cref="EventController"/> setup in the scene,
/// which will notify it about navigation gestures. It only works of course if a <see cref="GestureInputController"/>
/// is enabled somewhere in the scene.
/// </summary>
public class GestureRotation : MonoBehaviour
{
    private EventController _eventController;
    private Quaternion _originalRotation;
    private Quaternion _previousRotation;

    [SerializeField, Tooltip("Controls amount of rotation applied.")]
    private float _rotationSensitivity = 5.0f;

    private void Awake()
    {
        _eventController = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventController>();
    }

    private void Start()
    {
        _eventController.OnHoloInputGesture += _eventController_OnHoloInputGesture;
        _eventController.OnKeywordRecognized += _eventController_OnKeywordRecognized;
        _originalRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        _eventController.OnHoloInputGesture -= _eventController_OnHoloInputGesture;
        _eventController.OnKeywordRecognized -= _eventController_OnKeywordRecognized;
    }

    private void _eventController_OnKeywordRecognized(string phrase)
    {
        if (phrase.Equals("Reset"))
            transform.rotation = _originalRotation;
    }

    private void _eventController_OnHoloInputGesture(InputGestureEventArgs args)
    {
        // If the object the gesture was applied to is not the one this script is attached to, 
        // we can ingore this gesture.
        if (args.FocusedObject != gameObject)
            return;

        // We care only for navigation type gestures.
        switch (args.Type)
        {
            case InteractionSourceType.NavigationStarted:
            case InteractionSourceType.NavigationCompleted:
                _previousRotation = transform.rotation;
                break;
            case InteractionSourceType.NavigationUpdated:
                var rotationFactor = args.NavigationPosition.x*_rotationSensitivity;
                transform.Rotate(new Vector3(0, -1*rotationFactor, 0));
                break;
            case InteractionSourceType.NavigationCanceled:
                transform.rotation = _previousRotation;
                break;
        }
    }
}
