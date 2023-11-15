using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menubutton : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Button button;
    bool isUp = true;

    public void changeIcon() {
        if (isUp)
        {
            button.image.sprite = up;
        }
        else {
            button.image.sprite = down;
        }
        isUp = !isUp;
    }
}
