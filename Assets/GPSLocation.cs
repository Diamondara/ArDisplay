using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class GPSLocation : MonoBehaviour
{
    TextMeshProUGUI testtext;
    public double lat;
    public double lon;
    //Vector2 placement = new Vector2(51.671073f, 8.341861f);
    //Vector2 placement = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude); // test on camera position
    Quaternion rotationObject = new Quaternion(0, 0, 0, 0);
    public GameObject spawnObject;

    void Start()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
       
        StartCoroutine(GPSLoc());
    }
    IEnumerator GPSLoc() {

        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();

        if (!Input.location.isEnabledByUser)
        {
            testtext.text = "no gps";
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1) {
            testtext.text = "time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            testtext.text = "unable to determine location";
        }
        else {
            testtext.text = "running";
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }
      
    }

    private void UpdateGPSData()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            testtext.text = "lat: " + Input.location.lastData.latitude.ToString() + "\nlon: " + Input.location.lastData.longitude.ToString() +
                "\nalt: " + Input.location.lastData.altitude.ToString() + "\nacc: " + Input.location.lastData.horizontalAccuracy.ToString() +
                "\ntime: " + Input.location.lastData.timestamp.ToString();
            convertToXYZ();
        }
        else {
            testtext.text = "Stop";
        }

    }
    public void convertToXYZ()
    {
        Vector2 positionObject = new Vector2((float)lat, (float)lon);
        // GPSEncoder.SetLocalOrigin(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
        Vector3 positionFinder = GPSEncoder.GPSToUCS(positionObject);
        // Vector3 positionFinder = GPSEncoder.GPSToUCS(placement);


        // place object on camera
        Instantiate(spawnObject, positionFinder, rotationObject);
    }

    // place house here
    // 51.671073, 8.341861
    // maybe rotate ~180°
    // https://github.com/MichaelTaylor3D/UnityGPSConverter
}
