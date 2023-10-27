using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpObject : MonoBehaviour
{
    public GameObject Object;

    private bool _ZoomIn;
    private bool _ZoomOut;

    public float Scale = 0.1f; // Scale speed

    private void Update()
    {
        if (_ZoomIn) {
            Object.transform.localScale += new Vector3(Scale, Scale, Scale);
        }
        if (_ZoomOut)
        {
            Object.transform.localScale -= new Vector3(Scale, Scale, Scale);
        }
    }

    public void OnPressZoomIn() {
        _ZoomIn = true;
    }
    public void OnReleaseZoomIn()
    {
        _ZoomIn = false;
    }
    public void OnPressZoomOut()
    {
        _ZoomOut = true;
    }
    public void OnReleaseZoomOut()
    {
        _ZoomOut = false;
    }
}
