using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.IO;
using System.Net;
using System.Collections.Generic;

public class NetworkThing : MonoBehaviour {
	
	string url = "https://chirpper.herokuapp.com/";
	string username = "John",password = "Doe",passwordConfirm="",name="name",email="email",cookie = "",searchField="Search!";
	string chirpTitle = "Chirp Something!",follow="Enter a User To Follow!";
	Text test;
	bool signUp = false;
	
	
	public AudioSource source;


	void Start () {
		source = GetComponent<AudioSource>();
		test =  GameObject.Find("Text").GetComponent<Text>();
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

	
	IEnumerator SignUp() {
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
				signUp = false;
			}
			print(download.text);
			test.text = download.text;
			
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


	IEnumerator getFollowing() {
		WWWForm form = new WWWForm();
		//	Dictionary<string,string> headers = form.headers;
		//	headers ["COOKIE"] = cookie;
		form.AddField( "followingPage", username );
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
	
	IEnumerator sendChirp() {
		WWWForm form = new WWWForm();
		FileStream fs = new FileStream ("assets\\myChirp.wav", FileMode.Open, FileAccess.Read,FileShare.Read);
		BinaryReader reader = new BinaryReader(fs);
		byte[] toSend = new byte[fs.Length];
		fs.Read(toSend,0,System.Convert.ToInt32(fs.Length));
		fs.Close ();
		
		form.AddField( "username", username );
		form.AddField( "chirpTitle", chirpTitle );
		
		form.AddBinaryData ("chirpData", toSend);
		WWW download =  new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			print(download.text);
			test.text = download.text;
		}
		
	}
	
	IEnumerator Login() {
		WWWForm form = new WWWForm();
		//form.AddBinaryData("binary", new byte[1]);
		form.AddField( "login", username );
		form.AddField( "username", username );
		form.AddField( "password", password );		
		
		WWW download = new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			//gets number of followers, number of users following, and number of chirps in title
			test.text = download.text;
			print(download.text);
			
			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
			}
		}
	}

	IEnumerator getRecentChirps() {
		WWWForm form = new WWWForm();
		//	Dictionary<string,string> headers = form.headers;
		//	headers ["COOKIE"] = cookie;
		form.AddField( "getRecentChirps", "" );
		WWW download = new WWW( url, form );
		//WWW download =  new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			// show the highscores
			print(download.text);
			test.text = download.text;
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


	void OnGUI(){
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
			if (!signUp) {
				if (GUI.Button (new Rect (10, 70, 200, 20), "Login")) {
					StartCoroutine (Login ());
				}
				if (GUI.Button (new Rect (10, 100, 200, 20), "Sign Up")) {
					signUp = true;
					
				}	
				//attempting to sign up
			} else {
				passwordConfirm = GUI.PasswordField(new Rect(10, 70, 200, 20), passwordConfirm,'*', 25);
				name = GUI.TextField(new Rect(10, 100, 200, 20), name, 25);
				email = GUI.TextField(new Rect(10, 130, 200, 20), email, 25);
				if (GUI.Button (new Rect (10, 220, 200, 20), "Sign Up")) {
					signUp = true;
					StartCoroutine (SignUp ());
				}	
				if (GUI.Button (new Rect (10, 190, 200, 20), "Cancel")) {
					signUp = false;
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


			if (GUI.Button(new Rect(10,160,200,20),"Chirps From Users I'm Following")){
				StartCoroutine(getFollowingChirps ());
			}
		}
		
		
		
	}
	
}
