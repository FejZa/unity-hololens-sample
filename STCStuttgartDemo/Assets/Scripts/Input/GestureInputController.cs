using UnityEngine;
using UnityEngine.VR.WSA.Input;

[RequireComponent(typeof (EventController))]
public class GestureInputController : MonoBehaviour
{
    private EventController _eventController;
    private GestureRecognizer _activeRecognizer;
    private GestureRecognizer _manipulationRecognizer;
    private GestureRecognizer _navigationRecognizer;
    private GameObject _focusedObject;

    public GestureRecognizer ActiveRecognizer
    {
        get { return _activeRecognizer; }
        set
        {
            _activeRecognizer.CancelGestures();
            _activeRecognizer.StopCapturingGestures();
            _activeRecognizer = value;
            _activeRecognizer.StartCapturingGestures();
        }
    }

    private void Awake()
    {
        _eventController = GetComponent<EventController>();
    }

    private void Start()
    {
        // We need to know when the gazed at object changes, so we can start and stop
        // tracking gestures.
        _eventController.OnGazeFocusChanged += _eventController_OnGazeFocusChanged;
        _eventController.OnKeywordRecognized += _eventController_OnKeywordRecognized;

        InitManipulationRecognizer();
        InitNavigationRecognizer();

        // By default enable the navigation recognizer. There can only be one 
        // recognizer active at a time, since manipulation and navigation interfere.
        _activeRecognizer = _navigationRecognizer;
    }

    private void OnDestroy()
    {
        _eventController.OnGazeFocusChanged -= _eventController_OnGazeFocusChanged;
        _eventController.OnKeywordRecognized -= _eventController_OnKeywordRecognized;
    }

    private void InitManipulationRecognizer()
    {
        _manipulationRecognizer = new GestureRecognizer();
        _manipulationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.ManipulationTranslate);

        _manipulationRecognizer.TappedEvent += (source, tapCount, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
        {
            FocusedObject = _focusedObject,
            Ray = ray,
            Kind = source,
            TapCount = tapCount,
            Type = InteractionSourceType.Tap
        });

        _manipulationRecognizer.ManipulationStartedEvent +=
            (source, delta, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.ManipulationStarted,
                ManipulationDelta = delta
            });

        _manipulationRecognizer.ManipulationUpdatedEvent +=
            (source, delta, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.ManipulationUpdated,
                ManipulationDelta = delta
            });

        _manipulationRecognizer.ManipulationCompletedEvent +=
            (source, delta, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.ManipulationCompleted,
                ManipulationDelta = delta
            });

        _manipulationRecognizer.ManipulationCanceledEvent +=
            (source, delta, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.ManipulationCanceled,
                ManipulationDelta = delta
            });
    }

    private void InitNavigationRecognizer()
    {
        _navigationRecognizer = new GestureRecognizer();
        _navigationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.NavigationX | GestureSettings.NavigationY | GestureSettings.NavigationZ);

        _navigationRecognizer.TappedEvent += (source, tapCount, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
        {
            FocusedObject = _focusedObject,
            Ray = ray,
            Kind = source,
            TapCount = tapCount,
            Type = InteractionSourceType.Tap
        });

        _navigationRecognizer.NavigationStartedEvent +=
            (source, offset, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.NavigationStarted,
                NavigationPosition = offset
            });

        _navigationRecognizer.NavigationUpdatedEvent +=
            (source, offset, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.NavigationUpdated,
                NavigationPosition = offset
            });

        _navigationRecognizer.NavigationCompletedEvent +=
            (source, offset, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.NavigationCompleted,
                NavigationPosition = offset
            });

        _navigationRecognizer.NavigationCanceledEvent +=
            (source, offset, ray) => _eventController.NotifyHoloInputGesture(new InputGestureEventArgs
            {
                FocusedObject = _focusedObject,
                Ray = ray,
                Kind = source,
                Type = InteractionSourceType.NavigationCanceled,
                NavigationPosition = offset
            });
    }

    private void _eventController_OnKeywordRecognized(string phrase)
    {
        switch (phrase)
        {
            case "Scale":
                ActiveRecognizer = _manipulationRecognizer;
                break;
            case "Rotate":
                ActiveRecognizer = _navigationRecognizer;
                break;
        }
    }

    private void _eventController_OnGazeFocusChanged(GameObject focusedObject)
    {
        _focusedObject = focusedObject;
        _activeRecognizer.CancelGestures();
        _activeRecognizer.StopCapturingGestures();

        // Only if we are actually gazing at something, we want to track gestures.
        if (_focusedObject != null)
            _activeRecognizer.StartCapturingGestures();
    }
}
