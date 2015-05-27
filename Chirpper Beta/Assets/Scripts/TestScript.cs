using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	GameObject recentChirps;
	public GameObject chirpPrefab;
	GameObject temp;
	MyNetwork myNetwork;
	void Start () {
		myNetwork = GameObject.Find("Main Camera").GetComponent<MyNetwork> ();
		recentChirps = GameObject.Find("Recent Chirps Panel");
		temp = Instantiate (chirpPrefab,recentChirps.transform.position,Quaternion.identity) as GameObject;
		temp.transform.SetParent (recentChirps.transform);

	}
	
	// Update is called once per frame
	void Update () {
	}
}
