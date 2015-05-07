using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class MainMenuGUI : MonoBehaviour {

	public GameObject loginPanel;
	public GameObject signUpPanel;

	public GameObject meowingSFX;
	public Button playButton;
	public Sprite pauseSprite;

	// Use this for initialization
	void Start () {

		Cursor.visible = true;

		loginPanel.SetActive(false);
		signUpPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayLoginPanel()
	{
		loginPanel.SetActive(true);
	}

	public void DisplaySignUpPanel()
	{
		signUpPanel.SetActive(true);
	}

	public void CancelLogin()
	{
		loginPanel.SetActive(false);
	}

	public void CancelSignUp()
	{
		signUpPanel.SetActive(false);
	}

	public void PlayMeowSFX()
	{
		if (!meowingSFX.GetComponent<AudioSource>().isPlaying)
		{
			meowingSFX.GetComponent<AudioSource>().Play();
			playButton.image.overrideSprite = pauseSprite;
		}
		else
		{
			meowingSFX.GetComponent<AudioSource>().Pause();
			playButton.image.overrideSprite = null;
		}
			
	}
}
