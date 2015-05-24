using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class MainMenuGUI : MonoBehaviour {

	public GameObject loginPanel;
	public GameObject signUpPanel;
	public GameObject loggedInPanel;
	public GameObject postChirpsPanel;
    public GameObject followingPanel;
    public GameObject followersPanel;
    public GameObject homeChirpsPanel;
    public GameObject recentChirpsPanel;
    public GameObject myChirpsPanel;

    public InputField newUsername;

    public InputField oldUsername;
    public InputField oldPassword;

    public Text currentUsername;
    public Text currentChirpsText;
    public Text currentFollowersText;
    public Text currentFollowingText;

    static string currentChirps;
    static string currentFollowers;
    static string currentFollowing;

	public GameObject meowingSFX;
	public Button playButton;
	public Sprite pauseSprite;

    static string[] data;

	// Use this for initialization
	void Start () 
    {
        Screen.showCursor = true;

		loginPanel.SetActive(false);
		signUpPanel.SetActive(false);
		loggedInPanel.SetActive(false);
		postChirpsPanel.SetActive(false);
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
        myChirpsPanel.SetActive(false);
	}
	
	void FixedUpdate () 
    {
        currentFollowingText.text = currentFollowing;
        currentFollowersText.text = currentFollowers;
        currentChirpsText.text = currentChirps;
	}

    public static void SetUserInfo(string following, string followers, string chirps)
    {
        currentFollowing = following;
        currentFollowers = followers;
        currentChirps = chirps;
    }

    public void LoginUser()
    {
        MyNetwork.login(oldUsername.text, oldPassword.text);
        currentUsername.text = newUsername.text;
    }

    public void DisplayRecentChirpsPanel()
    {
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
        myChirpsPanel.SetActive(false);
    }

    public void DisplayHomeChirpsPanel()
    {
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
    }

    public void DisplayMyChirpsPanel()
    {
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(true);
    }

    public void DisplayFollowingPanel()
    {
        followingPanel.SetActive(true);
        followersPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
    }

    public void DisplayFollowersPanel()
    {
        followersPanel.SetActive(true);
        followingPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
    }

	public void DisplayLoginPanel()
	{
		loginPanel.SetActive(true);
	}

	public void DisplaySignUpPanel()
	{
		signUpPanel.SetActive(true);
	}

	public void DisplayLoggedInPanel()
	{
		currentUsername.text = "@" + oldUsername.text;
		loggedInPanel.SetActive(true);
		postChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
		loginPanel.SetActive(false);
	}

    public void DisplayNewUserLoggedInPanel()
    {
        currentUsername.text = "@" + newUsername.text;
        loggedInPanel.SetActive(true);
        postChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

	public void CancelLogin()
	{
		loginPanel.SetActive(false);
	}

	public void CancelSignUp()
	{
		signUpPanel.SetActive(false);
	}

	public void PlayMeowSFX()
	{
		if (!meowingSFX.GetComponent<AudioSource>().isPlaying)
		{
			meowingSFX.GetComponent<AudioSource>().Play();
			playButton.image.overrideSprite = pauseSprite;
		}
		else
		{
			meowingSFX.GetComponent<AudioSource>().Pause();
			playButton.image.overrideSprite = null;
		}
			
	}
}
