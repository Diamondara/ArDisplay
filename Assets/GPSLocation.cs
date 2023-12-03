using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
public class GPSLocation : MonoBehaviour
{
    TextMeshProUGUI testtext;

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
        }
        else {
            testtext.text = "Stop";
        }

    }

    // place house here
    // 51.671073, 8.341861
    // maybe rotate ~180°
}
