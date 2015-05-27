using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class CreatePost : MonoBehaviour {

    GameObject menu;
    AudioRecorder recorder;

    public Text timerText;
    public float timer;
    bool pauseTimer = false;
	float accurateTime;
	public float length;

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

    public void setPauseTimer(bool value)
    {
        pauseTimer = value;
		if (value) {
			length = timer;
			SavWav.trimClipToLength (ref recorder.myAudioClip, length);
		}
    }

    public void resetTimer()
    {
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (recorder.getIsRecording())
        {
            if (!pauseTimer){
                timer += Time.deltaTime;

				if(timer >= recorder.MAX_CHIRP_LENGTH){
					recorder.setIsRecording(false);
					setPauseTimer (true);

				}
			}

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
