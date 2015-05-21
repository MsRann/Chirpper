using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class MainMenuGUI : MonoBehaviour {

	public GameObject loginPanel;
	public GameObject signUpPanel;
	public GameObject loggedInPanel;
	public GameObject postChirpsPanel;
    public GameObject followingPanel;
    public GameObject recentChirpsPanel;

	public InputField newUsername;
	public Text currentUsername;

	public GameObject meowingSFX;
	public Button playButton;
	public Sprite pauseSprite;

	// Use this for initialization
	void Start () {

        Screen.showCursor = true;

		loginPanel.SetActive(false);
		signUpPanel.SetActive(false);
		loggedInPanel.SetActive(false);
		postChirpsPanel.SetActive(false);
        followingPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayRecentChirpsPanel()
    {
        followingPanel.SetActive(false);
        recentChirpsPanel.SetActive(true);
    }

    public void DisplayFollowingPanel()
    {
        followingPanel.SetActive(true);
        recentChirpsPanel.SetActive(false);
    }

	public void DisplayLoginPanel()
	{
		loginPanel.SetActive(true);
	}

	public void DisplaySignUpPanel()
	{
		signUpPanel.SetActive(true);
	}

	public void DisplayLoggedInPanel()
	{
		currentUsername.text = "@" + newUsername.text;
		loggedInPanel.SetActive(true);
		postChirpsPanel.SetActive(true);
		signUpPanel.SetActive(false);
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
