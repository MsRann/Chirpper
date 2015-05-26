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

    GameObject menuGUI;
    CreatePost newPost;
    MainMenuGUI menu;

    GameObject networkObject;
    MyNetwork myNetwork;

    public Sprite stopRecordingSprite;
    public Button stopRecordingButton;

    int startedRecording = 0;

    bool isRecording;

    public bool getIsRecording()
    {
        return isRecording;
    }

	// Use this for initialization
	void Start () {

        menuGUI = GameObject.FindGameObjectWithTag("Menu");
        newPost = menuGUI.GetComponent<CreatePost>();
        menu = menuGUI.GetComponent<MainMenuGUI>();

        networkObject = GameObject.FindGameObjectWithTag("Network");
        myNetwork = networkObject.GetComponent<MyNetwork>();

        isRecording = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RecordAudio()
	{
        startedRecording++;

        if (startedRecording == 1)
        {
            isRecording = true;
            newPost.resetTimer();
            newPost.setPauseTimer(false);
            myAudioClip = Microphone.Start(null, false, 10, 44100);
            stopRecordingButton.image.overrideSprite = stopRecordingSprite;
        }
        else if (startedRecording == 2)
        {
            newPost.setPauseTimer(true);
            stopRecordingButton.image.overrideSprite = null;
            startedRecording = 0;
        }
        
	}

	public void PostAudio()
	{
        isRecording = false;
		byte[] toSend = SavWav.Save(myAudioClip);

  //      float[] samples = new float[myAudioClip.samples * myAudioClip.channels];
//        myAudioClip.GetData(samples, 0);
//        // create a byte array and copy the floats into it...
//        byte[] byteArray = new byte[samples.Length * 4];
//        Buffer.BlockCopy(samples, 0, byteArray, 0, byteArray.Length);

        StartCoroutine(myNetwork.sendChirp(menu.getChirpTitle(), toSend));
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
