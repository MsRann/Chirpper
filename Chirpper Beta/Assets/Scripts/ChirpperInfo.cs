using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChirpperInfo : MonoBehaviour
{
    public Text username;
    public Text description;
    public Text time;
    public Button playButton;
    public Button followButton;
    public Button unfollowButton;
    public RawImage profilePicture;

    public int id;
    public MyNetwork myNetwork;
    public MainMenuGUI menu;
    
    void Start()
    {
        myNetwork = GameObject.FindGameObjectWithTag("Network").GetComponent<MyNetwork>();
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MainMenuGUI>();
    }

    void Update()
    {

    }

    public void addIntroButtonFunction()
    {
		playButton.onClick.AddListener(() => { StartCoroutine(myNetwork.stream(id, playButton));});
    }

    public void addFollowButtonFunction()
    {
        followButton.onClick.AddListener(() => { 
			StartCoroutine(myNetwork.followUser(menu.currentUsername.text.Substring(1), username.text));
			followButton.gameObject.SetActive(false);
			unfollowButton.gameObject.SetActive(true);
		});
    }

    public void addUnfollowButtonFunction()
    {
        unfollowButton.onClick.AddListener(() => { 
			StartCoroutine(myNetwork.unfollowUser(menu.currentUsername.text.Substring(1), username.text)); 
			followButton.gameObject.SetActive(true);
			unfollowButton.gameObject.SetActive(false);			
		});
    }
}
