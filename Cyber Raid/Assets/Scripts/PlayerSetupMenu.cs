using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerSetupMenu : MonoBehaviour
{
	private Rewired.Player player;
	[HideInInspector] public int playerId;
	[HideInInspector] public SetupManager manager;
	[SerializeField] bool ready;
	[SerializeField] int characterInd;
	[SerializeField] GameObject selectedUi;
	[SerializeField] GameObject readyUi;


	void Awake() 
	{
		this.gameObject.SetActive(false);
		if (selectedUi != null)
			selectedUi.SetActive(true);
		if (readyUi != null)
			readyUi.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
		if (player.GetButtonDown("A"))
		{
			ready = true;
			if (manager != null)
				manager.SetCharacter(playerId, 0);
			if (selectedUi != null)
				selectedUi.SetActive(false);
			if (readyUi != null)
				readyUi.SetActive(true);
		}
		else if (player.GetButtonDown("B"))
		{
			ready = false;
			if (manager != null)
				manager.DeselectCharacter();
			if (selectedUi != null)
				selectedUi.SetActive(true);
			if (readyUi != null)
				readyUi.SetActive(false);
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
