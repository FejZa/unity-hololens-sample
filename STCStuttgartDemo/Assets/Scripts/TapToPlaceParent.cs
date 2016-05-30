using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    private EventController _eventController;
    private bool _placing = false;

    private void Awake()
    {
        _eventController = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventController>();
    }

    private void Start()
    {
        _eventController.OnHoloInputGesture += _eventController_OnHoloInputGesture;
    }

    private void OnDestroy()
    {
        _eventController.OnHoloInputGesture -= _eventController_OnHoloInputGesture;
    }

    private void _eventController_OnHoloInputGesture(InputGestureEventArgs args)
    {
        if (args.Type == InteractionSourceType.Tap)
        {
            // On each Select gesture, toggle whether the user is in placing mode.
            _placing = !_placing;

            // If the user is in placing mode, display the spatial mapping mesh.
            SpatialMapping.Instance.DrawVisualMeshes = _placing;
        }
    }

    private void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.
        if (_placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                transform.parent.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                transform.parent.rotation = toQuat;
            }
        }
    }
}
