using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneDescription : MonoBehaviour
{
	public bool localOrRemote; //if remote - you joining the game someone created

	public string localizedSceneName;
	public Sprite preview;
	public List<GameMode> availableModes;
	
	public GameMode GetRandomMode()
	{
		return availableModes[Random.Range(0, availableModes.Count)];
	}

	public bool IsModeEnabled(GameMode.Type modeType)
	{
		return availableModes.Find(mode => mode.type == modeType) != null;
	}
}
