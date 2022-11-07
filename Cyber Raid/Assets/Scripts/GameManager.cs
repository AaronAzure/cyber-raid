using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int nPlayers;
	public List<int> characterInds;

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
