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
    public MainMenuGUI menu;
	public RawImage profilePicture;
    public Button profileButton;

	void Start () {
		myNetwork = GameObject.FindGameObjectWithTag("Network").GetComponent<MyNetwork>();
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenuGUI>();
	}

	void Update () {
		
	}

	public void addButtonFunction(){
		playButton.onClick.AddListener (() => {	StartCoroutine (myNetwork.stream (id,playButton)); });
	}

    public void addProfileButtonFunction()
    {
        profileButton.onClick.AddListener(() => { menu.DisplayUserPreviewPanel(username.text, profilePicture); });
    }
}
