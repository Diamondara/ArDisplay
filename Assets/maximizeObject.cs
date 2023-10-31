using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maximizeObject : MonoBehaviour
{
    GameObject objectBigger;
    Text testtext;
    public float step = 0.001f;

    public void MaximizeObject()
    {
        objectBigger = GameObject.Find("testscale(Clone)");
        if (objectBigger.transform.localScale.x + step < 0.01f)
        {
            testtext.text = "";
           objectBigger.transform.localScale += new Vector3(step,step,step);
        }
        else { 
            testtext.text = "Das Objekt kann nicht größer skaliert werden.";
        }
    }

    public void MinimizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<Text>();
        objectBigger = GameObject.Find("testscale(Clone)");
        if (objectBigger.transform.localScale.x - step > 0)
        {
            testtext.text = "";
           objectBigger.transform.localScale -= new Vector3(step, step, step);
        }
        else {
            testtext.text = "Das Objekt kann nicht kleiner skaliert werden.";
        }
    }

}
