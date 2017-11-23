using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Test : getBeacons {

    //public List<Material> materials;
    public Material mOut, mIn;
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
        
        first = true;
        mOut = (Material)Resources.Load("Out", typeof(Material));
        mIn = (Material)Resources.Load("In", typeof(Material));
        /*materials.Add(mOut);
        materials.Add(mIn);*/
        rend.sharedMaterial = mOut;

        //actualZone.name = "-1";
    }

    private void OnTriggerExit(Collider other)
    {
        rend.sharedMaterial = mOut;
        prevZone = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        actualZone = this.gameObject;

        if (!first)
        {
            for (int j = 0; j < beacons.Count; j++)
            {
                if (beacons[j].beacon_id == actualZone.name)
                {
                    if (beacons[j].adjacency.Contains(prevZone.name))
                    {
                        rend.sharedMaterial = mIn;
                        text.text = "Current zone: " + actualZone.name;
                    }
                    else
                    {
                        rend.sharedMaterial = mOut;
                        text.text = "Current zone: not adj";
                    }
                    break;
                }
            }
        }
        else
        {
            rend.sharedMaterial = mIn;
            text.text = "Current zone: " + actualZone.name;
            first = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
