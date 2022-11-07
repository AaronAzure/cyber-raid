using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SetupManagerAddOn : MonoBehaviour
{
	private Rewired.Player player;
	public SetupManager manager;
    // Start is called before the first frame update
    void Start()
    {
		if (manager == null)
		{
			Debug.LogError("SetupManager not assigned");
			this.enabled = false;
		}
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetAnyButtonDown())
		{
			manager.NextAnimStage();
			manager.ToServerUI();
			this.enabled = false;
		}
    }
}
