using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Zone : MonoBehaviour {
    public Material[] material;
    Renderer rend;
    string actualZone;
    public Text zone;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    private void OnTriggerExit(Collider other)
    {
        rend.sharedMaterial = material[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        rend.sharedMaterial = material[1];
        actualZone = this.gameObject.name;
        zone.text = "Current zone: "+ actualZone;
    }

    // Update is called once per frame
    void Update () {
        
    }
}
