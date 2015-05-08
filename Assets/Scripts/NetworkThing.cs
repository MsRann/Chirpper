using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class NetworkThing : MonoBehaviour {
	string url = "https://chirpper.herokuapp.com/";
	string username = "username",password = "",passwordConfirm="",name="name",email="email",cookie = "";
	string chirp = "Chirp Something!",follow="Enter a username to follow!";
	GUIText test;
	bool signUp = false;
	
	void Start () {
		test =  GameObject.Find("Text").GetComponent<GUIText>();
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
	
	IEnumerator showFollowing() {
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
			// show the highscores
			print(download.text);
			test.text = download.text;
		}
	}

	IEnumerator sendChirp() {
		WWWForm form = new WWWForm();
		//	Dictionary<string,string> headers = form.headers;
		//	headers ["COOKIE"] = cookie;
		form.AddField( "username", username );
		form.AddField( "chirp", chirp );
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
	
	IEnumerator Login() {
		WWWForm form = new WWWForm();
		//form.AddBinaryData("binary", new byte[1]);
		form.AddField( "username", username );
		form.AddField( "password", password );		

		WWW download = new WWW( url, form);
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			print( "Error downloading: " + download.error );
		} else {
			test.text = download.text;
			print(download.text);
			
			if(download.responseHeaders.ContainsKey("SET-COOKIE")){
				cookie = download.responseHeaders["SET-COOKIE"];
			}
		}
	}
	
	void OnGUI(){
		//not signed in
		if (cookie == "") {

			username = GUI.TextField (new Rect (10, 10, 200, 20), username, 25);
			password = GUI.PasswordField (new Rect (10, 40, 200, 20), password, '*', 25);

			//default view
			if (!signUp) {
				if (GUI.Button (new Rect (10, 70, 100, 20), "Login")) {
					StartCoroutine (Login ());
				}
				if (GUI.Button (new Rect (10, 100, 100, 20), "Sign Up")) {
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
			chirp = GUI.TextField (new Rect (10, 10, 200, 20), chirp, 25);
			if (GUI.Button(new Rect(10,40,200,20),"Chirp it!")){
				StartCoroutine(sendChirp ());
			}
			follow = GUI.TextField (new Rect (10, 70, 200, 20), follow, 25);
			if (GUI.Button(new Rect(10,100,200,20),"Follow User!")){
				StartCoroutine(followUser ());
			}

			if (GUI.Button(new Rect(10,130,200,20),"Show Who I'm Following")){
				StartCoroutine(showFollowing ());
			}
		}

		

	}
	
}
