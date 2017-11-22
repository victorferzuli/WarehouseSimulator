using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class Beacon {
	public string section_id;
	public string beacon_minor;
    public List<string> adjacency;
    public GameObject sphere;

    public Beacon(string section_id, string beacon_minor, GameObject sphere) {
		this.section_id = section_id;
		this.beacon_minor = beacon_minor;
        this.adjacency = new List<string>();
        this.sphere = sphere;
    }

	public string toString() {
		return "Section ID: " + this.section_id + " Beacon Minor: " + this.beacon_minor;
	}
}

public class getBeacons : MonoBehaviour
{
    public static List<Beacon> beacons = new List<Beacon>();

    private string url = "https://webservice-warehouse.run.aws-usw02-pr.ice.predix.io/index.php";

    void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("s", "sectionBeaconFloor");
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            Processjson(www.text);
            WWWForm form2 = new WWWForm();
            form2.AddField("s", "adjacencies");
            WWW www2 = new WWW(url, form2);
            StartCoroutine(WaitForRequest2(www2));
        }
        else{
            Debug.Log("WWW Error: " + www.error);
        }
    }

    IEnumerator WaitForRequest2(WWW www)
    {
		yield return www;

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW2 Ok!: " + www.text);
			Processjson2(www.text);
		} else {
			Debug.Log("WWW2 Error: "+ www.error);
		}    
	}

    private void Processjson(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);

        for (int i = 0; i < jsonvale.Count; i++)
        {
            string section_id = "null";
            if (!(jsonvale[i]["section_id"] == null))
                section_id = jsonvale[i]["section_id"].ToString();

            string beacon_minor = "null";
            if (!(jsonvale[i]["beacon_minor"] == null))
                beacon_minor = jsonvale[i]["beacon_minor"].ToString();

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale= new Vector3(13F, 13F, 13F);
            sphere.transform.position = new Vector3(13F, 13F, 13F);
            sphere.name = section_id;
            Material mOut = (Material)Resources.Load("Out", typeof(Material));
            sphere.GetComponent<Renderer>().material = mOut;

            Beacon beacon = new Beacon(section_id, beacon_minor, sphere);
            beacons.Add(beacon);
            Debug.Log("WWW beacons:  minor:   " + beacons[i].beacon_minor +  " section_id:   " + beacons[i].section_id);
        }
    }

    private void Processjson2(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);

        for (int i = 0; i < jsonvale.Count; i++)
        {
            string beacon_id = "null";
            if (!(jsonvale[i]["beacon_id"] == null))
                beacon_id = jsonvale[i]["beacon_id"].ToString();

            string adjacent_beacon_id = "null";
            if (!(jsonvale[i]["adjacent_beacon_id"] == null))
                adjacent_beacon_id = jsonvale[i]["adjacent_beacon_id"].ToString();

            Debug.Log("WWW Beacon adj " + " beacon_id: " + beacon_id + " adj_beacon: " + adjacent_beacon_id);

            for (int j = 0; j < beacons.Count; j++)
            {
                if (beacons[j].section_id == beacon_id)
                {
                    Debug.Log("WWW Beacon adj: " + j);
                    beacons[j].adjacency.Add(adjacent_beacon_id);
                    break;
                }
            }
        }
    }
}