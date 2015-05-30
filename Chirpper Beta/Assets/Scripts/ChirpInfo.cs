using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChirpInfo : MonoBehaviour {
	public Text username;
	public Text title;
	public Text timestamp;
	public Text timer;
	public Button playButton;
	public int id;
	public MyNetwork myNetwork;
	public RawImage profilePicture;

	void Start () {
		myNetwork = GameObject.FindGameObjectWithTag("Network").GetComponent<MyNetwork>();
	}

	void Update () {
		
	}

	public void addButtonFunction(){
		playButton.onClick.AddListener (() => {	StartCoroutine (myNetwork.stream (id,playButton)); });
	}
}
