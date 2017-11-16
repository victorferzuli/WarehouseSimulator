using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class Beacon {
	public string section_id;
	public string beacon_minor;

	public Beacon(string section_id, string beacon_minor) {
		this.section_id = section_id;
		this.beacon_minor = beacon_minor;
	}

	/*public void setFloors(Dictionary<string, string> adjacency) {
		this.adjacency = adjacency;
	}*/

	public string toString() {
		return "Section ID: " + this.section_id + " Beacon Minor: " + this.beacon_minor;
	}
}

public class getBeacons : MonoBehaviour
{
    public List<Beacon> beacons = new List<Beacon>();
    public Dictionary<string, string> adjacency;
    private string url = "https://webservice-warehouse.run.aws-usw02-pr.ice.predix.io/index.php";

    void Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("s", "sectionBeaconFloor");
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));

        WWWForm form2 = new WWWForm();
        form2.AddField("s", "adjacencies");
        WWW www2 = new WWW(url, form2);
        StartCoroutine(WaitForRequest2(www2));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
            Processjson(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    IEnumerator WaitForRequest2(WWW www)
    {
		yield return www;

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			Processjson2(www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
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

            Beacon beacon = new Beacon(section_id, beacon_minor);
            beacons.Add(beacon);
        }
    }

    private void Processjson2(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);

        for (int i = 0; i < jsonvale.Count; i++)
        {
            string beacon_id = "null";
            if (!(jsonvale[i]["section_id"] == null))
                beacon_id = jsonvale[i]["beacon_id"].ToString();

            string adjacent_beacon_id = "null";
            if (!(jsonvale[i]["beacon_minor"] == null))
                adjacent_beacon_id = jsonvale[i]["adjacent_beacon_id"].ToString();

            adjacency.Add(beacon_id, adjacent_beacon_id);
        }
    }

    void Update()
    {
        string test;
        Debug.Log("WWW Beacon: " + beacons[0].toString());
        Debug.Log("WWW Beacon: " + adjacency.TryGetValue("1", out test) + test);
    }
}