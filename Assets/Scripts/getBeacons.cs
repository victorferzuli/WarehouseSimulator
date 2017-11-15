using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class Beacon {
	public string section_id;
	public string beacon_minor;
	public List<string> adjacency;

	public Beacon(string section_id, string beacon_minor) {
		this.section_id = section_id;
		this.beacon_minor = beacon_minor;
	}

	public void setFloors(List<string> adjacency) {
		this.adjacency = adjacency;
	}

	public string toString() {
		return "Section ID: " + this.section_id + " Beacon Minor: " + this.beacon_minor;
	}
}

public class getBeacons : MonoBehaviour {
	public List<Beacon> beacons = new List<Beacon>();
	private string url = "https://webservice-warehouse.run.aws-usw02-pr.ice.predix.io/index.php";

	void Start () {
		WWWForm form = new WWWForm();
		form.AddField("s", "sectionMinorFloor");
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www){
		yield return www;

			// check for errors
			if (www.error == null)
			{
				Debug.Log("WWW Ok!: " + www.text);
				Processjson(www.text);
			} else {
				Debug.Log("WWW Error: "+ www.error);
			}    
	}

	IEnumerator WaitForRequest2(WWW www){
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

		for(int i = 0; i<jsonvale.Count; i++)
		{
			string section_id = "null";
			if (!(jsonvale[i]["section_id"] == null))
				section_id = jsonvale[i]["section_id"].ToString();

			string beacon_minor = "null";
			if (!(jsonvale[i]["beacon_minor"] == null))
				beacon_minor = jsonvale[i]["beacon_minor"].ToString();
			
			Beacon beacon = new Beacon (section_id, beacon_minor);
			beacons.Add (beacon);
		}

		/*
		for (int i = 0; i < this.beacons.Count; i++) {
			Debug.Log (beacons [i].toString ());
		}
		*/
		// Después de bajar la información de los beacons, agregamos la información de adjacency
		WWWForm form = new WWWForm();
		form.AddField("s", "sectionMinorFloor");
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
	}
}