using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
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
    string searchField="Search!";
    string follow="Enter a User To Follow!";
	Text test;
	bool signedUp = false;
	static string[] delim = {"\\"};
	public Button playingButton;
	public Sprite stopImage;
	public AudioSource source;
	bool isLoading = false;
    GameObject menuGUI;
    MainMenuGUI menu;

    public bool isLoggedIn = false;

	public GameObject chirpPrefab;
    public GameObject chirpperPrefab;

	public Button uploadPicture;

	void Start () 
    {
		source = gameObject.AddComponent<AudioSource> ();
		test =  GameObject.Find("Text").GetComponent<Text>();

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
			//print(download.text);
			//test.text = download.text;
		}
	}

	IEnumerator search(string search) {
		WWWForm form = new WWWForm();
		form.AddField( "search", search );
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
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

	/*IEnumerator signUp() {
		WWWForm form = new WWWForm();
		form.AddField( "signUp", "true" );	
		form.AddField( "username", username );
		form.AddField( "password", password );
		form.AddField( "email", email );	
		form.AddField( "name", name );	
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
                signedUp = false;
			}
			print(download.text);
			test.text = download.text;	
		}
	}*/

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


	public IEnumerator getChirps(string username) {
		WWWForm form = new WWWForm();
		form.AddField( "getUsersChirps", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print("DOWNLOADED TEXT: " + download.text);
			GameObject recentChirps = GameObject.Find("My Chirps Feed Panel");
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			//get 3 recent chirps and add them to the panel
			
			//Removes previous old chirps
			if(recentChirps.transform.Find("myChirp0")!= null){
				Destroy(recentChirps.transform.Find("myChirp0").gameObject);
				Destroy(recentChirps.transform.Find("myChirp4").gameObject);
				Destroy(recentChirps.transform.Find("myChirp8").gameObject);
			}
			//loop a max of 3 times
			int loop = words.Length >= 12? 12: words.Length;
			
			
			for (int i = 0; i < loop; i+=4){
				Vector3 newPos = recentChirps.transform.position;
				newPos.y = 2 - ((i/4)*75);
				GameObject temp = Instantiate(chirpPrefab,newPos,Quaternion.identity) as GameObject;
				temp.transform.position = newPos;
				temp.transform.SetParent (recentChirps.transform,false);
				temp.name = "myChirp" + i;
				ChirpInfo ci = temp.GetComponent<ChirpInfo>();
				
				DateTime dt = Convert.ToDateTime(words[i+3]); // THIS LINE IS CAUSING AN ERROR
				if ((DateTime.Now - dt).TotalDays >= 1){
					ci.timestamp.text = dt.ToString ("MMM") + " " + dt.Day ;
				}else if ((DateTime.Now - dt).TotalHours > 0){
					ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
				}else{
					ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
				}
				
				ci.id = int.Parse(words[i]);
				//ci.username.text = words[i+1];
				ci.title.text = words[i+1];
				ci.timer.text = "00:";
				ci.timer.text += int.Parse (words[i+2]) < 10? "0" + words[i+2]:words[i+2];
				ci.addButtonFunction();
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


			print(download.text);
			test.text = download.text;
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
                        GameObject followers = GameObject.Find("Followers Panel");
                        //get 6 recent chirppers and add them to the panel

                        //Removes previous old chirppers
                        /*if (followers.transform.Find("followers1") != null)
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
                            Destroy(followers.transform.Find("followers6").gameObject);*/

                        if (followers.transform.Find("following1") != null)
                        {
                            Debug.Log("Destroying old chirpper list...");

                            Destroy(followers.transform.Find("followers1").gameObject);
                            Destroy(followers.transform.Find("followers2").gameObject);
                            Destroy(followers.transform.Find("followers3").gameObject);
                            Destroy(followers.transform.Find("followers4").gameObject);
                            Destroy(followers.transform.Find("followers5").gameObject);
                            Destroy(followers.transform.Find("followers6").gameObject);
                        }

                        //loop a max of 6 times
                        int loop = 0;

                        if (words.Length > 5)
                            loop = 6;
                        else
                            loop = words.Length - 1;

                        for (int i = 1; i < loop; i++)
                        {
                            Debug.Log("Following #" + i + ": " + words[i]);
                            Vector3 newPos = followers.transform.position;

                            // Chirpper #1 (-70, 2)     Chirpper #2 (203, 2)
                            // Chirpper #3 (-70, -66)   Chirpper #4 (203, -66)
                            // Chirpper #5 (-70, -135)  Chirpper #6 (203, -135)
                            switch (i)
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
                            temp.transform.SetParent(followers.transform, false);
                            temp.name = "followers" + i;
                            ChirpperInfo ci = temp.GetComponent<ChirpperInfo>();

                            ci.username.text = words[i];
                            ci.description.text = "This is a small sample of some description text...";
                            ci.time.text = "00:14";
                            ci.addUnfollowButtonFunction();
                        }
                    }
                }

                //print(download.text);
                //test.text = download.text;
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
                        GameObject following = GameObject.Find("Following Panel");
                        //get 6 recent chirppers and add them to the panel

                        //Removes previous old chirppers
                        /*if (following.transform.Find("following1") != null)
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
                            Destroy(following.transform.Find("following6").gameObject);*/

                        if (following.transform.Find("following1") != null)
                        {
                            Debug.Log("Destroying old chirpper list...");

                            Destroy(following.transform.Find("following1").gameObject);
                            Destroy(following.transform.Find("following2").gameObject);
                            Destroy(following.transform.Find("following3").gameObject);
                            Destroy(following.transform.Find("following4").gameObject);
                            Destroy(following.transform.Find("following5").gameObject);
                            Destroy(following.transform.Find("following6").gameObject);
                        }

                        //loop a max of 6 times
                        int loop = 0;

                        if (words.Length > 5)
                            loop = 6;
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

                            ci.username.text = words[i];
                            ci.description.text = "This is a small sample of some description text...";
                            ci.time.text = "00:14";
                            ci.addUnfollowButtonFunction();
                        }
                    }
                }

                //print(download.text);
                //test.text = download.text;
            }
        }
        else
        {
            Debug.Log("Must be logged in to see the people you are following.");
        }
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

			//Removes previous old chirps
			if(recentChirps.transform.Find("FollowingChirp0")!= null){
				Destroy(recentChirps.transform.Find("FollowingChirp0").gameObject);
				Destroy(recentChirps.transform.Find("FollowingChirp5").gameObject);
				Destroy(recentChirps.transform.Find("FollowingChirp10").gameObject);
			}
			//loop a max of 3 times
			int loop = words.Length >= 15? 15: words.Length;


			for (int i = 0; i < loop; i+=5){
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
				}else if ((DateTime.Now - dt).TotalHours > 0){
					ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalHours)) + " hr ago";
				}else{
					ci.timestamp.text = (int)(Math.Round ((DateTime.Now - dt).TotalMinutes)) + " min ago";
				}
				
				ci.id = int.Parse(words[i]);
				ci.username.text = words[i+1];
				ci.title.text = words[i+2];
				ci.timer.text = "00:";
				ci.timer.text += int.Parse (words[i+3]) < 10? "0" + words[i+3]:words[i+3];
				ci.addButtonFunction();




				form = new WWWForm();
				
				form.AddField( "getUsersProfilePicture", ci.username.text);
				download =  new WWW( url, form);
				yield return download;
				
				//couldn't get extension for file
				if(!string.IsNullOrEmpty(download.error)) {
					print( "Error downloading: " + download.error );
					
				} else {
					//download.text should be extension of file
					WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + username + download.text);
					yield return www;
					
					if (!string.IsNullOrEmpty (www.error)) {
						print("No Picture found, using default");
					} else {
						ci.profilePicture.texture = www.texture;
						//result(www.texture);
					}
				}

			//	StartCoroutine(getProfilePicture(ci.profilePicture,ci.username.text,value => ci.profilePicture.texture = value));

			}
		}
	}
	
	IEnumerator deleteChirp(int id) {
		WWWForm form = new WWWForm();
		//form.AddBinaryData("binary", new byte[1]);
		form.AddField( "deleteChirp", id );	
		
		WWW download = new WWW( url, form);
		yield return download;
		
		if (!string.IsNullOrEmpty (download.error)) {
			print ("Error downloading: " + download.error);
		} else {
			test.text = download.text;
			print (download.text);
		}
	}
	
	public IEnumerator sendChirp(string chirpTitle, byte[] toSend, int length) {
		WWWForm form = new WWWForm();
		//FileStream fs = new FileStream ("assets\\myChirp.wav", FileMode.Open, FileAccess.Read,FileShare.Read);
		//BinaryReader reader = new BinaryReader(fs);
		//byte[] toSend = new byte[fs.Length];
		//fs.Read(toSend,0,System.Convert.ToInt32(fs.Length));
		//fs.Close ();
		
		form.AddField( "username",  menu.currentUsername.text.ToString().Substring(1));
		form.AddField( "chirpTitle", chirpTitle);
		form.AddField ("length", length);

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
			if (!source.isPlaying && !isLoading) {
				playingButton.image.overrideSprite = null;
			} else {
				playingButton.image.overrideSprite = stopImage;
			}
		}
	}
	
	public IEnumerator login(string oldUsername, string oldPassword) {
		username = oldUsername;
        WWWForm form = new WWWForm();
		//form.AddBinaryData("binary", new byte[1]);
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
				StartCoroutine(getFollowingChirps());
                // Collect info to display on user profile page
                menu.SetUserInfo(words[0], words[1], words[2]);
                menu.DisplayLoggedInPanel();
                isLoggedIn = true;

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
//					WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + username + words[3]);
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


			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
			}
		}
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

			//print(download.text);
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);

			//Removes previous old chirps
			if(recentChirps.transform.Find("Chirp0")!= null){
				Destroy(recentChirps.transform.Find("Chirp0").gameObject);
				Destroy(recentChirps.transform.Find("Chirp5").gameObject);
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
				}else if ((DateTime.Now - dt).TotalHours > 0){
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



				form = new WWWForm();
				
				form.AddField( "getUsersProfilePicture", ci.username.text);
				download =  new WWW( url, form);
				yield return download;
				
				//couldn't get extension for file
				if(!string.IsNullOrEmpty(download.error)) {
					print( "Error downloading: " + download.error );
					
				} else {
					//download.text should be extension of file
					WWW www = new WWW("https://s3.amazonaws.com/chirpperprofilepicture/" + username + download.text);
					yield return www;
					
					if (!string.IsNullOrEmpty (www.error)) {
						print("No Picture found, using default");
					} else {
						ci.profilePicture.texture = www.texture;
						//result(www.texture);
					}
				}

				//StartCoroutine(getProfilePicture(ci.profilePicture,ci.username.text,value => ci.profilePicture.texture = value));


			}
		}
	}

	public IEnumerator wait(int seconds){
		yield return new WaitForSeconds (seconds);
	}

	public IEnumerator stream(int id,Button button) {
		if (playingButton != button || !source.isPlaying) {
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


	
	public IEnumerator sendProfilePicture() {
		string pathname = EditorUtility.OpenFilePanel("Load file","","*.png,*.jpg");
		if (pathname != "") {
			string extension = pathname.Substring (pathname.LastIndexOf ('.'));
			extension = extension.ToLower ();
			if (extension == ".jpg" || extension == ".png") {
				WWWForm form = new WWWForm ();
				FileStream fs = new FileStream (pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
				BinaryReader reader = new BinaryReader (fs);
				byte[] toSend = new byte[fs.Length];
				fs.Read (toSend, 0, System.Convert.ToInt32 (fs.Length));
				fs.Close ();
			
				form.AddField ("username", username);
				form.AddField ("extension", extension);
				form.AddBinaryData ("profilePicture", toSend);
				WWW download = new WWW (url, form);
				yield return download;
			
				if (!string.IsNullOrEmpty (download.error)) {
					print ("Error downloading: " + download.error);
				} else {
					print (download.text);
				}
			} else {
				print ("ERROR: Selected file is not a valid extension");
			}
		
		}
	}


	/*void OnGUI(){
		searchField = GUI.TextField (new Rect (600, 10, 200, 20), searchField, 25);
		if (GUI.Button(new Rect(800,10,200,20),"Search Chirpper!")){
			StartCoroutine(search(searchField));
		}

		if (GUI.Button (new Rect (410, 140, 200, 20), "get Recent Chirps")) {
			StartCoroutine (getRecentChirps ());
		}

		//not signed in
		if (cookie == "") {
			
			username = GUI.TextField (new Rect (10, 10, 200, 20), username, 25);
			password = GUI.PasswordField (new Rect (10, 40, 200, 20), password, '*', 25);
			
			//default view
            if (!signedUp)
            {
				if (GUI.Button (new Rect (10, 70, 200, 20), "Login")) {
					StartCoroutine (Login ());
				}
				if (GUI.Button (new Rect (10, 100, 200, 20), "Sign Up")) {
                    signedUp = true;
					
				}	
				//attempting to sign up
			} else {
				passwordConfirm = GUI.PasswordField(new Rect(10, 70, 200, 20), passwordConfirm,'*', 25);
				name = GUI.TextField(new Rect(10, 100, 200, 20), name, 25);
				email = GUI.TextField(new Rect(10, 130, 200, 20), email, 25);
				if (GUI.Button (new Rect (10, 220, 200, 20), "Sign Up")) {
                    signedUp = true;
                    StartCoroutine(signUp());
				}	
				if (GUI.Button (new Rect (10, 190, 200, 20), "Cancel")) {
                    signedUp = false;
				}	
			}
			
			//signed in
		} else {
			chirpTitle = GUI.TextField (new Rect (10, 10, 200, 20), chirpTitle, 25);

			if (GUI.Button(new Rect(10,40,200,20),"Chirp it!")){
				StartCoroutine(sendChirp ());
			}

			if (GUI.Button(new Rect(210,40,200,20),"Delete Chirp")){
				StartCoroutine(deleteChirp (3));
			}

			
			if (GUI.Button (new Rect (410, 70, 200, 20), "Stream")) {
				StartCoroutine (stream (5));
			}
			
			if (GUI.Button (new Rect (410, 100, 200, 20), "Get Users Chirps")) {
				StartCoroutine (getChirps ("John"));
			}


			follow = GUI.TextField (new Rect (10, 70, 200, 20), follow, 25);
			if (GUI.Button(new Rect(10,100,200,20),"Follow User!")){
				StartCoroutine(followUser ());
			}

			if (GUI.Button(new Rect(210,100,200,20),"Unfollow User!")){
				StartCoroutine(unfollowUser ());
			}
			
			if (GUI.Button(new Rect(10,130,200,20),"Get Who I'm Following")){
				StartCoroutine(getFollowing ());
			}

			if (GUI.Button(new Rect(210,130,200,20),"Get Who's Following Me")){
				StartCoroutine(getFollowers ());
			}


			if (GUI.Button(new Rect(10,160,200,20),"Chirps From Users I'm Following")){
				StartCoroutine(getFollowingChirps ());
			}
		}
	}*/
}
