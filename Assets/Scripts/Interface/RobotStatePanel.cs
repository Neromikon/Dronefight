using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class RobotStatePanel : MonoBehaviour
{
	public enum Menu { STATE, MAIN, LOCAL, GUEST, TRAINER }

	public struct ResourceBar
	{
		public Slider slider;
		public Image icon;
		public Text count;
		public Image fillImage;

		public void Update(ResourceContainer resource)
		{
			slider.value = resource.amount;
			fillImage.gameObject.SetActive(resource.amount > 0);

			if (resource.resource.type == Resource.Type.COUNTABLE)
			{
				count.text = ((int)resource.amount).ToString();
			}
		}

		public void DynamicSetup(Transform panel, int index)
		{
			Transform resourceBar = panel.Find("Resource" + index.ToString());
			
			slider = resourceBar.GetComponent<Slider>();
			icon = Support.GetComponentRecursive<Image>(resourceBar, "Icon");
			count = Support.GetComponentRecursive<Text>(resourceBar, "Count");
			fillImage = Support.GetComponentRecursive<Image>(resourceBar, "Fill");

			Debug.Assert(slider, "Resource bar have no slider");
			Debug.Assert(icon, "Resource bar have no icon");
			Debug.Assert(count, "Resource bar have no count text");
			Debug.Assert(fillImage, "Resource bar have no fill image");
		}
	}

	public GameObject miniMenuMain;
	public GameObject miniMenuLocal;
	public GameObject miniMenuGuest;
	public GameObject miniMenuTrainer;

	public PrivateButton menuMainPause;
	public PrivateButton menuMainOptions;
	public PrivateButton menuMainEnd;

	public PrivateButton menuLocalLeave;

	public PrivateButton menuGuestOptions;
	public PrivateButton menuGuestLeave;

	public Transform activeMenu { get; private set; }
	public PrivateButton menuButton1 { get; private set; }
	public PrivateButton menuButton2 { get; private set; }
	public PrivateButton menuButton3 { get; private set; }
	public PrivateButton menuButton4 { get; private set; }

	private Controller owner;

	public Image avatarImage;
	public Image controllerIcon;
	public ResourceBar resourceBar1;
	public ResourceBar resourceBar2;
	public ResourceBar resourceBar3;
	public ResourceBar resourceBar4;

	private bool initialized = false;

	private bool menuActive = false;

	public bool InMenu { get { return menuActive; } }

	public Menu currentMenu { get; private set; }

    void Start()
    {
		Transform resourcesPanel = transform.Find("Resources");
		resourceBar1.DynamicSetup(resourcesPanel, 1);
		resourceBar2.DynamicSetup(resourcesPanel, 2);
		resourceBar3.DynamicSetup(resourcesPanel, 3);
		resourceBar4.DynamicSetup(resourcesPanel, 4);

		menuMainPause.privateOnClick.AddListener(OnMenuMainPause);
		menuMainOptions.privateOnClick.AddListener(OnMenuMainOptions);
		menuMainEnd.privateOnClick.AddListener(OnMenuMainEnd);
		menuLocalLeave.privateOnClick.AddListener(OnMenuLocalLeave);
		menuGuestOptions.privateOnClick.AddListener(OnMenuGuestOptions);
		menuGuestLeave.privateOnClick.AddListener(OnMenuGuestLeave);

		InactivateMenu();

		initialized = true;

		SwitchMenu(Menu.STATE);
    }
	
    void Update()
    {
		if (!owner) { return; }
		if (!owner.unit) { return; }

		resourceBar1.Update(owner.unit.resource1);
		resourceBar2.Update(owner.unit.resource2);
		resourceBar3.Update(owner.unit.resource3);
		resourceBar4.Update(owner.unit.resource4);
	}

	public void SwitchMenu(Menu menu)
	{
		currentMenu = menu;

		miniMenuMain.SetActive(menu == Menu.MAIN);
		miniMenuLocal.SetActive(menu == Menu.LOCAL);
		miniMenuGuest.SetActive(menu == Menu.GUEST);
		miniMenuTrainer.SetActive(menu == Menu.TRAINER);

		Refresh();
	}

	public void SetOwner(Controller newOwner)
	{
		owner = newOwner;

		Refresh();
	}

	private void Refresh()
	{
		if (!owner) { return; }
		if (!initialized) { return; }

		Debug.Assert(owner.unitPrefab, "Unit prefab is not set for " + owner.name);

		controllerIcon.sprite = owner.visual.icon;
		avatarImage.sprite = owner.unitPrefab.avatar;
		
		resourceBar1.icon.sprite = owner.unitPrefab.resource1.resource.icon;
		resourceBar1.fillImage.color = owner.unitPrefab.resource1.resource.barColor;
		resourceBar1.count.gameObject.SetActive(owner.unitPrefab.resource1.resource.type == Resource.Type.COUNTABLE);
		resourceBar1.slider.maxValue = owner.unitPrefab.resource1.maximum;
		resourceBar1.slider.wholeNumbers = (owner.unitPrefab.resource1.resource.type == Resource.Type.COUNTABLE);

		resourceBar2.icon.sprite = owner.unitPrefab.resource2.resource.icon;
		resourceBar2.fillImage.color = owner.unitPrefab.resource2.resource.barColor;
		resourceBar2.count.gameObject.SetActive(owner.unitPrefab.resource2.resource.type == Resource.Type.COUNTABLE);
		resourceBar2.slider.maxValue = owner.unitPrefab.resource2.maximum;
		resourceBar2.slider.wholeNumbers = (owner.unitPrefab.resource2.resource.type == Resource.Type.COUNTABLE);

		resourceBar3.icon.sprite = owner.unitPrefab.resource3.resource.icon;
		resourceBar3.fillImage.color = owner.unitPrefab.resource3.resource.barColor;
		resourceBar3.count.gameObject.SetActive(owner.unitPrefab.resource3.resource.type == Resource.Type.COUNTABLE);
		resourceBar3.slider.maxValue = owner.unitPrefab.resource3.maximum;
		resourceBar3.slider.wholeNumbers = (owner.unitPrefab.resource3.resource.type == Resource.Type.COUNTABLE);

		resourceBar4.icon.sprite = owner.unitPrefab.resource4.resource.icon;
		resourceBar4.fillImage.color = owner.unitPrefab.resource4.resource.barColor;
		resourceBar4.count.gameObject.SetActive(owner.unitPrefab.resource4.resource.type == Resource.Type.COUNTABLE);
		resourceBar4.slider.maxValue = owner.unitPrefab.resource4.maximum;
		resourceBar4.slider.wholeNumbers = (owner.unitPrefab.resource4.resource.type == Resource.Type.COUNTABLE);
	}

	public void ActivateMenu()
	{
		menuActive = true;

		if (owner == GameSettings.mainPlayer)
		{
			miniMenuMain.SetActive(true);
			activeMenu = miniMenuMain.transform;
			menuButton1 = menuMainPause;
			menuButton2 = menuMainOptions;
			menuButton3 = menuMainEnd;
		}
		else
		{
			if (owner.type == Controller.PlayerType.LOCAL)
			{
				miniMenuLocal.SetActive(true);
				activeMenu = miniMenuLocal.transform;
				menuButton1 = menuLocalLeave;
			}
			else if (owner.type == Controller.PlayerType.REMOTE)
			{
				miniMenuGuest.SetActive(true);
				activeMenu = miniMenuGuest.transform;
				menuButton1 = menuGuestOptions;
				menuButton2 = menuGuestLeave;
			}
		}
	}

	public void InactivateMenu()
	{
		menuActive = false;

		miniMenuMain.SetActive(false);
		miniMenuLocal.SetActive(false);
		miniMenuGuest.SetActive(false);

		activeMenu = null;
		menuButton1 = null;
		menuButton2 = null;
		menuButton3 = null;
		menuButton4 = null;
	}
	
	private void OnMenuMainPause()
	{
		//todo
	}

	private void OnMenuMainOptions()
	{
		//todo
	}

	private void OnMenuMainEnd()
	{
		GameSettings.SwitchToMenuMode();
		//UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
	}

	private void OnMenuLocalLeave()
	{
		//todo
	}

	private void OnMenuGuestOptions()
	{
		//todo
	}

	private void OnMenuGuestLeave()
	{
		//todo
	}
}


//[CustomEditor(typeof(RobotStatePanel))]
//[CanEditMultipleObjects]
//public class RobotStatePanelInspector : Editor
//{
//	SerializedProperty playerPanelProperty;

//	private void OnEnable()
//	{
//		playerPanelProperty = serializedObject.FindProperty("playerPanel");
//	}

//	public override void OnInspectorGUI()
//	{
//		PlaceSelectionButton targetButton = (PlaceSelectionButton)target;

//		targetButton.playerPanel = (PlayerPanel)EditorGUILayout.ObjectField(
//			"Player Panel", targetButton.playerPanel, typeof(PlayerPanel), true);

//		DrawDefaultInspector();
//	}
//}
