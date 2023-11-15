using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMenu : MonoBehaviour
{
    public GameObject gui;
    bool isActive = true;
   
        
        public void toggleMenu() {
       // gui = GameObject.Find("UIPanel");

        if (isActive)
        {
            gui.gameObject.SetActive(false);
            isActive = false;
        }
        else {
            gui.gameObject.SetActive(true);
            isActive = true;
        }


    }
}
