using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProductP : MonoBehaviour {
    public Text status;
 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
            status.text = "Products picked in: " + Test.actualZone.name;
        if (Input.GetKey(KeyCode.A))
            status.text = "Products delivered in: " + Test.actualZone.name;
    }
}
