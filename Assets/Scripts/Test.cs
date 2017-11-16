using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Test : MonoBehaviour {
    public Material[] material;
    public GameObject[] adj;
    Renderer rend;

    public Text text;
    public static GameObject actualZone;
    public static GameObject prevZone;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];

        //actualZone.name = "-1";
    }

    private void OnTriggerExit(Collider other)
    {
        rend.sharedMaterial = material[0];
        prevZone = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        actualZone = this.gameObject;
       
            rend.sharedMaterial = material[1];
            text.text = "Current zone: " + actualZone.name;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
