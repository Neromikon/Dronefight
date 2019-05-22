using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSettings : MonoBehaviour
{
	public MainCameraControl mainCameraControl;
	public Spawner[] neutralSpawners;
	public Spawner[] blueSpawners;
	public Spawner[] redSpawners;
	
	void Start ()
	{
		List<Spawner> availableNeutralSpawners = new List<Spawner>(neutralSpawners);
		List<Spawner> availableBlueSpawners = new List<Spawner>(blueSpawners);
		List<Spawner> availableRedSpawners = new List<Spawner>(redSpawners);

		foreach (Controller player in GameSettings.activePlayers)
		{
			Spawner chosenSpawner = null;

			switch (player.team)
			{
				case Controller.Team.NEUTRAL:
				{
					Debug.Assert(availableNeutralSpawners.Count > 0, "Scene " + GameSettings.currentMap.name + " has not enough neutral spawners");
					int randomIndex = Random.Range(0, availableNeutralSpawners.Count);
					chosenSpawner = availableNeutralSpawners[randomIndex];
					availableNeutralSpawners.RemoveAt(randomIndex);
					break;
				}

				case Controller.Team.BLUE:
				{
					Debug.Assert(availableBlueSpawners.Count > 0, "Scene " + GameSettings.currentMap.name + " has not enough blue spawners");
					int randomIndex = Random.Range(0, availableBlueSpawners.Count);
					chosenSpawner = availableBlueSpawners[randomIndex];
					availableBlueSpawners.RemoveAt(randomIndex);
					break;
				}

				case Controller.Team.RED:
				{
					Debug.Assert(availableRedSpawners.Count > 0, "Scene " + GameSettings.currentMap.name + " has not enough red spawners");
					int randomIndex = Random.Range(0, availableRedSpawners.Count);
					chosenSpawner = availableRedSpawners[randomIndex];
					availableRedSpawners.RemoveAt(randomIndex);
					break;
				}
			}

			player.unit = chosenSpawner.Spawn(player.unitPrefab);

			if (player.type == Controller.PlayerType.LOCAL)
			{
				mainCameraControl.AddTarget(player.unit.gameObject);
			}
		}
	}
	
	void Update ()
	{
		
	}
}
