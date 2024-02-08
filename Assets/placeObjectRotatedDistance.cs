using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class placeObjectRotatedDistance : MonoBehaviour
{
    public float distance;
    public float rotation;
    GameObject building;
    public Camera mainCamera;
    public GameObject rotator;
    Compass compass;
    // Start is called before the first frame update
    void Start()
    {
        compass = FindObjectOfType<Compass>();
        building = GameObject.Find("building");

     
            Invoke("place", 3); 
        
    }
    public void place() {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            distance = (float)compass.distanceBird;
            rotation = 0 - compass.angle;
            Debug.Log("*** d " + distance + " r " + rotation);

            Debug.Log(building.name.ToString());
            // building.gameObject.SetActive(true);
            Debug.Log(" x " + mainCamera.transform.position.x + " y " + mainCamera.transform.position.y + " z " + mainCamera.transform.position.z);
            //  building.transform.position = new Vector3(0, distance, rotation);
            Vector3 pos = mainCamera.transform.position;
            rotator.transform.position = mainCamera.transform.position;
            building.transform.position = new Vector3(0, -3, distance);
            rotator.transform.rotation = Quaternion.Euler(0, rotation, 0);
            GameObject model = GameObject.Find("buildingmodel");
            model.transform.rotation = Quaternion.Euler(0, -165,0);
        }
    }

}
