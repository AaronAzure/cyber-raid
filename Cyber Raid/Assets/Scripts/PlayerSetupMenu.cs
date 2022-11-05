using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerSetupMenu : MonoBehaviour
{
	private Rewired.Player player;
	public int playerId;


	void Awake() 
	{
		this.gameObject.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

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
