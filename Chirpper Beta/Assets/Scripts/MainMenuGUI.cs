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
    public GameObject homeButtonPanel;
    public GameObject userPreviewPanel;
    public GameObject loggedOutPanel;
    public GameObject blackoutPanel;
    public GameObject mustBeLoggedInPanel;
	public GameObject searchPanel;
	public GameObject searchField;
	public GameObject deleteChirpPanel;
	public GameObject loggedOutHomeButtonPanel;

    // For Sign Up Panel
    public InputField newEmailAddress;
    public InputField newPassword;
    public InputField newPasswordConfirm;
    public InputField newUsername;

    // For Login Panel
    public InputField oldUsername;
    public InputField oldPassword;

    // For Create Chirps Panel
    public InputField chirpTitle;

    // For Logged In Panel
    public Text currentUsername;
    public Text currentChirpsText;
    public Text currentFollowersText;
    public Text currentFollowingText;

    // For User Preview Panel
    public Text previewUsername;
    public RawImage previewImage;
    public Button previewFollowButton;
    public Text previewFollowText;
    string lastPanel = "";

    // For All People Panel
    public GameObject allPeoplePanel;

    // For Following Buttons
    public bool isFollowing = false;

    // For Following Panel
    public Text numberOfFollowing;
    public Text noFollowingText;

    // For Followers Panel
    public Text numberOfFollowers;
    public Text noFollowersText;
	public Text noChirpsText;

    public Text createNewUsername;

	public Text searchText;

	public GameObject meowingSFX;
	public Button playButton;
	public Sprite pauseSprite;

    static string result;

    static string[] data;

    GameObject networkObject;
    MyNetwork myNetwork;
	int deleteChirpID;

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
		homeButtonPanel.SetActive(false);
		searchPanel.SetActive(false);
        userPreviewPanel.SetActive(false);
        loggedOutPanel.SetActive(true);
        blackoutPanel.SetActive(false);
        mustBeLoggedInPanel.SetActive(false);
        allPeoplePanel.SetActive(false);

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

    // For Logging Out
    public void Logout()
    {
		myNetwork.setSearchFieldPosition (187);
        loggedOutPanel.SetActive(true);
        recentChirpsPanel.SetActive(true);

        postChirpsPanel.SetActive(false);
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loggedInPanel.SetActive(false);
		homeButtonPanel.SetActive(false);
		searchPanel.SetActive(false);
        myNetwork.isLoggedIn = false;
        allPeoplePanel.SetActive(false);
		loggedOutHomeButtonPanel.SetActive(true);

        newUsername.text = "";
        newPassword.text = "";
        newPasswordConfirm.text = "";

        oldUsername.text = "";
        oldPassword.text = "";

		myNetwork.clearAllPanels ();
    }

    // For Following Users
    public void ClickPreviewFollowUser()
    {
        if (myNetwork.isLoggedIn)
        {
            string temp1 = currentUsername.text.Remove(0, 1);
            string temp2 = previewUsername.text.Remove(0, 1);

            StartCoroutine(myNetwork.followUser(temp1, temp2));
        }
        else
        {
            DisplayMustBeLoggedInPanel();
        }
    }

    // For Must Be Logged In Panel
    public void DisplayMustBeLoggedInPanel()
    {
        Debug.Log("Displaying Must Be Logged In Panel...");
        mustBeLoggedInPanel.SetActive(true);
        userPreviewPanel.SetActive(false);

        if (loginPanel.activeSelf)
        {
            loginPanel.SetActive(false);
            lastPanel = "loginPanel";
        }
        else if (signUpPanel.activeSelf)
        {
            signUpPanel.SetActive(false);
            lastPanel = "signUpPanel";
        }
        
        loggedOutPanel.SetActive(false);
    }

    public void CloseMustBeLoggedInPanel()
    {
        Debug.Log("Closing Must Be Logged In Panel");
        mustBeLoggedInPanel.SetActive(false);

        if (lastPanel == "loginPanel")
            loginPanel.SetActive(true);
        else if (lastPanel == "signUpPanel")
            signUpPanel.SetActive(true);

        loggedOutPanel.SetActive(true);
        recentChirpsPanel.SetActive(true);
    }

    // For User Preview Panel
    public void DisplayUserPreviewPanel(string username, RawImage profileImage)
    {
        Debug.Log("Displaying User Preview Panel...");
        Debug.Log("Previewing username: " + username);

        string temp = currentUsername.text.Remove(0, 1);

        StartCoroutine(myNetwork.getFollowing(temp, username));

        Debug.Log("Is Following: " + isFollowing);

        if (isFollowing)
            previewFollowText.text = "Unfollow";
        else
            previewFollowText.text = "Follow";

        previewUsername.text = "@" + username;
        previewImage = profileImage;
        //blackoutPanel.SetActive(true);
        userPreviewPanel.SetActive(true);

		searchField.SetActive (false);
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false);
        loggedInPanel.SetActive(false);
        loggedOutPanel.SetActive(false);
        postChirpsPanel.SetActive(false);
        homeButtonPanel.SetActive(false);

        if (followingPanel.activeSelf) {
			followingPanel.SetActive (false);
			lastPanel = "followingPanel";
		} else if (followersPanel.activeSelf) {
			followersPanel.SetActive (false);
			lastPanel = "followersPanel";
		} else if (homeChirpsPanel.activeSelf) {
			homeChirpsPanel.SetActive (false);
			lastPanel = "homeChirpsPanel";
		} else if (recentChirpsPanel.activeSelf) {
			recentChirpsPanel.SetActive (false);
			lastPanel = "recentChirpsPanel";
		} else if (myChirpsPanel.activeSelf) {
			myChirpsPanel.SetActive (false);
			lastPanel = "myChirpsPanel";
		}else if  (searchPanel.activeSelf){
			searchPanel.SetActive (false);
			lastPanel = "searchPanel";
		}
    }

    public void CloseUserPreviewPanel()
    {
        Debug.Log("Closing User Preview Panel");
        //blackoutPanel.SetActive(false);
        userPreviewPanel.SetActive(false);

        loggedOutPanel.SetActive(true);

        if (myNetwork.isLoggedIn)
        {
            loggedInPanel.SetActive(true);
            postChirpsPanel.SetActive(true);
            homeButtonPanel.SetActive(true);
        }
        
		searchField.SetActive (true);
        if (lastPanel == "followingPanel")
            followingPanel.SetActive(true);
        else if (lastPanel == "followersPanel")
            followersPanel.SetActive(true);
        else if (lastPanel == "homeChirpsPanel")
            homeChirpsPanel.SetActive(true);
        else if (lastPanel == "recentChirpsPanel")
            recentChirpsPanel.SetActive(true);
		else if (lastPanel == "myChirpsPanel")
			myChirpsPanel.SetActive(true);
		else if (lastPanel == "searchPanel")
			searchPanel.SetActive(true);
    }

	public void DisplayDeleteChirpPanel(int id){
		deleteChirpPanel.SetActive (true);
		deleteChirpID = id;
	}

	public void NetworkDeleteChirp(){
		myNetwork.deleteChirpVoid (deleteChirpID);
		myNetwork.refreshAllPanels ();
		CloseDeleteChirpPanel ();
	}

	public void CloseDeleteChirpPanel(){
		deleteChirpPanel.SetActive (false);
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
        {
            Debug.Log("Setting Following to: " + following);
            currentFollowingText.text = following;
        }
  
        if (followers != "nc")
        {
            Debug.Log("Setting Followers to: " + followers);
            currentFollowersText.text = followers;
        }
            
        if (chirps != "nc")
        {
            Debug.Log("Setting Posts to: " + chirps);
            currentChirpsText.text = chirps;
        }     
    }

    public void LoginUser()
    {
        Debug.Log("Trying to login...");
        StartCoroutine (myNetwork.login(oldUsername.text.ToString(), oldPassword.text.ToString()));
        currentUsername.text = newUsername.text;
    }

    public void DisplayAllPeoplePanel()
    {
        myNetwork.setSearchFieldPosition(250);
        searchPanel.SetActive(false);
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(true);

        string temp = currentUsername.text.Remove(0, 1);
        StartCoroutine(myNetwork.getTopChirppers(temp));
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

	public void DisplaySearchPanel()
	{
		myNetwork.setSearchFieldPosition (250);
		StartCoroutine (myNetwork.search(searchText.text));
		searchPanel.SetActive (true);
		followingPanel.SetActive(false);
		followersPanel.SetActive(false);
		homeChirpsPanel.SetActive(false);
		recentChirpsPanel.SetActive(false);
		myChirpsPanel.SetActive(false);
		loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(false);
	}

	public void DisplayRecentChirpsPanel()
	{
		myNetwork.setSearchFieldPosition (187);
		myNetwork.refreshRecentChirps ();
		searchPanel.SetActive (false);
		followingPanel.SetActive(false);
		followersPanel.SetActive(false);
		homeChirpsPanel.SetActive(false);
		recentChirpsPanel.SetActive(true);
		myChirpsPanel.SetActive(false);
		loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(false);
	}

    public void DisplayHomeChirpsPanel()
    {
		myNetwork.setSearchFieldPosition (187);
		searchPanel.SetActive (false);
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(false);
		StartCoroutine(myNetwork.getFollowingChirps());
    }

    public void DisplayMyChirpsPanel()
    {
		myNetwork.setSearchFieldPosition (187);
		searchPanel.SetActive (false);
		StartCoroutine (myNetwork.getChirps(currentUsername.text.Substring(1)));
        followingPanel.SetActive(false);
        followersPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(true);
        allPeoplePanel.SetActive(false);
        loginFailedPanel.SetActive(false);
    }

    public void DisplayFollowingPanel()
    {
		myNetwork.setSearchFieldPosition (250);
		searchPanel.SetActive (false);
        followingPanel.SetActive(true);
        followersPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(false);

        string temp = currentUsername.text.Remove(0, 1);
        StartCoroutine(myNetwork.getFollowing(temp, null));
        
    }

    public void DisplayFollowersPanel()
    {
		myNetwork.setSearchFieldPosition (250);
		searchPanel.SetActive (false);
        followersPanel.SetActive(true);
        followingPanel.SetActive(false);
        homeChirpsPanel.SetActive(false);
        recentChirpsPanel.SetActive(false);
        myChirpsPanel.SetActive(false);
        loginFailedPanel.SetActive(false);
        allPeoplePanel.SetActive(false);

        string temp = currentUsername.text.Remove(0, 1);
        StartCoroutine(myNetwork.getFollowers(temp, null));
        
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
		searchPanel.SetActive (false);
		currentUsername.text = "@" + oldUsername.text;
        Debug.Log("Welcome Back, " + currentUsername.text + "!");
		loggedInPanel.SetActive(true);
		postChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
		myNetwork.setSearchFieldPosition (187);
		loginPanel.SetActive(false);
		allPeoplePanel.SetActive(false);
		homeButtonPanel.SetActive(true);
		loggedOutHomeButtonPanel.SetActive(false);
	}

    public void DisplayNewUserLoggedInPanel()
    {
		searchPanel.SetActive (false);
        currentUsername.text = "@" + newUsername.text;
        Debug.Log("Welcome to Chirpper, " + currentUsername.text);
        loggedInPanel.SetActive(true);
        postChirpsPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
        homeChirpsPanel.SetActive(true);
		myNetwork.setSearchFieldPosition (187);
        signUpPanel.SetActive(false);
        createUsernamePanel.SetActive(false);
        homeButtonPanel.SetActive(true);
		loggedOutHomeButtonPanel.SetActive(false);
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
