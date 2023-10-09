using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent: typeof(ARRaycastManager),
    requiredComponent2:typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool placed = 0;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger) {
        if (finger.index != 0) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
            foreach (ARRaycastHit hit in hits) {
                Pose pose = hit.pose;
                // check if object to spawn does already exist in scene
                if (placed == 0)
                {
                    GameObject obj = Instantiate(prefab, position: pose.position, pose.rotation);

                    if (aRPlaneManager.GetPlane(trackableId: hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        Vector3 position = obj.transform.position;
                        Vector3 cameraPosition = Camera.main.transform.position;
                        Vector3 direction = cameraPosition - position;
                        // Quaternion targetRotation = Quaternion.LookRotation(forward: direction);
                        Vector3 targetRotationEuler = Quaternion.LookRotation(forward: direction).eulerAngles;
                        Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized); //(0,1,0)
                        Quaternion targetRotation = Quaternion.Euler(euler: scaledEuler);
                        obj.transform.rotation = obj.transform.rotation * targetRotation;
                    }
                    placed = 1;
                }
            }
        }
    }
}
