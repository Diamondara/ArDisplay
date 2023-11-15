using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class maximizeObject : MonoBehaviour
{
    GameObject objectBigger;
    TextMeshProUGUI testtext;
    public float step = 0.5f;
     
    public void MaximizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        objectBigger = GameObject.Find("modelchanger(Clone)");
        if (objectBigger.transform.localScale.x + step < 4f)
        {
            testtext.text = "Objekt größer skaliert.";
           objectBigger.transform.localScale += new Vector3(step,step,step);
        }
        else { 
            testtext.text = "Das Objekt kann nicht größer skaliert werden.";
        }
    }

    public void MinimizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        objectBigger = GameObject.Find("modelchanger(Clone)");
        if (objectBigger.transform.localScale.x - step > 0)
        {
            testtext.text = "Objekt kleiner skaliert.";
           objectBigger.transform.localScale -= new Vector3(step, step, step);
        }
        else {
            testtext.text = "Das Objekt kann nicht kleiner skaliert werden.";
        }
    }

}
