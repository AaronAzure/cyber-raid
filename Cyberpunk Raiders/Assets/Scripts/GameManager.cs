using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManager : MonoBehaviour
{
	public int nPlayers;
	public List<int> characterInds;
	public Rewired.InputManager rewInputManager;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
