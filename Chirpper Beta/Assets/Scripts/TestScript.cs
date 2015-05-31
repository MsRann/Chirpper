using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	GameObject recentChirps;
	public GameObject chirpPrefab;
	GameObject temp;
	MyNetwork myNetwork;

    // Adding Following Panel
    GameObject followingPanel;
    public GameObject chirpperPrefab;
    GameObject temp2;

	void Start () {
		myNetwork = GameObject.Find("Main Camera").GetComponent<MyNetwork> ();
		recentChirps = GameObject.Find("Recent Chirps Panel");
		temp = Instantiate (chirpPrefab,recentChirps.transform.position,Quaternion.identity) as GameObject;
		temp.transform.SetParent (recentChirps.transform);

        // Adding Following Panel
        followingPanel = GameObject.Find("Following Panel");
        temp2 = Instantiate(chirpperPrefab, followingPanel.transform.position, Quaternion.identity) as GameObject;
        temp2.transform.SetParent(followingPanel.transform);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
