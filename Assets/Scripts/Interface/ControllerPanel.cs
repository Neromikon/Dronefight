using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPanel : MonoBehaviour
{
	private Controller owner = null;

	public Toggle enableToggle;
	public InputField nameField;
	public Button iconButton;
	public Image iconImage;
	public GameObject keyboardConfiguration;
	public GameObject mouseConfiguration;
	public GameObject gamepadConfiguration;
	public GameObject touchscreenConfiguration;

	public Button layout1Button;
	public Button layout2Button;
	public Button layout3Button;
	public Button layout4Button;
	public Button layout5Button;

	public KeyBind[] keyBindButtons;

	void Start ()
	{
		enableToggle.onValueChanged.AddListener(OnToggleCheck);
		nameField.onValueChanged.AddListener(OnNameEdit);
		iconButton.onClick.AddListener(OnIconClick);

		layout1Button.onClick.AddListener(() => OnLayoutSelect(0));
		layout2Button.onClick.AddListener(() => OnLayoutSelect(1));
		layout3Button.onClick.AddListener(() => OnLayoutSelect(2));
		layout4Button.onClick.AddListener(() => OnLayoutSelect(3));
		layout5Button.onClick.AddListener(() => OnLayoutSelect(4));

		Refresh();
	}
	
	void Update ()
	{
		
	}

	private void Refresh()
	{
		if (!owner) { return; }

		foreach (KeyBind keybind in keyBindButtons)
		{
			keybind.targetPlayer = owner;
		}

		iconImage.sprite = owner.visual.icon;

		enableToggle.isOn = owner.isAvailable;
		enableToggle.interactable = (owner != GameSettings.mainPlayer);

		keyboardConfiguration.SetActive(owner.controlType == Controller.ControlType.KEYBOARD);
		mouseConfiguration.SetActive(owner.controlType == Controller.ControlType.MOUSE);
		gamepadConfiguration.SetActive(owner.controlType == Controller.ControlType.GAMEPAD);
		touchscreenConfiguration.SetActive(owner.controlType == Controller.ControlType.TOUCHSCREEN);

		nameField.text = owner.name;
	}

	public void SetOwner(Controller newOwner)
	{
		owner = newOwner;

		Refresh();
	}

	private void OnToggleCheck(bool newValue)
	{
		owner.isAvailable = newValue;

		GameSettings.Save();
	}

	private void OnNameEdit(string newName)
	{
		//owner.localizedName
	}

	private void OnIconClick()
	{
		GameSettings.TakeVisualScheme(owner.controlType, ref owner.visual);

		Refresh();

		GameSettings.Save();
	}

	private void OnLayoutSelect(int layout)
	{
		owner.layout = layout;
		GameSettings.Save();
	}
}
