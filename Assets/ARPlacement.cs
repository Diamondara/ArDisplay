using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARPlaneManager))]

public class ARPlacement : MonoBehaviour
{
    public GameObject sessionOrigin;
    public GameObject arPlaneManager;
    public GameObject arObjectToSpawn;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private float initialDistance;
    private Vector3 initialScale;
    private bool isPlaced = false;
    public bool enableTouchControls = true;
    TextMeshProUGUI testtext;
    TextMeshProUGUI massstab;

    private Vector2 startPos;
    private Vector2 direction;
    private float multiplierY = 0.02f;
    private float multiplierX = 0.02f;

    void Start()
    {
        TouchSimulation.Enable();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
      
    }

    // need to update placement indicator, placement pose and spawn 
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject(); // at the moment this just spawns the gameobject
        }

        // scale using pinch involves two touches
        // we need to count both the touches, store it somewhere, measure the distance between pinch 
        // and scale gameobject depending on the pinch distance
        // we also need to ignore if the pinch distance is small (cases where two touches are registered accidently)

        if (Input.touchCount == 1 && isPlaced && enableTouchControls) { //Rotate Object with 1 Finger

            var touchZero = Input.GetTouch(0);
            // save position where touch happened

            switch (touchZero.phase)
            {
                case TouchPhase.Began:
                    startPos = touchZero.position;
                    direction = Vector3.zero;
                    break;

                case TouchPhase.Moved:
                    direction = touchZero.position - startPos;
                    if (direction.magnitude > 0.05f)
                    { 
                        // var localAngles=this.transform.localEulerAngles;
                        // localAngles.y+=direction.y*multiplierY;
                        // this.transform.localEulerAngles=localAngles;

                        var globalAngles = spawnedObject.transform.eulerAngles;
                        globalAngles.y += -direction.x * multiplierX;
                        spawnedObject.transform.eulerAngles = globalAngles;

                        var dotVal1 = Vector3.Dot(Camera.main.transform.forward, spawnedObject.transform.right);
                        var dotVal2 = Vector3.Dot(Camera.main.transform.forward, spawnedObject.transform.up);
                        var dotVal3 = Vector3.Dot(Camera.main.transform.right, spawnedObject.transform.up);


                        // Debug.Log("Value"+(dotVal>0?true:false));
                        // MyDebug.Log("Value"+(dotVal>0?true:false));

                        spawnedObject.transform.localRotation *= Quaternion.Euler(0, -direction.y * multiplierY * (dotVal1 > 0 ? -1 : 1) * (dotVal1 > 0 ? -1 : 1) * (dotVal3 > 0 ? -1 : 1), 0);
                    }
                    break;

                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
            }
        }

        if (Input.touchCount == 2 && isPlaced && enableTouchControls)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            // if any one of touchzero or touchOne is cancelled or maybe ended then do nothing
            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return; // basically do nothing
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = spawnedObject.transform.localScale;
                Debug.Log("Initial Disatance: " + initialDistance + "GameObject Name: "
                    + arObjectToSpawn.name); // Just to check in console
            }
            else // if touch is moved
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                //if accidentally touched or pinch movement is very very small
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return; // do nothing if it can be ignored where inital distance is very close to zero
                }

                var factor = currentDistance / initialDistance;
                spawnedObject.transform.localScale = initialScale * factor; // scale multiplied by the factor we calculated
            }
        }



        UpdatePlacementPose();
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        testtext.text = "Komplettansicht";
        isPlaced = true;
        massstab = GameObject.Find("massstab").GetComponent<TextMeshProUGUI>();
        massstab.text = "Ma�stab: 1:" + Mathf.Round(1 / spawnedObject.transform.localScale.x * 1f) / 1f;
        DestroyPlaneTracking();

    }

    void DestroyPlaneTracking() {
        //Destroy(sessionOrigin.GetComponent("ARPlaneManager"));
        //Destroy(sessionOrigin.GetComponent("ARRaycastManager"));
        ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
        planeManager.enabled = !planeManager.enabled;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

    }
}

