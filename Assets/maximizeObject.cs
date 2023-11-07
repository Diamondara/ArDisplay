using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maximizeObject : MonoBehaviour
{
    GameObject objectBigger;
    Text testtext;
    public float step = 0.5f;
     
    public void MaximizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<Text>();
        objectBigger = GameObject.Find("modelchanger(Clone)");
        if (objectBigger.transform.localScale.x + step < 4f)
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
        objectBigger = GameObject.Find("modelchanger(Clone)");
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
