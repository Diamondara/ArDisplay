using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class GPSLocation : MonoBehaviour
{
    TextMeshProUGUI testtext;
    TextMeshProUGUI distance;
    TextMeshProUGUI pos;
    public double lat;
    public double lon;
    public float rotateValue;
    //Vector2 placement = new Vector2(51.671073f, 8.341861f);
    //Vector2 placement = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude); // test on camera position
    public GameObject spawnObject;

    void Start()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        pos = GameObject.Find("pos").GetComponent<TextMeshProUGUI>();
        distance = GameObject.Find("distance").GetComponent<TextMeshProUGUI>();
        StartCoroutine(GPSLoc());
    }
    IEnumerator GPSLoc() {

        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        pos = GameObject.Find("pos").GetComponent<TextMeshProUGUI>();
        distance = GameObject.Find("distance").GetComponent<TextMeshProUGUI>();

        if (!Input.location.isEnabledByUser)
        {
            testtext.text = "no gps. using mock data";
            float testlat = 51.67155658709858f;
            float testlon = 8.34235063836926f;
            float objlat = 51.671078150153f;
            float objlon = 8.341928611014499f;

            // Vector2 cameraPos = new Vector2(51.670993f, 8.330734f);
            // Vector2 cameraPos = new Vector2(51.67155658709858f, 8.34235063836926f); // straße davor
            Vector2 cameraPos = new Vector2(testlat,testlon); // local test
            GPSEncoder.SetLocalOrigin(cameraPos); // set 0,0
            Vector3 poscam = GPSEncoder.GPSToUCS(cameraPos);
           // Instantiate(spawnObject, poscam, rotationObject);

            convertToXYZ((float) lat,(float) lon); // spawn obj at coords
            double distanceBird = Math.Round(DistanceTo(testlat,testlon,objlat,objlon) * 100) / 100;
            distance.text ="Distance at start: "+ distanceBird.ToString() + " m";
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
            //  testtext.text = "running";
            convertToXYZ((float) lat, (float) lon);
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
            double distanceBird = Math.Round(DistanceTo(Input.location.lastData.latitude, Input.location.lastData.longitude, lat, lon) * 100) / 100;
            distance.text = "Distance: " + distanceBird.ToString() + " m to "+ lat + " " + lon;
            
        }
        else {
           // testtext.text = "Stop";
        }

    }
    public void convertToXYZ(float latCon, float lonCon)
    {
        Vector2 positionObject = new Vector2(latCon, lonCon);
        // Vector2 positionObject = new Vector2(51.67094912079465f, 8.341666826763078f); // location vor ort
        if (Input.location.status == LocationServiceStatus.Running)
        {
            GPSEncoder.SetLocalOrigin(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
        }
        Vector3 positionFinder = GPSEncoder.GPSToUCS(positionObject);
        // Vector3 positionFinder = GPSEncoder.GPSToUCS(placement);


        // place object on camera
        // rotate 340° to fit scene
        spawnObject.transform.position = positionFinder;
        // spawnObject.transform.Rotate(new Vector3(0,rotateValue,0));
        pos.text = "unity pos " + positionFinder.x +" "+ positionFinder.y +" "+ positionFinder.z;
        Instantiate(spawnObject);
    }

    // place house here
    // 51.671073, 8.341861
    // https://github.com/MichaelTaylor3D/UnityGPSConverter

    // calculate distance between gps coords
    public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
    {
        double rlat1 = Math.PI * lat1 / 180;
        double rlat2 = Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Math.PI * theta / 180;
        double dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;

        switch (unit)
        {
            case 'K': //Kilometers -> default - changed to M
                return dist * 1.609344 * 1000;
            case 'N': //Nautical Miles 
                return dist * 0.8684;
            case 'M': //Miles
                return dist;
        }

        return dist;
    }
}
