using UnityEngine;

[RequireComponent(typeof (EventController))]
public class GazeController : MonoBehaviour
{
    private EventController _eventController;

    /// <summary>
    /// Represents the hologram that is currently being gazed at.
    /// </summary>
    public GameObject FocusedObject { get; private set; }

    private void Awake()
    {
        _eventController = GetComponent<EventController>();
    }

    private void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // notify the system about the focus change.
        if (FocusedObject != oldFocusObject)
            _eventController.NotifyGazeFocusChanged(FocusedObject);
    }
}
