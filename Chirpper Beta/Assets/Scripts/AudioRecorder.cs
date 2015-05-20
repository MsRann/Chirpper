using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class AudioRecorder : MonoBehaviour {

	AudioClip myAudioClip;
	AudioSource audio;

    GameObject menu;
    CreatePost newPost;

    bool isRecording;

    public bool getIsRecording()
    {
        return isRecording;
    }

	// Use this for initialization
	void Start () {

        menu = GameObject.FindGameObjectWithTag("Menu");
        newPost = menu.GetComponent<CreatePost>();

        isRecording = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RecordAudio()
	{
        isRecording = true;
		myAudioClip = Microphone.Start ( null, false, 10, 44100 );
	}

	public void SaveAudio()
	{
        isRecording = false;
		SavWav.Save("myChirp", myAudioClip);
	}

	public void PlayAudio()
	{
        isRecording = false;
		//audio.PlayOneShot(myAudioClip);

        newPost.setTimer(myAudioClip.length);

        audio.clip = myAudioClip;
        audio.Play();
		/*if (!audio.isPlaying)
		{
			audio.Play();
			//playButton.image.overrideSprite = pauseSprite;
		}
		else
		{
			audio.Pause();
			//playButton.image.overrideSprite = null;
		}*/
	}
}
