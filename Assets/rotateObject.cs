using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    GameObject objectRotate;

    public void RotateObject() {
        objectRotate = GameObject.Find("testscale(Clone)");
        objectRotate.transform.Rotate(Vector3.up, 45);
    }

}
