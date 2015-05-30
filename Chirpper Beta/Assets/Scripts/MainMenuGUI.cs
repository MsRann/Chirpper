﻿using UnityEngine;
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
    public GameObject loginFailedPanel;
    public GameObject createUsernamePanel;
    public GameObject newAccountFailedPanel;
    public GameObject newUsernameFailedPanel;
    public GameObject passwordMismatchPanel;
    public GameObject addAccountFailedPanel;

    public InputField newEmailAddress;
    public InputField newPassword;
    public InputField newPasswordConfirm;
    public InputField newUsername;

    public InputField oldUsername;
    public InputField oldPassword;

    public InputField chirpTitle;

    public Text currentUsername;
    public Text currentChirpsText;
    public Text currentFollowersText;
    public Text currentFollowingText;

    public Text createNewUsername;

	public GameObject meowingSFX;
	public Button playButton;
	public Sprite pauseSprite;

    static string result;

    static string[] data;

    GameObject networkObject;
    MyNetwork myNetwork;

	float timer;
	//refreshes automatically every 3 seconds
	readonly float REFRESH_RATE = 3.0f;

	// Use this for initialization
	void Start () 
    {
		StartCoroutine (getPermissions());
        Screen.showCursor = true;

        result = "";

		loginPanel.SetActive(false);
		signUpPanel.SetActive(false);
		loggedInPanel.SetActive(false);
		postChirpsPanel.SetActive(false);
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
        createUsernamePanel.SetActive(false);
        newUsernameFailedPanel.SetActive(false);
        newAccountFailedPanel.SetActive(false);
        passwordMismatchPanel.SetActive(false);
        addAccountFailedPanel.SetActive(false);

        networkObject = GameObject.FindGameObjectWithTag("Network");
        myNetwork = networkObject.GetComponent<MyNetwork>();
		StartCoroutine(myNetwork.getRecentChirps ());

	}

	
	IEnumerator getPermissions(){
		yield return Application.RequestUserAuthorization (UserAuthorization.Microphone);
		if (Application.HasUserAuthorization (UserAuthorization.Microphone)) {
			print("Got Access of Microphone");

		} else {
			print("Denied Access of Microphone");
		}
	}

	void Update () 
    {

//		timer += Time.deltaTime;
//		if (timer >= REFRESH_RATE){
//			timer = 0f;
//			if (homeChirpsPanel.active) {
//				StartCoroutine (myNetwork.getFollowingChirps());
//			}else if (recentChirpsPanel.active){
//				StartCoroutine (myNetwork.getRecentChirps());
//			}
//		}

        createNewUsername.text = "@" + newUsername.text;
	}

    public string getChirpTitle()
    {
        return chirpTitle.text.ToString();
    }

    public void clearChirpTitle()
    {
        chirpTitle.text = "";
    }

    public void SetUserInfo(string following, string followers, string chirps)
    {
        if (following != "nc")
            currentFollowingText.text = following;
        if (followers != "nc")
            currentFollowersText.text = followers;
        if (chirps != "nc")
            currentChirpsText.text = chirps;
    }

    public void LoginUser()
    {
        Debug.Log("Trying to login...");
        StartCoroutine (myNetwork.login(oldUsername.text.ToString(), oldPassword.text.ToString()));
        currentUsername.text = newUsername.text;
    }

    public void DisplayAddAccountFailedPanel()
    {
        addAccountFailedPanel.SetActive(false);
    }

    public void DisplayPasswordMismatchPanel()
    {
        signUpPanel.SetActive(false);
        passwordMismatchPanel.SetActive(true);
    }

    public void DisplayNewAccountFailedPanel()
    {
        signUpPanel.SetActive(false);
        newAccountFailedPanel.SetActive(true);
    }

    public void DisplayNewUsernameFailedPanel()
    {
        createUsernamePanel.SetActive(false);
        newUsernameFailedPanel.SetActive(true);
    }

    public void DisplayCreateUsernamePanel()
    {
        createUsernamePanel.SetActive(true);
        signUpPanel.SetActive(false);
        newUsernameFailedPanel.SetActive(false);
    }

    public void CheckPasswords()
    {
        Debug.Log("Confirming that passwords match...");
        if (newPassword.text != newPasswordConfirm.text)
        {
            DisplayPasswordMismatchPanel();
        }
        else
        {
            CreateAccount();
        }
    }

    public void CreateAccount()
    {
        Debug.Log("Trying to create new account...");
        StartCoroutine(myNetwork.createAccount(newEmailAddress.text.ToString(), newPassword.text.ToString()));
    }

    public void CreateUsername()
    {
        Debug.Log("Trying to create new username...");
        StartCoroutine(myNetwork.createUsername(newUsername.text.ToString()));
    }

    public void DisplayLoginFailedPanel()
    {
        loginPanel.SetActive(false);
        loginFailedPanel.SetActive(true);
    }

    public void DisplayRecentChirpsPanel()
    {
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
    }

    public void DisplayHomeChirpsPanel()
    {
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
		StartCoroutine(myNetwork.getFollowingChirps());
    }

    public void DisplayMyChirpsPanel()
    {
		StartCoroutine (myNetwork.getChirps(currentUsername.text.Substring(1)));
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(true);
        loginFailedPanel.SetActive(false);
    }

    public void DisplayFollowingPanel()
    {
        followingPanel.SetActive(true);
        followersPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
    }

    public void DisplayFollowersPanel()
    {
        followersPanel.SetActive(true);
        followingPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
    }

	public void DisplayLoginPanel()
	{
		loginPanel.SetActive(true);
        loginFailedPanel.SetActive(false);
        passwordMismatchPanel.SetActive(false);
	}

	public void DisplaySignUpPanel()
	{
		signUpPanel.SetActive(true);
        newAccountFailedPanel.SetActive(false);
        passwordMismatchPanel.SetActive(false);
        addAccountFailedPanel.SetActive(false);
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
        createUsernamePanel.SetActive(false);
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
