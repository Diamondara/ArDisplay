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

    private Vector2 startPos, direction;
    private float multiplierY = 0.02f;
    private float multiplierX = 0.02f;

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
                         Quaternion targetRotation = Quaternion.LookRotation(forward: direction);
                       // Vector3 targetRotationEuler = Quaternion.LookRotation(forward: direction).eulerAngles;
                       // Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized); //(0,1,0)
                       // Quaternion targetRotation = Quaternion.Euler(euler: scaledEuler);
                       // obj.transform.rotation = obj.transform.rotation * targetRotation;
                    
                    }
                    placed = true;
                }
            }
        }

        if (Input.touchCount == 2){ //Scale model
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

        if (Input.touchCount == 1 && placed) { // Rotate model
            var touch0 = Input.GetTouch(0);
            // save position where touch happened

                    switch (touch0.phase)
                    {
                        case TouchPhase.Began:
                            startPos = touch0.position;
                            direction = Vector3.zero;
                            break;

                        case TouchPhase.Moved:
                            direction = touch0.position - startPos;
                            if (direction.magnitude > 0.05f)
                            {
                                // var localAngles=this.transform.localEulerAngles;
                                // localAngles.y+=direction.y*multiplierY;
                                // this.transform.localEulerAngles=localAngles;

                                var globalAngles = this.transform.eulerAngles;
                                globalAngles.y += -direction.x * multiplierX;
                                this.transform.eulerAngles = globalAngles;

                                var dotVal1 = Vector3.Dot(Camera.main.transform.forward, this.transform.right);
                                var dotVal2 = Vector3.Dot(Camera.main.transform.forward, this.transform.up);
                                var dotVal3 = Vector3.Dot(Camera.main.transform.right, this.transform.up);


                                // Debug.Log("Value"+(dotVal>0?true:false));
                                // MyDebug.Log("Value"+(dotVal>0?true:false));

                                this.transform.localRotation *= Quaternion.Euler(0, -direction.y * multiplierY * (dotVal1 > 0 ? -1 : 1) * (dotVal1 > 0 ? -1 : 1) * (dotVal3 > 0 ? -1 : 1), 0);
                            }
                            break;

                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            break;
                    }
                        


        }
    }
}
