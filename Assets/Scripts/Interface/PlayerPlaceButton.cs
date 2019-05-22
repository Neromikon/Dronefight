using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPlaceButton : MonoBehaviour
{
	public Button buttonComponent;
	public Text nameText;
	public Image playerControllerIcon;
	public Image trainerControllerIcon;
	public Controller owner;
	public Controller.Team team;
	public Controller trainer;
}
