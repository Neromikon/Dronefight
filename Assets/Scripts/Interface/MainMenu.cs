using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public PlayMenu playMenu;
	public OptionsMenu optionsMenu;
	public InfoMenu infoMenu;

	public Button playButton;
	public Button optionsButton;
	public Button infoButton;
	public Button exitButton;

	private void Start()
	{
		playButton.onClick.AddListener(OnPlayClick);
		optionsButton.onClick.AddListener(OnOptionsClick);
		infoButton.onClick.AddListener(OnInfoClick);
		exitButton.onClick.AddListener(OnExitClick);
	}

	private void OnEnable()
	{
		playMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		infoMenu.gameObject.SetActive(false);

		foreach (Controller player in GameSettings.localPlayers)
		{
			player.gameObject.SetActive(false);
		}
	}

	private void OnPlayClick()
	{
		gameObject.SetActive(false);
		playMenu.gameObject.SetActive(true);
	}

	private void OnOptionsClick()
	{
		gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(true);
	}

	private void OnInfoClick()
	{
		gameObject.SetActive(false);
		infoMenu.gameObject.SetActive(true);
	}

	private void OnExitClick()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
	}
}
