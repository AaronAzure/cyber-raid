using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SetupManager : MonoBehaviour
{
	public PlayerSetupMenu[] players;


	void Awake() 
	{
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0 ; i<players.Length ; i++)
			players[i].playerId = i;
    }

	// todo : REWIRED ------------------------------------------------------------

    // This function will be called when a controller is connected
    void OnControllerConnected(ControllerStatusChangedEventArgs args) 
	{
		Debug.Log("OnControllerConnected = " + args.controllerId + "(" + args.controllerType + ")");
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
			players[args.controllerId].gameObject.SetActive(true);
    }

	// This function will be called when a controller is fully disconnected
	void OnControllerDisconnected(ControllerStatusChangedEventArgs args) 
	{
		Debug.Log("OnControllerDisconnected = " + args.controllerId + "(" + args.controllerType + ")");
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
			players[args.controllerId].gameObject.SetActive(false);
    }
}
