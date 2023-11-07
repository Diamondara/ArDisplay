using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swapVisuals : MonoBehaviour
{
    public GameObject modelchanger;
    GameObject placedobject;
    private int state = 4; // active state 4 = base, 0-3 = etage 0-3
    Text testtext;

    swapVisuals() {
      //  zero = modelchanger.transform.GetChild(0).gameObject;
      //  one = modelchanger.transform.GetChild(1).gameObject;
      //  two = modelchanger.transform.GetChild(2).gameObject;
      //  three = modelchanger.transform.GetChild(3).gameObject;
      //  baselvl = modelchanger.transform.GetChild(4).gameObject;
    }
    public void toggleModels() {
        placedobject = GameObject.Find("modelchanger(Clone)");
        testtext = GameObject.Find("testtext").GetComponent<Text>();

       // modelchanger = GameObject.Find("modelchanger(Clone)");
        switch (state) {
            case 0:
                swapToBase();
                testtext.text = "0 " + placedobject.transform.GetChild(0).gameObject.name;
                //modelchanger.transform.GetChild(0).gameObject.active = true;
                break;
            case 1:
                swapToZero();
                testtext.text = "1" + placedobject.transform.GetChild(1).gameObject.name;
                break;
            case 2:
                swapToOne();
                testtext.text = "2" + placedobject.transform.GetChild(2).gameObject.name;
                break;
            case 3:
                swapToTwo();
                testtext.text = "3" + placedobject.transform.GetChild(3).gameObject.name;
                break;
            case 4:
                swapToThree();
                testtext.text = "4" + placedobject.transform.GetChild(4).gameObject.name;
                break;
            default:
                break;
        }
    
    }

    public void swapToBase()
    {
        placedobject.transform.GetChild(0).gameObject.SetActive(false);
        placedobject.transform.GetChild(1).gameObject.SetActive(false);
        placedobject.transform.GetChild(2).gameObject.SetActive(false);
        placedobject.transform.GetChild(3).gameObject.SetActive(false);
        placedobject.transform.GetChild(4).gameObject.SetActive(true);
        state = 4;
    }
    public void swapToZero()
    {
        placedobject.transform.GetChild(0).gameObject.SetActive(true);
        placedobject.transform.GetChild(1).gameObject.SetActive(false);
        placedobject.transform.GetChild(2).gameObject.SetActive(false);
        placedobject.transform.GetChild(3).gameObject.SetActive(false);
        placedobject.transform.GetChild(4).gameObject.SetActive(false);
        state = 0;
    }
    public void swapToOne()
    {
        placedobject.transform.GetChild(0).gameObject.SetActive(false);
        placedobject.transform.GetChild(1).gameObject.SetActive(true);
        placedobject.transform.GetChild(2).gameObject.SetActive(false);
        placedobject.transform.GetChild(3).gameObject.SetActive(false);
        placedobject.transform.GetChild(4).gameObject.SetActive(false);
        state = 1;
    }
    public void swapToTwo()
    {
        placedobject.transform.GetChild(0).gameObject.SetActive(false);
        placedobject.transform.GetChild(1).gameObject.SetActive(false);
        placedobject.transform.GetChild(2).gameObject.SetActive(true);
        placedobject.transform.GetChild(3).gameObject.SetActive(false);
        placedobject.transform.GetChild(4).gameObject.SetActive(false);
        state = 2;
    }
    public void swapToThree()
    {
        placedobject.transform.GetChild(0).gameObject.SetActive(false);
        placedobject.transform.GetChild(1).gameObject.SetActive(false);
        placedobject.transform.GetChild(2).gameObject.SetActive(false);
        placedobject.transform.GetChild(3).gameObject.SetActive(true);
        placedobject.transform.GetChild(4).gameObject.SetActive(false);
        state = 3;
    }
}
