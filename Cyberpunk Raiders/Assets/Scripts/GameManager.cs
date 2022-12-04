using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManager : MonoBehaviour
{
	public int nPlayers { get; private set; }
	public List<int> characterInds;
	public Rewired.InputManager rewInputManager;
	public string boardName;
	public bool hasStarted;
	[SerializeField] public string sceneName;


	void Awake() 
	{
		characterInds = new List<int>();
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		characterInds.Add(-1);
		DontDestroyOnLoad(this);
	}

    public void SetNPlayers(int n)
	{
		nPlayers = n;
	}

	public string GetMinigameSceneName()
	{
		return sceneName;
	}
}
