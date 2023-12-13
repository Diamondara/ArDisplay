using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class maximizeObject : MonoBehaviour
{
    GameObject objectBigger;
    TextMeshProUGUI testtext;
    TextMeshProUGUI massstab;
    public float step = 0.001f;
     
    public void MaximizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        massstab = GameObject.Find("massstab").GetComponent<TextMeshProUGUI>();
        objectBigger = GameObject.Find("modelchanger(Clone)");
        if (objectBigger.transform.localScale.x + step < 0.02f)
        {
            testtext.text = "Objekt größer skaliert.";
           objectBigger.transform.localScale += new Vector3(step,step,step);
            massstab.text = "Maßstab: 1:" + Mathf.Round(1 / objectBigger.transform.localScale.x * 1f) / 1f;
        }
        else { 
            testtext.text = "Das Objekt kann nicht größer skaliert werden.";
        }
    }

    public void MinimizeObject()
    {
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        massstab = GameObject.Find("massstab").GetComponent<TextMeshProUGUI>();
        objectBigger = GameObject.Find("modelchanger(Clone)");
        if (objectBigger.transform.localScale.x - step > 0)
        {
            testtext.text = "Objekt kleiner skaliert.";
           objectBigger.transform.localScale -= new Vector3(step, step, step);
            massstab.text = "Maßstab: 1:" + Mathf.Round(1/objectBigger.transform.localScale.x*1f)/1f;
        }
        else {
            testtext.text = "Das Objekt kann nicht kleiner skaliert werden.";
        }
    }

}
