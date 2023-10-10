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
    private bool placed = false;
    private GameObject obj;
    private float initialDistance;
    private Vector3 initialScale;

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
        if (finger.index != 0 && !placed) return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) { //spawn object
            foreach (ARRaycastHit hit in hits) {
                Pose pose = hit.pose;
                // check if object to spawn does already exist in scene
                if (!placed)
                {
                    obj = Instantiate(prefab, position: pose.position, pose.rotation);

                    if (aRPlaneManager.GetPlane(trackableId: hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        Vector3 position = obj.transform.position;
                        Vector3 cameraPosition = Camera.main.transform.position;
                        Vector3 direction = cameraPosition - position;

                        // Rotation anpassen, damit haupteingang immer vorne ist bei spawn
                        // Quaternion targetRotation = Quaternion.LookRotation(forward: direction);
                        Vector3 targetRotationEuler = Quaternion.LookRotation(forward: direction).eulerAngles;
                        Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized); //(0,1,0)
                        Quaternion targetRotation = Quaternion.Euler(euler: scaledEuler);
                        obj.transform.rotation = obj.transform.rotation * targetRotation;
                    
                    }
                    placed = true;
                }
            }
        }

        if (Input.touchCount == 2){
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Ended || touch0.phase == TouchPhase.Canceled ||
               touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled){
                return;
            }

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch0.position, touch1.position);
                initialScale = obj.transform.localScale;
            }
            else {
                var currentDistance = Vector2.Distance(touch0.position, touch1.position);
                if (Mathf.Approximately(initialDistance,0)) {
                    return;
                }
                var factor = currentDistance / initialDistance;
                obj.transform.localScale = initialScale * factor;
            
            }

        }
    }
}
