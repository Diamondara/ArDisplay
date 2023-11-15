using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class toggleActive : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    private GameObject spawnedObject;
    private Vector2 direction;
    private float multiplierY = 0.02f;
    private float multiplierX = 0.02f;
    TextMeshProUGUI testtext;
    private bool isPlaced = false;
    public void activateObject() {
        spawnedObject = Instantiate(arObjectToSpawn, new Vector3 (0.5f,0,0.5f), new Quaternion());
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        testtext.text = "Komplettansicht";
        isPlaced = true;
    }
}
