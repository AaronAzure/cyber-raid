using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SetupControls : MonoBehaviour
{
	private Rewired.Player player;
	[HideInInspector] public int playerId;
	[HideInInspector] public SetupManager manager;
	[SerializeField] bool ready;
	bool holdingCancel;
	[SerializeField] int characterInd;
	[SerializeField] GameObject[] characters;
	[SerializeField] GameObject selectedUi;
	[SerializeField] GameObject readyUi;
	float timer;
	[SerializeField] float holdTime=1.5f;
	[SerializeField] Slider slider;


	void Awake() 
	{
		if (selectedUi != null)
			selectedUi.SetActive(true);
		if (readyUi != null)
			readyUi.SetActive(false);
	}

	// void OnEnable() 
	// {
	// 	if (slider != null) slider.value = 0;
	// }

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
		if (slider != null) slider.value = 0;
		timer = 0;

		characterInd = Random.Range(0, characters.Length);
		if (characterInd < characters.Length)
			characters[characterInd].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
		if (playerId == 0 && holdingCancel)
		{
			timer += Time.deltaTime;
			if (slider != null) slider.value = timer / holdTime;
			if (timer >= holdTime)
			{
				holdingCancel = false;
				timer = 0;
				if (slider != null) slider.value = 0;
				if (manager != null)
				{
					manager.PrevAnimStage();
					manager.ToServerUI();
				}
			}
		}


		if (player.GetButtonDown("Start"))
			manager.NextScene();
		else if (!ready && player.GetButtonDown("Left"))
			ChangeCharacter(false);
		else if (!ready && player.GetButtonDown("Right"))
			ChangeCharacter(true);

		else if (player.GetButtonDown("A") && manager != null && !manager.startingGame)
		{
			ready = true;
			if (manager != null)
				manager.SetCharacter(playerId, characterInd);
			if (selectedUi != null)
				selectedUi.SetActive(false);
			if (readyUi != null)
				readyUi.SetActive(true);
		}
		else if (player.GetButtonDown("B") && manager != null && !manager.startingGame)
		{
			if (ready && manager != null)
				manager.DeselectCharacter(playerId);
			ready = false;
			holdingCancel = true;
			if (selectedUi != null)
				selectedUi.SetActive(true);
			if (readyUi != null)
				readyUi.SetActive(false);
		}
		else if (player.GetButtonUp("B"))
		{
			holdingCancel = false;
			if (slider != null) slider.value = 0;
			timer = 0;
		}
        else if (player.GetButtonDown("Left"))
		{
			Debug.Log("(" + playerId + ") Left");
		}
        else if (player.GetButtonDown("Right"))
		{
			Debug.Log("(" + playerId + ") Right");
		}
    }


	void ChangeCharacter(bool increment)
	{
		if (increment)
		{
			characters[characterInd].SetActive(false);
			if ((characterInd + 1) < characters.Length)
				characters[++characterInd].SetActive(true);
			else
			{
				characterInd = 0;
				characters[0].SetActive(true);
			}
		}
		else
		{
			characters[characterInd].SetActive(false);
			if (characterInd > 0)
				characters[--characterInd].SetActive(true);
			else
			{
				characterInd = characters.Length - 1;
				characters[characterInd].SetActive(true);
			}
		}
	}

	// // todo : REWIRED ------------------------------------------------------------

    // // This function will be called when a controller is connected
    // void OnControllerConnected(ControllerStatusChangedEventArgs args) 
	// {
	// 	if (args.controllerId == expectedId && args.controllerType == Rewired.ControllerType.Joystick)
	// 		this.gameObject.SetActive(true);
    // }

	// // This function will be called when a controller is fully disconnected
	// void OnControllerDisconnected(ControllerStatusChangedEventArgs args) 
	// {
	// 	if (args.controllerId == expectedId && args.controllerType == Rewired.ControllerType.Joystick)
	// 		this.gameObject.SetActive(false);
    // }

	// bool CheckControllers()
	// {
	// 	int n = ReInput.controllers.GetControllers(Rewired.ControllerType.Joystick).Length;
	// 	return n > 1;
	// }
}
