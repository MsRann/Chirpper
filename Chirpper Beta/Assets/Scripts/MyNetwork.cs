using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections;
using System.Linq;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

public class MyNetwork : MonoBehaviour {
	
	static string url = "https://chirpper.herokuapp.com/";
	string username;
    string password;
    string email;
    static string cookie = "";
    string follow="Enter a User To Follow!";
	bool signedUp = false;
	static string[] delim = {"\\"};
	public Button playingButton;
	public Sprite stopImage;
	public Sprite loadingImage;
	public AudioSource source;
	bool isLoading = false;
    GameObject menuGUI;
    MainMenuGUI menu;

    public bool isLoggedIn = false;
	public IEnumerator streamCoroutine;

	public GameObject chirpPrefab;
    public GameObject chirpperPrefab;

	public RawImage myProfilePicture;
	public Button uploadPicture;
	public GameObject searchField;

	public Text searchText;

	void Start () 
    {
		source = gameObject.AddComponent<AudioSource> ();

        menuGUI = GameObject.FindGameObjectWithTag("Menu");
        menu = menuGUI.GetComponent<MainMenuGUI>();

	//	uploadPicture.onClick.AddListener (() => {	StartCoroutine (sendProfilePicture());});
	}
	
	public IEnumerator followUser(string username, string followUsername) {
		WWWForm form = new WWWForm();
		form.AddField("username", username);
        form.AddField("follow", followUsername);
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {

            string[] words = download.text.Split(delim, System.StringSplitOptions.None);

            Debug.Log("[" + username + "] has started following: [" + followUsername + "]");
            menu.SetUserInfo(words[0], "nc", "nc");
            menu.previewFollowText.text = "Unfollow";
		}
	}

	public void refreshSearchPanel(){
		StartCoroutine (search (searchText.text));
	}

	public void setSearchFieldPosition(int x){
		Vector3 tempPosition = searchField.GetComponent<RectTransform>().anchoredPosition3D;
		tempPosition.x = x;
		searchField.GetComponent<RectTransform>().anchoredPosition3D = tempPosition;
	}

	public IEnumerator search(string search) {


		WWWForm form = new WWWForm();
		if (isLoggedIn) {
			form.AddField("username",menu.currentUsername.text.Substring(1));
		}
		form.AddField( "search", search );
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if (!string.IsNullOrEmpty (download.error)) {
			print ("Error downloading: " + download.error);
		} else {
		
			print("Raw Search Output:" + download.text);
			GameObject searchPanel = GameObject.Find ("Search Panel");
			//get 6 recent chirppers and add them to the panel
			if (searchPanel != null) {
				//Removes previous old searched users
				if (searchPanel.transform.Find ("searchUser1") != null)
					Destroy (searchPanel.transform.Find ("searchUser1").gameObject);
				if (searchPanel.transform.Find ("searchUser2") != null)
					Destroy (searchPanel.transform.Find ("searchUser2").gameObject);

				//found chirps
				if (searchPanel.transform.Find ("searchChirp1") != null)
					Destroy (searchPanel.transform.Find ("searchChirp1").gameObject);
				if (searchPanel.transform.Find ("searchChirp2") != null)
					Destroy (searchPanel.transform.Find ("searchChirp2").gameObject);
				if (searchPanel.transform.Find ("searchChirp3") != null)
					Destroy (searchPanel.transform.Find ("searchChirp3").gameObject);
				if (searchPanel.transform.Find ("searchChirp4") != null)
					Destroy (searchPanel.transform.Find ("searchChirp4").gameObject);
				if (searchPanel.transform.Find ("searchChirp5") != null)
					Destroy (searchPanel.transform.Find ("searchChirp5").gameObject);
				if (searchPanel.transform.Find ("searchChirp6") != null)
					Destroy (searchPanel.transform.Find ("searchChirp6").gameObject);

			}
			int iter = 0;
			int userNum = 0;
			int chirpNum = 0;
			string[] words = download.text.Split(delim, System.StringSplitOptions.None);
			searchPanel.transform.Find ("Chirps").gameObject.SetActive(false);
			searchPanel.transform.Find ("Users").gameObject.SetActive(false);
			if (words.Length == 1){
				
				searchText.text = "No Results Found";
			}else{
				searchText.text = "";
			}
			while(iter < words.Length){
				if (words[iter] == "user"){
					searchPanel.transform.Find ("Users").gameObject.SetActive(true);
					Vector3 newPos = searchPanel.transform.position;
					
					// Chirpper #1 (-70, 2)     Chirpper #2 (203, 2)
					switch (userNum)
					{
						// Chirpper #1 Position
					case 0:
					{
						newPos.x = -70;
						newPos.y = 2;
						break;
					}
						// Chirpper #2 Position
					case 1:
					{
						newPos.x = 203;
						newPos.y = 2;
						break;
					}
				}
				GameObject temp = Instantiate(chirpperPrefab, newPos, Quaternion.identity) as GameObject;
				temp.transform.position = newPos;
				temp.transform.SetParent(searchPanel.transform, false);
				temp.name = "searchUser" + (userNum+1);
				ChirpperInfo ci = temp.GetComponent<ChirpperInfo>();
				
				ci.username.text = words[iter+1];
				ci.description.text = "This is a small sample of some description text...";
				ci.time.text = "00:14";

				//set follow / unfollow NEEDS IMPLEMENTATION
					if (isLoggedIn){
						ci.addUnfollowButtonFunction();
						ci.addFollowButtonFunction();

						if (words[iter+2] == "false"){
							temp.transform.Find("Follow Button").gameObject.SetActive(true);
						}else{
							temp.transform.Find("Unfollow Button").gameObject.SetActive(true);
						}

					}else{
						temp.transform.Find("Unfollow Button").gameObject.SetActive(true);
						temp.transform.Find("Unfollow Button").transform.Find("Text").GetComponent<Text>().text = "Login First!";
					}

					iter+=3;
					userNum++;
				}else if (words[iter] == "chirp"){
					Vector3 newPos = searchPanel.transform.position;
					if (userNum != 0){
						searchPanel.transform.Find ("Chirps").gameObject.SetActive(true);
					}
					switch (chirpNum)
					{
						// Chirpper #1 Position
					case 0:
					{
						newPos.x = -8;
						newPos.y = 0;
						break;
					}
						// Chirpper #2 Position
					case 1:
					{
						newPos.x = -8;
						newPos.y = -72;
						break;
					}
						// Chirpper #3 Position
					case 2:
					{
						newPos.x = -8;
						newPos.y = -143;
						break;
					}
					}

					GameObject temp = Instantiate(chirpPrefab,newPos,Quaternion.identity) as GameObject;
					if (userNum != 0){
						newPos.y -=72;
					}
					temp.transform.position = newPos;
					temp.transform.SetParent (searchPanel.transform,false);
					temp.name = "searchChirp" + (chirpNum+1);
					ChirpInfo ci = temp.GetComponent<ChirpInfo>();
					
					DateTime dt = Convert.ToDateTime(words[iter+5]); // THIS LINE IS CAUSING AN ERROR
					if ((DateTime.Now - dt).TotalDays >= 1){
						ci.timestamp.text = dt.ToString ("MMM") + " " + dt.Day ;
					}else if ((int)(Math.Round ((DateTime.Now - dt).TotalHours)) > 0){
						ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
					}else{
						ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
					}
					
					ci.id = int.Parse(words[iter+1]);
					ci.username.text = words[iter+2];
					ci.title.text = words[iter+3];
					ci.timer.text = "00:";
					ci.timer.text += int.Parse (words[iter+4]) < 10? "0" + words[iter+4]:words[iter+4];
					ci.addButtonFunction();
                    ci.addProfileButtonFunction();
					if (isLoggedIn){
						
						if (menu.currentUsername.text.Substring(1) == ci.username.text){
							ci.transform.Find("Delete Button").gameObject.SetActive(true);
							ci.addDeleteButtonFunction();
						}
					}

					iter+=6;
					chirpNum++;
					if ((chirpNum == 2 && userNum != 0) || (userNum == 0 && chirpNum == 3)){
						break;
					}
				}else{
					iter++;
				}
			}
		}
	}

