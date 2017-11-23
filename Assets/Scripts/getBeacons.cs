using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class Beacon {
	public string beacon_id;
	public string beacon_minor;
    public float vertex_1;
    public float vertex_2;
    public List<string> adjacency;
    public GameObject sphere;

    public Beacon(string beacon_id, string beacon_minor, float vertex_1, float vertex_2, GameObject sphere) {
		this.beacon_id = beacon_id;
		this.beacon_minor = beacon_minor;
        this.vertex_1 = vertex_1;
        this.vertex_2 = vertex_2;
        this.adjacency = new List<string>();
        this.sphere = sphere;
    }

	public string toString() {
		return "Section ID: " + this.beacon_id + " Beacon Minor: " + this.beacon_minor;
	}
}

public class getBeacons : MonoBehaviour
{
    public static List<Beacon> beacons = new List<Beacon>();
    public static bool first;

    private string url = "https://webservice-warehouse.run.aws-usw02-pr.ice.predix.io/index.php";

    void Start()
    {
        WWWForm form = new WWWForm();
        //form.AddField("s", "sectionBeaconFloor");
        form.AddField("s", "beacons");
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        first = true;
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
            string beacon_id = "null";
            if (!(jsonvale[i]["id"] == null))
                beacon_id = jsonvale[i]["id"].ToString();
            string beacon_minor = "null";
            if (!(jsonvale[i]["minor"] == null))
                beacon_minor = jsonvale[i]["minor"].ToString();
            string vertex_1 = "null";
            if (!(jsonvale[i]["vertex_1"] == null))
                vertex_1 = jsonvale[i]["vertex_1"].ToString();
            string vertext_2 = "null";
            if (!(jsonvale[i]["vertex_2"] == null))
                vertext_2 = jsonvale[i]["vertex_2"].ToString();

            float v1 = float.Parse(vertex_1);
            float v2 = float.Parse(vertext_2);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(13F, 13F, 13F);
            float pos = (v1*10 + v2*10) / 2;
            sphere.transform.position = new Vector3(pos, 5, 30);
            sphere.name = beacon_id;
            sphere.GetComponent<Collider>().isTrigger = true;
            sphere.AddComponent(Type.GetType("Test"));
            Material mOut = (Material)Resources.Load("Out", typeof(Material));
            sphere.GetComponent<Renderer>().material = mOut;
            Beacon beacon = new Beacon(beacon_id, beacon_minor, v1, v2, sphere);
            beacons.Add(beacon);

            /*
            if (first == false)
            {
                for (int j = 0; j < beacons.Count; j++)
                {
                    if (beacons[j].beacon_id != beacon_id)
                    {
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.localScale = new Vector3(13F, 13F, 13F);
                        sphere.transform.position = new Vector3(13F, 13F, 13F);
                        sphere.name = beacon_id;
                        Material mOut = (Material)Resources.Load("Out", typeof(Material));
                        sphere.GetComponent<Renderer>().material = mOut;
                        Beacon beacon = new Beacon(beacon_id, beacon_minor, sphere);
                        beacons.Add(beacon);
                        break;
                    }
                }
            }
            else if(first == true)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.localScale = new Vector3(13F, 13F, 13F);
                sphere.transform.position = new Vector3(13F, 13F, 13F);
                sphere.name = beacon_id;
                Material mOut = (Material)Resources.Load("Out", typeof(Material));
                sphere.GetComponent<Renderer>().material = mOut;
                Beacon beacon = new Beacon(beacon_id, beacon_minor, sphere);
                beacons.Add(beacon);
                first = false;
            }*/
            Debug.Log("WWW beacons: " + " beacon_id: " + beacons[i].beacon_id);
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
                if (beacons[j].beacon_id == beacon_id)
                {
                    Debug.Log("WWW Beacon adj: " + j);
                    beacons[j].adjacency.Add(adjacent_beacon_id);
                    break;
                }
            }
        }
    }
}