using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class AudioRecorder : MonoBehaviour {

	public readonly int MAX_CHIRP_LENGTH = 15;
	public AudioClip myAudioClip;
	AudioSource audio;

    GameObject menuGUI;
    CreatePost newPost;
    MainMenuGUI menu;

    MyNetwork myNetwork;

    public Sprite stopRecordingSprite;

	public Button stopRecordingButton;
	public Button postButton;
	public Button playButton;



    int startedRecording = 0;
	float length;
    bool isRecording;

	public bool getIsRecording()
	{
		return isRecording;
	}

	public void setIsRecording(bool value)
	{
		isRecording = value;
	}

	// Use this for initialization
	void Start () {
        menuGUI = GameObject.FindGameObjectWithTag("Menu");
        newPost = menuGUI.GetComponent<CreatePost>();
        menu = menuGUI.GetComponent<MainMenuGUI>();
		
		myNetwork =  GameObject.FindGameObjectWithTag("Network").GetComponent<MyNetwork>();
		audio = myNetwork.source;
        isRecording = false;
	}

	public void RecordAudio()
	{
        startedRecording++;

        if (startedRecording == 1)
        {
            isRecording = true;
            newPost.resetTimer();
            newPost.setPauseTimer(false);
            myAudioClip = Microphone.Start(null, false, MAX_CHIRP_LENGTH, 44100);
            stopRecordingButton.image.overrideSprite = stopRecordingSprite;
        }
        else if (startedRecording == 2)
        {
			isRecording = false;
            newPost.setPauseTimer(true);
            stopRecordingButton.image.overrideSprite = null;
            startedRecording = 0;

        }
        
	}

	public void PostAudio()
	{
		SavWav.trimClipToLength (ref myAudioClip, newPost.length);
        isRecording = false;
		byte[] toSend = SavWav.Save(myAudioClip);
        StartCoroutine(myNetwork.sendChirp(menu.getChirpTitle(), toSend,(int)Math.Round (newPost.length)));
	}


	public void Update(){

		//disallow users to play or post their chirp until they're done recording
		if (myAudioClip == null || isRecording) {
			playButton.gameObject.SetActive (false);
			postButton.gameObject.SetActive (false);
		
		} else {
			playButton.gameObject.SetActive (true);
			postButton.gameObject.SetActive (true);
		
		}

		//to change record button when max length of chirp achieved
		if (isRecording) {
			stopRecordingButton.image.overrideSprite = stopRecordingSprite;
		}else{
			stopRecordingButton.image.overrideSprite = null;
		}

		//to change play button logo when its done playing
		if (!audio.isPlaying){
			playButton.image.overrideSprite = null;
		}else{
			playButton.image.overrideSprite = stopRecordingSprite;
		}
	}

	public void PlayAudio()
	{

        isRecording = false;
		//audio.PlayOneShot(myAudioClip);

        //newPost.setTimer(myAudioClip.length);

		audio.clip = myAudioClip;

		if (!audio.isPlaying){
			audio.Play();
		}else{
			audio.Stop();
		}

	}
}
