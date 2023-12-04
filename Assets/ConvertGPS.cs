using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConvertGPS : MonoBehaviour
{
    public float lat;
    public float lon;
    //Vector2 placement = new Vector2(51.671073f, 8.341861f);
    Vector2 placement = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude); // test on camera position
    Quaternion rotationObject = new Quaternion(0, 0, 0,0);
    public GameObject spawnObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void convertToXYZ() {
        Vector2 positionObject = new Vector2(lat, lon);
       // GPSEncoder.SetLocalOrigin(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
        Vector3 positionFinder = GPSEncoder.GPSToUCS(positionObject);
        // Vector3 positionFinder = GPSEncoder.GPSToUCS(placement);
        

        // place object on camera
        Instantiate(spawnObject, positionFinder, rotationObject);
    }
}
