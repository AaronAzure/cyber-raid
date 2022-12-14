using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
	[SerializeField] private GameManager gm;
	[SerializeField] private int nPlayers;
	[SerializeField] private int gameNumber;


	[Header("Player Related")]
	[SerializeField] private BoardControls playerObj;
	[SerializeField] private List<BoardControls> players;
	[SerializeField] private Transform spawnPos;
	[SerializeField] private float spawnRadius=2;
	[SerializeField] private GameObject mapCam;
	[SerializeField] private GameObject mapCamObj;


	[Header("Ui Related")]
	[SerializeField] private Transform mainCanvas;
	[SerializeField] private TransitionCanvas tc;


	[Header("Board Related")]
	[SerializeField] private Node startingNode;
	[SerializeField] private NodeMaster nodeMaster;
	[SerializeField] private Transform mainCam;
	[SerializeField] private List<int> playerOrder;
	private int nextPlayer;
	[SerializeField] private int turnNumber=1;
	private int maxTurns=15;


	[Header("Boss Related")]
	[SerializeField] List<Boss> bosses;
	

	
    // Start is called before the first frame update
    void Start()
    {
		if (GameObject.Find("GAME MANAGER") != null)
        	gm = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
		else
		{
			Debug.LogError("GameManager cannot be found!");
			return;
		}
		nPlayers = gm.nPlayers;
		players = new List<BoardControls>();
		playerOrder = new List<int>();

		SpawnPlayersInCircle();
		NextPlayerTurn();

		if (!gm.hasStarted)
		{
			gm.boardName = SceneManager.GetActiveScene().name;
			gm.hasStarted = true;
		}
		else 
		{
			if (PlayerPrefsElite.VerifyInt("turnNumber"))
				turnNumber = PlayerPrefsElite.GetInt("turnNumber");
			foreach (BoardControls player in players)
				player.LOAD_SAVE_STATE();
		}
    }


	void SpawnPlayersInCircle()
	{
		for ( int i=0 ; i<nPlayers ; i++ )
        {
			if (gm.characterInds[i] == -1)
				continue;
				
            /* Distance around the circle */  
            var radians = 2 * Mathf.PI / nPlayers * i;
            
            /* Get the vector direction */ 
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            
            var spawnDir = new Vector3 (-horizontal, vertical);
            
            /* Get the spawn position */ 
            Vector3 pos = spawnPos.position + (spawnDir * spawnRadius); // Radius is just the distance away from the point
            
            /* Now spawn */
            BoardControls player = Instantiate(playerObj, pos, Quaternion.identity, transform);
            // player.transform.parent = instances.transform;
            // player.transform.localScale *= ratio;
            player.SetPlayerConfig(i, gm.rewInputManager, mapCam, mapCamObj); 
            player.manager = this;
            player.gm = this.gm;
            player.nextNode = startingNode;
            player.nodeMaster = nodeMaster;
			player.moveCounter.cam = this.mainCam;
            // player.sceneName = this.sceneName;

			if (mainCanvas != null)
				player.SetUiData(mainCanvas);

			players.Add(player);
			playerOrder.Add(i);
        }
	}


	public void NextPlayerTurn()
	{
		// NEXT PLAYER'S TURN
		if (nextPlayer < playerOrder.Count)
			players[ playerOrder[nextPlayer++] ].YOUR_TURN();
		// EVERYONE HAS THEIR TURN
		else
		{
			SavePlayerStates();
			StartCoroutine( StartMinigame() );
		}
	}

	void SavePlayerStates()
	{
		PlayerPrefsElite.SetInt("turnNumber", ++turnNumber);

		foreach (BoardControls player in players)
			player.SAVE_STATE();
	}

	
	IEnumerator StartMinigame()
	{
		Debug.Log("MINIGAME!!");
		if (tc != null)
			tc.ToBlack();
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("2Preview");
	}
}
