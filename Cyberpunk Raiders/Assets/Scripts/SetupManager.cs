using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class SetupManager : MonoBehaviour
{
	public SetupControls[] players;
	public List<int> characterInds;

	[Space] public GameManager gm; [Space]
	[SerializeField] private int nConnected;
	[SerializeField] private int nReady;
	private bool initGame;
	public bool startingGame;
	[SerializeField] Animator introAnim;
	public SetupManagerAddOn managerAddOn;
	[SerializeField] GameObject titleScreenUi;
	[SerializeField] GameObject serverUi;
	[SerializeField] GameObject characterUi;
	[SerializeField] GameObject startUi;


	[Space] [SerializeField] string nextScene;


	void Awake() 
	{
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
		characterInds = new List<int>();
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
    }

    // Start is called before the first frame update
    void Start()
    {
		ToTitleUI();
        for (int i=0 ; i<players.Length ; i++)
		{
			players[i].playerId = i;
			players[i].manager = this;
		}
    }

	public void ToTitleUI()
	{
		if (titleScreenUi != null) titleScreenUi.SetActive(true);
		if (serverUi != null) serverUi.SetActive(false);
		if (characterUi != null) characterUi.SetActive(false);
		if (managerAddOn != null) managerAddOn.enabled = true;
	}
	public void ToServerUI()
	{
		if (titleScreenUi != null) titleScreenUi.SetActive(false);
		if (serverUi != null) serverUi.SetActive(true);
		if (characterUi != null) characterUi.SetActive(false);
	}
	public void ToCharacterUI()
	{
		if (titleScreenUi != null) titleScreenUi.SetActive(false);
		if (serverUi != null) serverUi.SetActive(false);
		if (characterUi != null) characterUi.SetActive(true);
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

	public void SetCharacter(int id, int ind)
	{
		if (id < characterInds.Count)
		{
			nReady++;
			characterInds[id] = ind;
			// if (gm != null && id < gm.characterInds.Count)
			// 	gm.characterInds[id] = ind;
			if (nReady == nConnected && startUi != null)
				startUi.SetActive(true);
		}	
	}
	public void DeselectCharacter(int id)
	{
		nReady--;
		if (startUi != null)
			startUi.SetActive(false);
		if (id < characterInds.Count)
			characterInds[id] = -1;
	}

	public void NextScene()
	{
		if (nReady == nConnected)
		{
			if (gm != null)
			{
				gm.nPlayers = nConnected;
				for (int i=0 ; i<characterInds.Count ; i++)
					if (characterInds[i] < gm.characterInds.Count)
						gm.characterInds[ i ] = characterInds[i];
			}

			startingGame = true;
			NextAnimStage();
			StartCoroutine( LoadingNextScene() );
		}
	}

	IEnumerator LoadingNextScene()
	{
		yield return new WaitForSeconds(0.5f);
		if (titleScreenUi != null) titleScreenUi.SetActive(false);
		if (serverUi != null) serverUi.SetActive(false);
		if (characterUi != null) characterUi.SetActive(false);

		// yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(nextScene);
	}


	// todo : REWIRED ------------------------------------------------------------

    // This function will be called when a controller is connected
    void OnControllerConnected(ControllerStatusChangedEventArgs args) 
	{
		if (startingGame)
			return;
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
		{
			Debug.Log("OnControllerConnected = " + args.controllerId + "(" + args.controllerType + ")");
			nConnected++;
			players[args.controllerId].gameObject.SetActive(true);
		}
    }

	// This function will be called when a controller is fully disconnected
	void OnControllerDisconnected(ControllerStatusChangedEventArgs args) 
	{
		if (startingGame)
			return;
		if (args.controllerId < players.Length && args.controllerType == Rewired.ControllerType.Joystick)
		{
			Debug.Log("OnControllerDisconnected = " + args.controllerId + "(" + args.controllerType + ")");
			nConnected--;
			players[args.controllerId].gameObject.SetActive(false);
		}
    }
}
