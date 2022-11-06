using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class SetupManager : MonoBehaviour
{
	public PlayerSetupMenu[] players;
	public List<int> characterInds;
	[SerializeField] private int nConnected;
	[SerializeField] private int nReady;
	private bool startingGame;
	[SerializeField] Animator introAnim;


	void Awake() 
	{
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
		characterInds = new List<int>();
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
		characterInds.Add(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0 ; i<players.Length ; i++)
		{
			players[i].playerId = i;
			players[i].manager = this;
		}
    }

	public void NextAnimStage()
	{
		if (introAnim != null)
		{
			introAnim.ResetTrigger("prev");
			introAnim.SetTrigger("next");
		}
	}

	public void PrevAnimStage()
	{
		if (introAnim != null)
		{
			introAnim.ResetTrigger("next");
			introAnim.SetTrigger("prev");
		}
	}

	public void SetCharacter(int id, int x)
	{
		if (id < characterInds.Count)
		{
			nReady++;
			characterInds[id] = x;
		}	
	}
	public void DeselectCharacter()
	{
		nReady--;
	}

	public void NextScene()
	{
		if (nReady == nConnected)
		{
			startingGame = true;
			StartCoroutine( LoadingNextScene() );
		}
	}

	IEnumerator LoadingNextScene()
	{
		yield return new WaitForEndOfFrame();
		// yield return new WaitForSeconds(0);
		SceneManager.LoadScene(1);
	}


	// todo : REWIRED ------------------------------------------------------------

    // This function will be called when a controller is connected
    void OnControllerConnected(ControllerStatusChangedEventArgs args) 
	{
		if (startingGame)
			return;
		Debug.Log("OnControllerConnected = " + args.controllerId + "(" + args.controllerType + ")");
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
		{
			nConnected++;
			players[args.controllerId].gameObject.SetActive(true);
		}
    }

	// This function will be called when a controller is fully disconnected
	void OnControllerDisconnected(ControllerStatusChangedEventArgs args) 
	{
		if (startingGame)
			return;
		Debug.Log("OnControllerDisconnected = " + args.controllerId + "(" + args.controllerType + ")");
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
		{
			nConnected--;
			players[args.controllerId].gameObject.SetActive(false);
		}
    }
}
