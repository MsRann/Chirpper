using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.IO;
using System.Net;
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
	
	public AudioSource source;

    GameObject menuGUI;
    MainMenuGUI menu;

	void Start () 
    {
		source = GetComponent<AudioSource>();
		test =  GameObject.Find("Text").GetComponent<Text>();

        menuGUI = GameObject.FindGameObjectWithTag("Menu");
        menu = menuGUI.GetComponent<MainMenuGUI>();
	}
	
	IEnumerator followUser() {
		WWWForm form = new WWWForm();
		form.AddField( "username", username );
		form.AddField( "follow", follow);
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
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

	IEnumerator unfollowUser() {
		WWWForm form = new WWWForm();
		form.AddField( "username", username );
		form.AddField( "unfollow", follow);
		WWW download = new WWW( url, form );
		
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
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
            }


            if (download.responseHeaders.ContainsKey("SET-COOKIE"))
            {
                cookie = download.responseHeaders["SET-COOKIE"];
            }
        }
    }


	IEnumerator getChirps(string username) {
		WWWForm form = new WWWForm();
		//	Dictionary<string,string> headers = form.headers;
		//	headers ["COOKIE"] = cookie;
		form.AddField( "getUsersChirps", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
		}
	}

	IEnumerator getFollowers() {
		WWWForm form = new WWWForm();
		form.AddField( "getFollowers", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			for (int i = 0; i < words.Length; i ++){
				print(i + ": " + words[i]);
			}
			//print(download.text);
			//test.text = download.text;
		}
	}

	IEnumerator getFollowing() {
		WWWForm form = new WWWForm();
		form.AddField( "getFollowing", username );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			for (int i = 0; i < words.Length; i ++){
				print(i + ": " + words[i]);
			}
			//print(download.text);
			//test.text = download.text;
		}
	}

	IEnumerator getFollowingChirps() {
		WWWForm form = new WWWForm();
		//	Dictionary<string,string> headers = form.headers;
		//	headers ["COOKIE"] = cookie;
		form.AddField( "getFollowingChirps", username );
		WWW download = new WWW( url, form );
		//WWW download =  new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
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
	
	public IEnumerator sendChirp(string chirpTitle, byte[] toSend) {
		WWWForm form = new WWWForm();
		//FileStream fs = new FileStream ("assets\\myChirp.wav", FileMode.Open, FileAccess.Read,FileShare.Read);
		//BinaryReader reader = new BinaryReader(fs);
		//byte[] toSend = new byte[fs.Length];
		//fs.Read(toSend,0,System.Convert.ToInt32(fs.Length));
		//fs.Close ();
		
		form.AddField( "username",  menu.currentUsername.text.ToString());
		form.AddField( "chirpTitle", chirpTitle);

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
	
	public IEnumerator login(string oldUsername, string oldPassword) {
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

                // Collect info to display on user profile page
                menu.SetUserInfo(words[0], words[1], words[2]);
                menu.DisplayLoggedInPanel();
            }


			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
			}
		}
	}

	IEnumerator getRecentChirps() {
		WWWForm form = new WWWForm();
		form.AddField( "getRecentChirps", "" );
		WWW download = new WWW( url, form );
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			//gets results and stores them into string array split with delimiter \\
			string[] words = download.text.Split(delim,System.StringSplitOptions.None);
			for (int i = 0; i < words.Length; i ++){
				print(i + ": " + words[i]);
			}
		}
	}

	IEnumerator stream(int id) {
		WWW www = new WWW("https://s3.amazonaws.com/chirpper/" + id + ".wav");
		//wait for 10% buffer
		while (www.progress < 0.1f) {
			yield return null;
		}
		//set audio clip stream up
		source.clip = www.GetAudioClip (false, true,AudioType.WAV);
		while (!www.audioClip.isReadyToPlay) {
			yield return null;
		}
		source.clip = www.audioClip;
		source.Play ();
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
