using UnityEngine;

/// <summary>
/// This component enables scaling for the game object it is attached to. It requires a 
/// proper <see cref="EventController"/> setup in the scene, which will notify it about manipulation gestures. 
/// It only works of course if a <see cref="GestureInputController"/> is enabled somewhere in the scene, which
/// is configured to support the manipulation gesture.
/// </summary>
public class GestureScaling : MonoBehaviour
{
    private EventController _eventController;
    private Vector3 _originalScale;
    private Vector3 _previousScale;

    [SerializeField, Tooltip("Controls amount of scale applied.")]
    private float _scaleSensitivity = 1.0f;

    [SerializeField, Tooltip("If set, object will scale proportionally")]
    private bool _preserveScaleAspectRatio;

    private void Awake()
    {
        _eventController = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventController>();
    }

    private void Start()
    {
        _eventController.OnHoloInputGesture += _eventController_OnHoloInputGesture;
        _eventController.OnKeywordRecognized += _eventController_OnKeywordRecognized;
        _originalScale = transform.localScale;
    }

    private void OnDestroy()
    {
        _eventController.OnHoloInputGesture -= _eventController_OnHoloInputGesture;
        _eventController.OnKeywordRecognized -= _eventController_OnKeywordRecognized;
    }

    private void _eventController_OnKeywordRecognized(string phrase)
    {
        if (phrase.Equals("Reset"))
            transform.localScale = _originalScale;
    }

    private void _eventController_OnHoloInputGesture(InputGestureEventArgs args)
    {
        // If the object the gesture was applied to is not the one this script is attached to, 
        // we can ingore this gesture.
        if (args.FocusedObject != gameObject)
            return;

        // We care only for manipulation type gestures.
        switch (args.Type)
        {
            case InteractionSourceType.ManipulationStarted:
            case InteractionSourceType.ManipulationCompleted:
                _previousScale = transform.localScale;
                break;
            case InteractionSourceType.ManipulationUpdated:
                Vector3 appliedScale;
                if (_preserveScaleAspectRatio)
                    appliedScale = ConvertToAspectValidScale(args.ManipulationDelta) * _scaleSensitivity;
                else
                    appliedScale = transform.localScale + (args.ManipulationDelta * _scaleSensitivity);

                transform.localScale = appliedScale;
                break;
            case InteractionSourceType.ManipulationCanceled:
                transform.localScale = _previousScale;
                break;
        }
    }

    private Vector3 ConvertToAspectValidScale(Vector3 inputScale)
    {
        var maxInput = Mathf.Max(inputScale.x, inputScale.y, inputScale.z);
        var scaleInput = new Vector3(maxInput, maxInput, maxInput);
        return transform.localScale + (scaleInput*_scaleSensitivity);
    }
}
