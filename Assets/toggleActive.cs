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
    TextMeshProUGUI massstab;
    private bool isPlaced = false;
    public void activateObject() {
        spawnedObject = Instantiate(arObjectToSpawn, new Vector3 (0.5f,0,0.5f), new Quaternion());
        testtext = GameObject.Find("testtext").GetComponent<TextMeshProUGUI>();
        testtext.text = "Komplettansicht";
        massstab = GameObject.Find("massstab").GetComponent<TextMeshProUGUI>();
        massstab.text = "Maﬂstab: 1:" + Mathf.Round(1 / spawnedObject.transform.localScale.x * 1f) / 1f;
        isPlaced = true;
    }
}
