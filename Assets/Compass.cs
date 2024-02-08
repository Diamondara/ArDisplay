using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Compass : MonoBehaviour
{
    GPSLocation gps;
    public GameObject compass;
    GameObject needle;
    GameObject pointer;
    TextMeshProUGUI distance;
    [HideInInspector] public float angle;
    [HideInInspector] public double distanceBird;
    [HideInInspector] public Vector2 direction;
    bool test = false;


    public void Start()
    {
        needle = GameObject.Find("Needle");
        pointer = GameObject.Find("Pointer");
        gps = FindObjectOfType<GPSLocation>();

        angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg)-90 ; 
        Debug.Log("ANGLE?: " + angle);
    }
    public void Update() {
        // pointer.transform.Rotate(Vector3.forward, 45);
        //  needle.transform.Rotate(Vector3.forward, -45);
        float magneticHeading = Input.compass.magneticHeading;
        angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg)-90+magneticHeading; 
        Debug.Log("ANGLE: " + angle);
        // float magneticHeading = 180;
        // pointer.transform.rotation = Quaternion.Euler(0, 0, magneticHeading);
        //needle.transform.rotation = Quaternion.Euler(0, 0, 340);
        needle.transform.rotation = Quaternion.Euler(0, 0, magneticHeading);
        direction = determineDirection();
        distanceBird = gps.distanceBird;
        pointer.transform.rotation = Quaternion.Euler(0, 0, ((Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg)-90)+magneticHeading); // fix verschiebung
       // pointer.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(direction.x,direction.y)* Mathf.Rad2Deg);
        Debug.Log("degree"+(Mathf.Atan2(direction.x, direction.y)* Mathf.Rad2Deg-90));
    }

    public Vector2 determineDirection() {

        // GetComponent direction to house in degrees


        Vector2 buildingLocation = new Vector2((float)gps.lat, (float)gps.lon);
       // Vector2 currentLocation = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        Vector2 currentLocation = new Vector2((float)51.67088423227033, (float)8.330839936016385);
        //  Vector2 buildingLocation = new Vector2(0,1);
        // building - current = vektor im 0 punkt
        // (-) 
        direction = buildingLocation - currentLocation;
       // Vector2 direction2 = buildingLocation - currentLocation;
        Debug.Log("direction " +direction.ToString());
        Debug.Log("abstand in M " + GPSEncoder.GPSToUCS(direction).ToString());
        Vector2 north = new Vector2(currentLocation.x, currentLocation.y+1);
        Vector2 dirnorth = new Vector2(north.x-currentLocation.x,north.y-currentLocation.y);
        // winkel

        Debug.Log("dirnorth:"+dirnorth.ToString());
        Debug.Log("north:" + north.ToString());
        Debug.Log("current:" + currentLocation.ToString());
        Debug.Log("building:" + buildingLocation.ToString());
        Debug.Log("*winkel pls: "+Mathf.Acos(((direction.x*dirnorth.x)+(direction.y+ dirnorth.y))/(Mathf.Sqrt(direction.x*direction.x+direction.y*direction.y)* Mathf.Sqrt(dirnorth.x * dirnorth.x + dirnorth.y * dirnorth.y)))*Mathf.Rad2Deg);

        return direction;

    }
}
