using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Test : getBeacons {

    public Material[] material;
    //public GameObject[] adj;
    Renderer rend;

    public Text text;
    public static GameObject actualZone;
    public static GameObject prevZone;
    bool first;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        first = true;

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

        if (!first)
        {
            for (int j = 0; j < beacons.Count; j++)
            {
                if (beacons[j].section_id == actualZone.name)
                {
                    if (beacons[j].adjacency.Contains(prevZone.name))
                    {
                        rend.sharedMaterial = material[1];
                        text.text = "Current zone: " + actualZone.name;
                    }
                    else
                    {
                        rend.sharedMaterial = material[0];
                        text.text = "Current zone: not adj";
                    }
                    break;
                }
            }
        }
        else
        {
            rend.sharedMaterial = material[1];
            text.text = "Current zone: " + actualZone.name;
            first = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