    public IEnumerator unfollowUser(string username, string unfollowUsername)
    {
		WWWForm form = new WWWForm();
		form.AddField("username", username );
        form.AddField("unfollow", unfollowUsername);
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
            string[] words = download.text.Split(delim, System.StringSplitOptions.None);

            Debug.Log("[" + username + "] has just unfollowed: [" + unfollowUsername + "]");
            menu.SetUserInfo(words[0], "nc", "nc");
            menu.previewFollowText.text = "Follow";
		}
	}


    public IEnumerator createAccount(string newEmail, string newPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("createAccount", "true");
        form.AddField("email", newEmail);
        form.AddField("password", newPassword);
      
        WWW download = new WWW(url, form);

        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            //gets results and stores them into string array split with delimiter \\
            string[] words = download.text.Split(delim, System.StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                //print(i + ": " + words[i]);
            }

            Debug.Log("Checking result...");

            if (words[0] == "false")
            {
                Debug.Log("Failed to create a new account!");

                // Display an error message
                menu.DisplayNewAccountFailedPanel();
            }
            else
            {
                Debug.Log("Successfully created a new account!");

                email = newEmail;
                password = newPassword;

                // Ask the user to create a new username
                menu.DisplayCreateUsernamePanel();
            }


            if (download.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                cookie = download.responseHeaders["SET-COOKIE"];
            }
        }
    }

    public IEnumerator createUsername(string newUsername)
    {
        WWWForm form = new WWWForm();
        form.AddField("createUsername", "true");
        form.AddField("username", newUsername);

        WWW download = new WWW(url, form);

        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            //gets results and stores them into string array split with delimiter \\
            string[] words = download.text.Split(delim, System.StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                //print(i + ": " + words[i]);
            }

            Debug.Log("Checking result...");

            if (words[0] == "false")
            {
                Debug.Log("Failed to create a new username!");

                // Display an error message
                menu.DisplayNewUsernameFailedPanel();
            }
            else
            {
                Debug.Log("Successfully created a new username!");

                username = newUsername;

                // Ask the user to create a new username
                StartCoroutine(addAccount());
            }


            if (download.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                cookie = download.responseHeaders["SET-COOKIE"];
            }
        }
    }

    public IEnumerator addAccount()
    {
        Debug.Log("Adding account...");
        Debug.Log("Email: " + email);
        Debug.Log("Username: " + username);
        Debug.Log("Password: " + password);

        WWWForm form = new WWWForm();
        form.AddField("addAccount", "true");
        form.AddField("email", email);
        form.AddField("username", username);
        form.AddField("password", password);

        WWW download = new WWW(url, form);

        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            //gets results and stores them into string array split with delimiter \\
            string[] words = download.text.Split(delim, System.StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                //print(i + ": " + words[i]);
            }

            Debug.Log("Adding new account to the Chirpper database...");

            if (words[0] == "false")
            {
                Debug.Log("Failed to add a new account!");

                // Display an error message
                menu.DisplayAddAccountFailedPanel();
            }
            else
            {
                Debug.Log("Successfully added a new account!");

                // Ask the user to create a new username
                menu.SetUserInfo("0", "0", "0");
                menu.DisplayNewUserLoggedInPanel();
                isLoggedIn = true;
            }


            if (download.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                cookie = download.responseHeaders["SET-COOKIE"];
            }
        }
    }

	public void refreshMyChirps(){
	
		StartCoroutine (getChirps(menu.currentUsername.text.Substring(1)));
	}

	public IEnumerator getChirps(string username) {
		print("Updating My Chirps Feed");
		WWWForm form = new WWWForm();
		form.AddField( "getUsersChirps", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			GameObject recentChirps = GameObject.Find("My Chirps Feed Panel");
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			//get 3 recent chirps and add them to the panel
			if (recentChirps != null){
				
				//Removes previous old chirps
				if(recentChirps.transform.Find("myChirp0")!= null){
					Destroy(recentChirps.transform.Find("myChirp0").gameObject);
				}
				if(recentChirps.transform.Find("myChirp4")!= null){
					Destroy(recentChirps.transform.Find("myChirp4").gameObject);
				}			
				if(recentChirps.transform.Find("myChirp8")!= null){
					Destroy(recentChirps.transform.Find("myChirp8").gameObject);
				}
				//loop a max of 3 times
				int loop = words.Length >= 12? 12: words.Length;
				if (words.Length != 1){
					menu.noChirpsText.enabled = false;
					for (int i = 0; i < loop-1; i+=4){
						Vector3 newPos = recentChirps.transform.position;
						newPos.y = 2 - ((i/4)*75);
						GameObject temp = Instantiate(chirpPrefab,newPos,Quaternion.identity) as GameObject;
						temp.transform.position = newPos;
						temp.transform.SetParent (recentChirps.transform,false);
						temp.name = "myChirp" + i;
						ChirpInfo ci = temp.GetComponent<ChirpInfo>();
						
						DateTime dt = Convert.ToDateTime(words[i+3]); 
						if ((DateTime.Now - dt).TotalDays >= 1){
							ci.timestamp.text = dt.ToString ("MMM") + " " + dt.Day ;
						}else if ((int)(Math.Round ((DateTime.Now - dt).TotalHours)) > 0){
							ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
						}else{
							ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
						}
						
						ci.id = int.Parse(words[i]);
						ci.username.text = username;
						ci.title.text = words[i+1];
						if (ci.title.text.Length > 20){
							ci.title.fontSize = 10;
						}
						ci.timer.text = "00:";
						ci.timer.text += int.Parse (words[i+2]) < 10? "0" + words[i+2]:words[i+2];
						ci.addButtonFunction();
                        ci.addProfileButtonFunction();
						if (isLoggedIn){
							if (menu.currentUsername.text.Substring(1) == ci.username.text){
								ci.transform.Find("Delete Button").gameObject.SetActive(true);
								ci.addDeleteButtonFunction();
							}
						}

						//ci.profilePicture.texture = myProfilePicture.texture;
					}
				}else{
					menu.noChirpsText.enabled = true;
				}
			}
				
				
				
//				form = new WWWForm();
//				
//				form.AddField( "getUsersProfilePicture", ci.username.text);
//				download =  new WWW( url, form);
//				yield return download;
//				
//				//couldn't get extension for file
//				if(!string.IsNullOrEmpty(download.error)) {
//					print( "Error downloading: " + download.error );
//					
//				} else {
//					//download.text should be extension of file
//					WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + username + download.text);
//					yield return www;
//					
//					if (!string.IsNullOrEmpty (www.error)) {
//						print("No Picture found, using default");
//					} else {
//						ci.profilePicture.texture = www.texture;
//						print ("Loading Picture");
//						//result(www.texture);
//					}
//				}
		}
	}

    public IEnumerator getFollowers(string thisUsername, string checkUsername)
    {

        if (isLoggedIn)
        {
            WWWForm form = new WWWForm();
            form.AddField("getFollowers", thisUsername);
            WWW download = new WWW(url, form);
            yield return download;

            if (!string.IsNullOrEmpty(download.error))
            {
                print("Error downloading: " + download.error);
            }
            else
            {
                //gets results and stores them into string array split with delimiter \\
                string[] words = download.text.Split(delim, System.StringSplitOptions.None);
                for (int i = 0; i < words.Length; i++)
                {
                    //print(i + ": " + words[i]);
                }

				Debug.Log("Number of people following [" + thisUsername + "]: " + words[0]);
                menu.SetUserInfo("nc", words[0], "nc");
				menu.numberOfFollowers.text = "Followers:  " +  words[0] + " Chirppers";
				GameObject followers = GameObject.Find("Followers Panel");
				//get 6 recent chirppers and add them to the panel
				if (followers != null){
					//Removes previous old chirppers
					if (followers.transform.Find("followers1") != null)
						Destroy(followers.transform.Find("followers1").gameObject);
					if (followers.transform.Find("followers2") != null)
						Destroy(followers.transform.Find("followers2").gameObject);
					if (followers.transform.Find("followers3") != null)
						Destroy(followers.transform.Find("followers3").gameObject);
					if (followers.transform.Find("followers4") != null)
						Destroy(followers.transform.Find("followers4").gameObject);
					if (followers.transform.Find("followers5") != null)
						Destroy(followers.transform.Find("followers5").gameObject);
					if (followers.transform.Find("followers6") != null)
						Destroy(followers.transform.Find("followers6").gameObject);
				}


                if (Convert.ToInt32(words[0]) < 1)
                {
                    Debug.Log("No one is following [" + thisUsername + "]");
					menu.noFollowersText.enabled = true;
                }
                else
                {
                    menu.noFollowersText.enabled = false;

                    for (int i = 1; i < words.Length - 1; i++)
                    {
                        Debug.Log("Follower #" + i + ": " + words[i]);
                        Debug.Log("Does [" + checkUsername + "] == " + "[" + words[i] + "]?");

                        if (!menu.isFollowing)
                            if (checkUsername == words[i])
                            {
                                Debug.Log("Setting isFollowing to true!");
                                // Set some match variable equal to true
                                menu.isFollowing = true;
                            }
                    }

                    //if (checkUsername == null)
                    // Set some match variable equal to false
                    //menu.isFollowing = false;

                    if (menu.followersPanel.activeSelf)
                    {
                        Debug.Log("Displaying a list of the people that [" + thisUsername + "] is following...");


//                        //loop a max of 6 times
//                        int loop = 0;
//
//                        if (words.Length > 5)
//                            loop = 6;
//                        else
//                            loop = words.Length - 1;

                        int index = 1;

						for (int i = 1; i < Convert.ToInt32(words[0])*2; i+=2)
                        {
                            Debug.Log("Following #" + i + ": " + words[i]);
                            Vector3 newPos = followers.transform.position;

                            // Chirpper #1 (-70, 2)     Chirpper #2 (203, 2)
                            // Chirpper #3 (-70, -66)   Chirpper #4 (203, -66)
                            // Chirpper #5 (-70, -135)  Chirpper #6 (203, -135)
                            switch (index)
                            {
                                // Chirpper #1 Position
                                case 1:
                                    {
                                        newPos.x = -70;
                                        newPos.y = 2;
                                        break;
                                    }
                                // Chirpper #2 Position
                                case 2:
                                    {
                                        newPos.x = 203;
                                        newPos.y = 2;
                                        break;
                                    }
                                // Chirpper #3 Position
                                case 3:
                                    {
                                        newPos.x = -70;
                                        newPos.y = -66;
                                        break;
                                    }
                                // Chirpper #4 Position
                                case 4:
                                    {
                                        newPos.x = 203;
                                        newPos.y = -66;
                                        break;
                                    }
                                // Chirpper #5 Position
                                case 5:
                                    {
                                        newPos.x = -70;
                                        newPos.y = -135;
                                        break;
                                    }
                                // Chirpper #6 Position
                                case 6:
                                    {
                                        newPos.x = 203;
                                        newPos.y = -135;
                                        break;
                                    }
                                // Default to the first position
                                default:
                                    {
                                        newPos.x = -70;
                                        newPos.y = 2;
                                        break;
                                    }
                            }

                            index++;

                            //newPos.y = 2 - ((i / 4) * 75);
                            //Debug.Log("X-POS: " + newPos.x + " Y-POS: " + newPos.y);
                            GameObject temp = Instantiate(chirpperPrefab, newPos, Quaternion.identity) as GameObject;
                            temp.transform.position = newPos;
                            temp.transform.SetParent(followers.transform, false);
                            temp.name = "followers" + i;
                            ChirpperInfo ci = temp.GetComponent<ChirpperInfo>();

                            ci.username.text = words[i];
                            ci.description.text = "This is a small sample of some description text...";
                            ci.time.text = "00:14";
							ci.addUnfollowButtonFunction();
							ci.addFollowButtonFunction();

							if(words[i+1] == "true"){
								temp.transform.Find("Unfollow Button").gameObject.SetActive(true);
							}else{
								temp.transform.Find("Follow Button").gameObject.SetActive(true);
							}
                        }
                    }
                }

            }
        }
        else
        {
            Debug.Log("Must be logged in to see the people who are following you.");
        }
	}

	public IEnumerator getFollowing(string thisUsername, string checkUsername) {

        if (isLoggedIn)
        {
            WWWForm form = new WWWForm();
            form.AddField("getFollowing", thisUsername);
            WWW download = new WWW(url, form);
            yield return download;

            if (!string.IsNullOrEmpty(download.error))
            {
                print("Error downloading: " + download.error);
            }
            else
            {

                //gets results and stores them into string array split with delimiter \\
                string[] words = download.text.Split(delim, System.StringSplitOptions.None);
                for (int i = 0; i < words.Length; i++)
                {
                    //print(i + ": " + words[i]);
                }

                Debug.Log("Number of people [" + thisUsername + "] is Following: " + words[0]);
				menu.SetUserInfo(words[0], "nc", "nc");

				menu.numberOfFollowing.text = "Following:  " + words[0] + " Chirppers";

				GameObject following = GameObject.Find("Following Panel");
				//get 6 recent chirppers and add them to the panel
				if (following != null){
					//Removes previous old chirppers
					if (following.transform.Find("following1") != null)
						Destroy(following.transform.Find("following1").gameObject);
					if (following.transform.Find("following2") != null)
						Destroy(following.transform.Find("following2").gameObject);
					if (following.transform.Find("following3") != null)
						Destroy(following.transform.Find("following3").gameObject);
					if (following.transform.Find("following4") != null)
						Destroy(following.transform.Find("following4").gameObject);
					if (following.transform.Find("following5") != null)
						Destroy(following.transform.Find("following5").gameObject);
					if (following.transform.Find("following6") != null)
						Destroy(following.transform.Find("following6").gameObject);
				}


                if (Convert.ToInt32(words[0]) < 1)
                {
                    Debug.Log("[" + thisUsername + "] is not following anyone.");
                    menu.noFollowingText.enabled = true;
                }
                else
                {
                    menu.noFollowingText.enabled = false;

                    for (int i = 1; i < words.Length - 1; i++)
                    {
                        Debug.Log("Following #" + i + ": " + words[i]);
                        Debug.Log("Does [" + checkUsername + "] == " + "[" + words[i] + "]?");

                        if (!menu.isFollowing)
                            if (checkUsername == words[i])
                            {
                                Debug.Log("Setting isFollowing to true!");
                                // Set some match variable equal to true
                                menu.isFollowing = true;
                            }
                    }

                    //if (checkUsername == null)
                    // Set some match variable equal to false
                    //menu.isFollowing = false;

                    if (menu.followingPanel.activeSelf)
                    {
                        Debug.Log("Displaying a list of the people that [" + thisUsername + "] is following...");
                        //loop a max of 6 times
                        int loop = 0;

                        if (words.Length > 7)
                            loop = 7;
                        else
                            loop = words.Length - 1;
                        
						for (int i = 1; i < loop; i++)
                        {
                            Debug.Log("Following #" + i + ": " + words[i]);
                            Vector3 newPos = following.transform.position;

                            // Chirpper #1 (-70, 2)     Chirpper #2 (203, 2)
                            // Chirpper #3 (-70, -66)   Chirpper #4 (203, -66)
                            // Chirpper #5 (-70, -135)  Chirpper #6 (203, -135)
                            switch(i)
                            {
                                // Chirpper #1 Position
                                case 1:
                                    {
                                        newPos.x = -70;
                                        newPos.y = 2;
                                        break;
                                    }
                                // Chirpper #2 Position
                                case 2:
                                    {
                                        newPos.x = 203;
                                        newPos.y = 2;
                                        break;
                                    }
                                // Chirpper #3 Position
                                case 3:
                                    {
                                        newPos.x = -70;
                                        newPos.y = -66;
                                        break;
                                    }
                                // Chirpper #4 Position
                                case 4:
                                    {
                                        newPos.x = 203;
                                        newPos.y = -66;
                                        break;
                                    }
                                // Chirpper #5 Position
                                case 5:
                                    {
                                        newPos.x = -70;
                                        newPos.y = -135;
                                        break;
                                    }
                                // Chirpper #6 Position
                                case 6:
                                    {
                                        newPos.x = 203;
                                        newPos.y = -135;
                                        break;
                                    }
                                // Default to the first position
                                default:
                                    {
                                        newPos.x = -70;
                                        newPos.y = 2;
                                        break;
                                    }
                            }
                            //newPos.y = 2 - ((i / 4) * 75);
                            //Debug.Log("X-POS: " + newPos.x + " Y-POS: " + newPos.y);
                            GameObject temp = Instantiate(chirpperPrefab, newPos, Quaternion.identity) as GameObject;
                            temp.transform.position = newPos;
                            temp.transform.SetParent(following.transform, false);
                            temp.name = "following" + i;
                            ChirpperInfo ci = temp.GetComponent<ChirpperInfo>();
							temp.transform.Find("Unfollow Button").gameObject.SetActive(true);

                            ci.username.text = words[i];
                            ci.description.text = "This is a small sample of some description text...";
                            ci.time.text = "00:14";
							ci.addUnfollowButtonFunction();
							ci.addFollowButtonFunction();
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Must be logged in to see the people you are following.");
        }
	}

    public IEnumerator getTopChirppers(string thisUsername)
    {
        WWWForm form = new WWWForm();
		form.AddField("getTopChirppers", "");
        if (isLoggedIn)
            form.AddField("username", thisUsername);	
        
        WWW download = new WWW(url, form);
        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            //gets results and stores them into string array split with delimiter \\
            string[] words = download.text.Split(delim, System.StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                //print(i + ": " + words[i]);
            }

            Debug.Log("Total number of Chirppers: " + words[0]);

            GameObject allPeople = GameObject.Find("All People Panel");
            //get 6 recent chirppers and add them to the panel
            if (allPeople != null)
            {
                //Removes previous old chirppers
                if (allPeople.transform.Find("allPeople1") != null)
                    Destroy(allPeople.transform.Find("allPeople1").gameObject);
                if (allPeople.transform.Find("allPeople2") != null)
                    Destroy(allPeople.transform.Find("allPeople2").gameObject);
                if (allPeople.transform.Find("allPeople3") != null)
                    Destroy(allPeople.transform.Find("allPeople3").gameObject);
                if (allPeople.transform.Find("allPeople4") != null)
                    Destroy(allPeople.transform.Find("allPeople4").gameObject);
                if (allPeople.transform.Find("allPeople5") != null)
                    Destroy(allPeople.transform.Find("allPeople5").gameObject);
                if (allPeople.transform.Find("allPeople6") != null)
                    Destroy(allPeople.transform.Find("allPeople6").gameObject);
            }

            if (Convert.ToInt32(words[0]) < 1)
            {
                Debug.Log("There are no Chirppers in the database.");
            }
            else
            {
                menu.noFollowingText.enabled = false;

                // Might not need this check here
                /*for (int i = 1; i < words.Length - 1; i++)
                {
                    Debug.Log("Following #" + i + ": " + words[i]);
                    Debug.Log("Does [" + checkUsername + "] == " + "[" + words[i] + "]?");

                    if (!menu.isFollowing)
                        if (checkUsername == words[i])
                        {
                            Debug.Log("Setting isFollowing to true!");
                            // Set some match variable equal to true
                            menu.isFollowing = true;
                        }
                }*/

                //if (checkUsername == null)
                // Set some match variable equal to false
                //menu.isFollowing = false;

                if (menu.allPeoplePanel.activeSelf)
                {
                    Debug.Log("Displaying a list of all the people on Chirpper...");

                    int index = 1;

                    for (int i = 1; i < Convert.ToInt32(words[0]) * 2 - 1; i += 2)
                    {
                        Debug.Log("Chirpper #" + index + ": " + words[i]);
                        Vector3 newPos = allPeople.transform.position;

                        // Chirpper #1 (-70, 2)     Chirpper #2 (203, 2)
                        // Chirpper #3 (-70, -66)   Chirpper #4 (203, -66)
                        // Chirpper #5 (-70, -135)  Chirpper #6 (203, -135)
                        switch (index)
                        {
                            // Chirpper #1 Position
                            case 1:
                                {
                                    newPos.x = -70;
                                    newPos.y = 2;
                                    break;
                                }
                            // Chirpper #2 Position
                            case 2:
                                {
                                    newPos.x = 203;
                                    newPos.y = 2;
                                    break;
                                }
                            // Chirpper #3 Position
                            case 3:
                                {
                                    newPos.x = -70;
                                    newPos.y = -66;
                                    break;
                                }
                            // Chirpper #4 Position
                            case 4:
                                {
                                    newPos.x = 203;
                                    newPos.y = -66;
                                    break;
                                }
                            // Chirpper #5 Position
                            case 5:
                                {
                                    newPos.x = -70;
                                    newPos.y = -135;
                                    break;
                                }
                            // Chirpper #6 Position
                            case 6:
                                {
                                    newPos.x = 203;
                                    newPos.y = -135;
                                    break;
                                }
                            // Default to the first position
                            default:
                                {
                                    newPos.x = -70;
                                    newPos.y = 2;
                                    break;
                                }
                        }

                        index++;

                        //newPos.y = 2 - ((i / 4) * 75);
                        //Debug.Log("X-POS: " + newPos.x + " Y-POS: " + newPos.y);
                        GameObject temp = Instantiate(chirpperPrefab, newPos, Quaternion.identity) as GameObject;
                        temp.transform.position = newPos;
                        temp.transform.SetParent(allPeople.transform, false);
                        temp.name = "allPeople" + i;
                        ChirpperInfo ci = temp.GetComponent<ChirpperInfo>();

                        ci.username.text = words[i];
                        ci.description.text = "This is a small sample of some description text...";
                        ci.time.text = "00:14";

						if (isLoggedIn){
	                        ci.addUnfollowButtonFunction();
	                        ci.addFollowButtonFunction();

	                        if (words[i + 1] == "true"){
	                            temp.transform.Find("Unfollow Button").gameObject.SetActive(true);
	                        }else{
	                            temp.transform.Find("Follow Button").gameObject.SetActive(true);
	                        }
						}else{
							temp.transform.Find("Unfollow Button").gameObject.SetActive(true);
							temp.transform.Find("Unfollow Button").transform.Find("Text").GetComponent<Text>().text = "Login First!";
						}
                    }
                }
            }
        }
    }

	public void refreshFollowingChirps(){
		StartCoroutine (getFollowingChirps ());
	}

	public IEnumerator getFollowingChirps() {
		print ("Updating Home Chirps Feed");
		WWWForm form = new WWWForm();
		form.AddField( "getFollowingChirps", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {

			GameObject recentChirps = GameObject.Find("Home Chirps Feed Panel");
			//gets results and stores them into string array split with delimiter \\

			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			//get 3 recent chirps and add them to the panel
			if (recentChirps != null){
				//Removes previous old chirps
				if(recentChirps.transform.Find("FollowingChirp0")!= null){
					Destroy(recentChirps.transform.Find("FollowingChirp0").gameObject);
				}
				if(recentChirps.transform.Find("FollowingChirp5")!= null){
					Destroy(recentChirps.transform.Find("FollowingChirp5").gameObject);
				}
				if(recentChirps.transform.Find("FollowingChirp10")!= null){
					Destroy(recentChirps.transform.Find("FollowingChirp10").gameObject);
				}
				//loop a max of 3 times
				int loop = words.Length >= 15? 15: words.Length;

				if (words.Length != 1){
					for (int i = 0; i < loop-1; i+=5){
						Vector3 newPos = recentChirps.transform.position;
						newPos.y = 2 - ((i/5)*75);
						GameObject temp = Instantiate(chirpPrefab,newPos,Quaternion.identity) as GameObject;
						temp.transform.position = newPos;
						temp.transform.SetParent (recentChirps.transform,false);
						temp.name = "FollowingChirp" + i;
						ChirpInfo ci = temp.GetComponent<ChirpInfo>();
						
						DateTime dt = Convert.ToDateTime(words[i+4]); // THIS LINE IS CAUSING AN ERROR
						if ((DateTime.Now - dt).TotalDays >= 1){
							ci.timestamp.text = dt.ToString ("MMM") + " " + dt.Day ;
						}else if ((int)(Math.Round ((DateTime.Now - dt).TotalHours)) > 0){
							ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
						}else{
							ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
						}
						
						ci.id = int.Parse(words[i]);
						ci.username.text = words[i+1];
						ci.title.text = words[i+2];
						if (ci.title.text.Length > 20){
							ci.title.fontSize = 10;
						}
						ci.timer.text = "00:";
						ci.timer.text += int.Parse (words[i+3]) < 10? "0" + words[i+3]:words[i+3];
						ci.addButtonFunction();
                        ci.addProfileButtonFunction();

						if (isLoggedIn){
						
							if (menu.currentUsername.text.Substring(1) == ci.username.text){
								ci.transform.Find("Delete Button").gameObject.SetActive(true);
								ci.addDeleteButtonFunction();
							}
						}
					}

//					if (username != ci.username.text){
//
//						form = new WWWForm();
//						
//						form.AddField( "getUsersProfilePicture", ci.username.text);
//						download =  new WWW( url, form);
//						yield return download;
//						
//						//couldn't get extension for file
//						if(!string.IsNullOrEmpty(download.error)) {
//							print( "Error downloading: " + download.error );
//							
//						} else {
//							//download.text should be extension of file
//							WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + username + download.text);
//							yield return www;
//							
//							if (!string.IsNullOrEmpty (www.error)) {
//								print("No Picture found, using default");
//							} else {
//								ci.profilePicture.texture = www.texture;
//							}
//						}
//					}else{
//						ci.profilePicture.texture = myProfilePicture.texture;
//					}

				}
			}
		}
	}

	public void deleteChirpVoid(int id){
		StartCoroutine (deleteChirp (id));
		refreshAllPanels ();
	}
	
	public IEnumerator deleteChirp(int id) {
		WWWForm form = new WWWForm();
		//form.AddBinaryData("binary", new byte[1]);
		form.AddField( "deleteChirp", id );	
		form.AddField( "username", menu.currentUsername.text.Substring(1));

		WWW download = new WWW( url, form);
		yield return download;
		
		if (!string.IsNullOrEmpty (download.error)) {
			print ("Error downloading: " + download.error);
		} else {
			string[] words = download.text.Split(delim, System.StringSplitOptions.None);
			menu.SetUserInfo("nc", "nc",words[0] );
			print(download.text);
		}
	}
	
	public IEnumerator sendChirp(string chirpTitle, byte[] toSend, int length) {
		WWWForm form = new WWWForm();
		form.AddField( "username",  menu.currentUsername.text.ToString().Substring(1));
		form.AddField( "chirpTitle", chirpTitle);
		if (length != 0) {
			form.AddField ("length", length);
		} else {
			form.AddField ("length", 1);
		}

        //byte[] toSend = new byte[fs.Length];
		form.AddBinaryData ("chirpData", toSend);
		WWW download =  new WWW( url, form);
		yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            //gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim, System.StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                //print(i + ": " + words[i]);
            }

            Debug.Log("Sending new chirp to the Chirpper database...");

            if (words[0] == "false")
            {
                Debug.Log("Failed to send new chirp!");
                // Display an error message
                //menu.DisplayAddAccountFailedPanel();
            }
            else
            {
                Debug.Log("Successfully sent new chirp!");
                Debug.Log("Current number of chirps: " + words[0]);
				refreshAllPanels();
				menu.SetUserInfo("nc", "nc", words[0]);
            }


            if (download.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                cookie = download.responseHeaders["SET-COOKIE"];
            }
        }
	}

	void Update(){
		if (playingButton != null) {
			if(isLoading){
				playingButton.image.overrideSprite = loadingImage;
			}else if (source.isPlaying) {
				playingButton.image.overrideSprite = stopImage;
			} else {
				playingButton.image.overrideSprite = null;
			}
		}
	}
	
	public IEnumerator login(string oldUsername, string oldPassword) {
		username = oldUsername;
        WWWForm form = new WWWForm();
        form.AddField("login", oldUsername);
        form.AddField("username", oldUsername);
        form.AddField("password", oldPassword);

        Debug.Log("Checking username and password entered...");
        Debug.Log("Username: " + oldUsername);
        Debug.Log("Password: " + oldPassword);
		
		WWW download = new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			Debug.Log( "Error downloading: " + download.error );
		} else {

			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			for (int i = 0; i < words.Length; i ++){
				print(i + ": " + words[i]);
			}

            Debug.Log("Checking result...");

            if (words[0] == "false")
            {
                Debug.Log("Login Failed!");
                // Display an error message
                menu.DisplayLoginFailedPanel();
            }
            else 
            {
                Debug.Log("Login Successful!");
                // Collect info to display on user profile page
                menu.SetUserInfo(words[1], words[2], words[3]);
                menu.DisplayLoggedInPanel();
                isLoggedIn = true;

//				form = new WWWForm();
//				
//				form.AddField( "getUsersProfilePicture", oldUsername);
//				download =  new WWW( url, form);
//				yield return download;
//				
//				//couldn't get extension for file
//				if(!string.IsNullOrEmpty(download.error)) {
//					print( "Error downloading: " + download.error );
//					
//				} else {
//					//download.text should be extension of file
//					WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + oldUsername + words[0]);
//					yield return www;
//					
//					if (!string.IsNullOrEmpty (www.error)) {
//						print("No Picture found, using default");
//					} else {
//						myProfilePicture.texture = www.texture;
//					}
			//	}
				StartCoroutine(getFollowingChirps());


            }


			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
			}
		}
	}

	public void refreshRecentChirps(){
		StartCoroutine (getRecentChirps ());
	}

	public IEnumerator getRecentChirps() {
		print("Updating Recent Chirps Feed");
		WWWForm form = new WWWForm();
		form.AddField( "getRecentChirps", "" );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			GameObject recentChirps = GameObject.Find("Recent Chirps Feed Panel");
			if (recentChirps != null){
				//gets results and stores them into string array split with delimiter \\
				string[] words = download.text.Split(delim,System.StringSplitOptions.None);

				//Removes previous old chirps
				if(recentChirps.transform.Find("Chirp0")!= null){
					Destroy(recentChirps.transform.Find("Chirp0").gameObject);
				}
				if(recentChirps.transform.Find("Chirp5")!= null){
					Destroy(recentChirps.transform.Find("Chirp5").gameObject);
				}
				if(recentChirps.transform.Find("Chirp10")!= null){
					Destroy(recentChirps.transform.Find("Chirp10").gameObject);
				}
				//get 3 recent chirps and add them to the panel
				for (int i = 0; i < 15; i+=5){
				    
					Vector3 newPos = recentChirps.transform.position;
					newPos.y = 2 - ((i/5)*75);
					GameObject temp = Instantiate(chirpPrefab,newPos,Quaternion.identity) as GameObject;
					temp.transform.position = newPos;
					temp.transform.SetParent (recentChirps.transform,false);
					temp.name = "Chirp" +i;

					ChirpInfo ci = temp.GetComponent<ChirpInfo>();

					DateTime dt = Convert.ToDateTime(words[i+4]);
					if ((DateTime.Now - dt).TotalDays >= 1){
						ci.timestamp.text = dt.ToString ("MMM") + " " + dt.Day ;
					}else if ((int)(Math.Round ((DateTime.Now - dt).TotalHours))> 0){
						ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
					}else{
						ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
					}

					ci.id = int.Parse(words[i]);
					ci.username.text = words[i+1];
					
					ci.title.text = words[i+2];
					if (ci.title.text.Length > 20){
						ci.title.fontSize = 10;
					}
					ci.timer.text = "00:";
					ci.timer.text += int.Parse (words[i+3]) < 10? "0" + words[i+3]:words[i+3];

					ci.addButtonFunction();
					ci.addProfileButtonFunction();
					if (isLoggedIn){
						if (menu.currentUsername.text.Substring(1) == ci.username.text){
							ci.transform.Find("Delete Button").gameObject.SetActive(true);
							ci.addDeleteButtonFunction();
						}
					}

//				if (username != ci.username.text){
//
//					form = new WWWForm();
//					
//					form.AddField( "getUsersProfilePicture", ci.username.text);
//					download =  new WWW( url, form);
//					yield return download;
//					
//					//couldn't get extension for file
//					if(!string.IsNullOrEmpty(download.error)) {
//						print( "Error downloading: " + download.error );
//						
//					} else {
//						//download.text should be extension of file
//						WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + ci.username.text + download.text);
//						yield return www;
//						
//						if (!string.IsNullOrEmpty (www.error)) {
//							print("No Picture found, using default");
//						} else {
//							ci.profilePicture.texture = www.texture;
//						}
//					}
//
//				}else{
//					ci.profilePicture.texture = myProfilePicture.texture;
//				}
				}
			}
		}
	}

	public IEnumerator wait(int seconds){
		yield return new WaitForSeconds (seconds);
	}

	public IEnumerator stream(int id,Button button) {
		if (playingButton != button || (!source.isPlaying && !isLoading)) {
			if (playingButton != null) {
				playingButton.image.overrideSprite = null;
			}
			playingButton = button;
			isLoading = true;
			playingButton.image.overrideSprite = stopImage;
			print ("streaming id " + id + " yo");		
			WWW www = new WWW ("https://s3.amazonaws.com/chirpper/" + id + ".wav");
			//wait for 10% buffer
//			while (www.progress < 0.1f) {
//				yield return null;
//			}
			//set audio clip stream up
			source.clip = www.GetAudioClip (false, true, AudioType.WAV);
			while (!www.audioClip.isReadyToPlay) {
				yield return null;
			}
			isLoading = false;
			source.clip = www.audioClip;
			source.Play ();
		} else {
			source.Stop();
			playingButton.image.overrideSprite = null;
			print("stopped yo");

		}

	}

	public void clearAllPanels(){


		GameObject allPeople = GameObject.Find("All People Panel");
		//get 6 recent chirppers and add them to the panel
		if (allPeople != null)
		{
			//Removes previous old chirppers
			if (allPeople.transform.Find("allPeople1") != null)
				Destroy(allPeople.transform.Find("allPeople1").gameObject);
			if (allPeople.transform.Find("allPeople2") != null)
				Destroy(allPeople.transform.Find("allPeople2").gameObject);
			if (allPeople.transform.Find("allPeople3") != null)
				Destroy(allPeople.transform.Find("allPeople3").gameObject);
			if (allPeople.transform.Find("allPeople4") != null)
				Destroy(allPeople.transform.Find("allPeople4").gameObject);
			if (allPeople.transform.Find("allPeople5") != null)
				Destroy(allPeople.transform.Find("allPeople5").gameObject);
			if (allPeople.transform.Find("allPeople6") != null)
				Destroy(allPeople.transform.Find("allPeople6").gameObject);
		}
	
		GameObject recentChirps = menu.homeChirpsPanel; 
		recentChirps.SetActive (true);
		//get 3 recent chirps and add them to the panel
		if (recentChirps != null) {
			//Removes previous old chirps
			if (recentChirps.transform.Find ("FollowingChirp0") != null) {
				Destroy (recentChirps.transform.Find ("FollowingChirp0").gameObject);
			}
			if (recentChirps.transform.Find ("FollowingChirp5") != null) {
				Destroy (recentChirps.transform.Find ("FollowingChirp5").gameObject);
			}
			if (recentChirps.transform.Find ("FollowingChirp10") != null) {
				Destroy (recentChirps.transform.Find ("FollowingChirp10").gameObject);
			}
		}
		recentChirps.SetActive (false);

		GameObject following = menu.followingPanel;
		following.SetActive (true);
		//get 6 recent chirppers and add them to the panel
		if (following != null){
			//Removes previous old chirppers
			if (following.transform.Find("following1") != null)
				Destroy(following.transform.Find("following1").gameObject);
			if (following.transform.Find("following2") != null)
				Destroy(following.transform.Find("following2").gameObject);
			if (following.transform.Find("following3") != null)
				Destroy(following.transform.Find("following3").gameObject);
			if (following.transform.Find("following4") != null)
				Destroy(following.transform.Find("following4").gameObject);
			if (following.transform.Find("following5") != null)
				Destroy(following.transform.Find("following5").gameObject);
			if (following.transform.Find("following6") != null)
				Destroy(following.transform.Find("following6").gameObject);
		}
		following.SetActive (false);


		GameObject followers = menu.followersPanel;
		//get 6 recent chirppers and add them to the panel
		followers.SetActive (true);
		if (followers != null){
			//Removes previous old chirppers
			if (followers.transform.Find("followers1") != null)
				Destroy(followers.transform.Find("followers1").gameObject);
			if (followers.transform.Find("followers2") != null)
				Destroy(followers.transform.Find("followers2").gameObject);
			if (followers.transform.Find("followers3") != null)
				Destroy(followers.transform.Find("followers3").gameObject);
			if (followers.transform.Find("followers4") != null)
				Destroy(followers.transform.Find("followers4").gameObject);
			if (followers.transform.Find("followers5") != null)
				Destroy(followers.transform.Find("followers5").gameObject);
			if (followers.transform.Find("followers6") != null)
				Destroy(followers.transform.Find("followers6").gameObject);
		}
		followers.SetActive(false);

		GameObject searchPanel = menu.searchPanel;
		//get 6 recent chirppers and add them to the panel
		searchPanel.SetActive (true);
		if (searchPanel != null) {
			//Removes previous old searched users
			if (searchPanel.transform.Find ("searchUser1") != null)
				Destroy (searchPanel.transform.Find ("searchUser1").gameObject);
			if (searchPanel.transform.Find ("searchUser2") != null)
				Destroy (searchPanel.transform.Find ("searchUser2").gameObject);
			
			//found chirps
			if (searchPanel.transform.Find ("searchChirp1") != null)
				Destroy (searchPanel.transform.Find ("searchChirp1").gameObject);
			if (searchPanel.transform.Find ("searchChirp2") != null)
				Destroy (searchPanel.transform.Find ("searchChirp2").gameObject);
			if (searchPanel.transform.Find ("searchChirp3") != null)
				Destroy (searchPanel.transform.Find ("searchChirp3").gameObject);
			if (searchPanel.transform.Find ("searchChirp4") != null)
				Destroy (searchPanel.transform.Find ("searchChirp4").gameObject);
			if (searchPanel.transform.Find ("searchChirp5") != null)
				Destroy (searchPanel.transform.Find ("searchChirp5").gameObject);
			if (searchPanel.transform.Find ("searchChirp6") != null)
				Destroy (searchPanel.transform.Find ("searchChirp6").gameObject);
			
		}
		searchPanel.SetActive (false);


		recentChirps = menu.myChirpsPanel;
		recentChirps.SetActive (true);
		if (recentChirps != null) {
			//Removes previous old chirps
			if (recentChirps.transform.Find ("myChirp0") != null) {
				Destroy (recentChirps.transform.Find ("myChirp0").gameObject);
			}
			if (recentChirps.transform.Find ("myChirp4") != null) {
				Destroy (recentChirps.transform.Find ("myChirp4").gameObject);
			}			
			if (recentChirps.transform.Find ("myChirp8") != null) {
				Destroy (recentChirps.transform.Find ("myChirp8").gameObject);
			}
		}
		recentChirps.SetActive (false);

		recentChirps = menu.recentChirpsPanel;
		recentChirps.SetActive (true);
		if (recentChirps != null) {
			//Removes previous old chirps
			if (recentChirps.transform.Find ("Chirp0") != null) {
				Destroy (recentChirps.transform.Find ("Chirp0").gameObject);
			}
			if (recentChirps.transform.Find ("Chirp5") != null) {
				Destroy (recentChirps.transform.Find ("Chirp5").gameObject);
			}
			if (recentChirps.transform.Find ("Chirp10") != null) {
				Destroy (recentChirps.transform.Find ("Chirp10").gameObject);
			}
		}
		refreshRecentChirps ();
	}

	public void refreshAllPanels(){
		refreshMyChirps ();
		refreshFollowingChirps ();
		refreshRecentChirps ();
	}

    public void refreshFollowingPanel()
    {
        string temp = menu.currentUsername.text.Remove(0, 1);
        StartCoroutine(getFollowing(temp, null));
    }

    public void refreshFollowersPanel()
    {
        string temp = menu.currentUsername.text.Remove(0, 1);
        StartCoroutine(getFollowers(temp, null));
    }
	
//	public IEnumerator sendProfilePicture() {
//		string pathname = EditorUtility.OpenFilePanel("Load file","","*.png,*.jpg");
//		if (pathname != "") {
//			string extension = pathname.Substring (pathname.LastIndexOf ('.'));
//			extension = extension.ToLower ();
//			if (extension == ".jpg" || extension == ".png") {
//				WWWForm form = new WWWForm ();
//				FileStream fs = new FileStream (pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
//				BinaryReader reader = new BinaryReader (fs);
//				byte[] toSend = new byte[fs.Length];
//				fs.Read (toSend, 0, System.Convert.ToInt32 (fs.Length));
//				fs.Close ();
//			
//				form.AddField ("username", username);
//				form.AddField ("extension", extension);
//				form.AddBinaryData ("profilePicture", toSend);
//				WWW download = new WWW (url, form);
//				yield return download;
//			
//				if (!string.IsNullOrEmpty (download.error)) {
//					print ("Error downloading: " + download.error);
//				} else {
//					print (download.text);
//				}
//			} else {
//				print ("ERROR: Selected file is not a valid extension");
//			}
//		
//		}
//	}

}
