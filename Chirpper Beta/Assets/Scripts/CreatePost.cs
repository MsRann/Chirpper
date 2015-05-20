using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class CreatePost : MonoBehaviour {

    GameObject menu;
    AudioRecorder recorder;

    public Text timerText;
    float timer;

    public void setTimer(float time)
    {
        timer = time;
    }

	// Use this for initialization
	void Start () {

        timerText = timerText.GetComponent<Text>();
        menu = GameObject.FindGameObjectWithTag("Menu");
        recorder = menu.GetComponent<AudioRecorder>();
        timer = 0;

	}
	
	// Update is called once per frame
	void Update () {

        if (recorder.getIsRecording())
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = niceTime;
	}
}
