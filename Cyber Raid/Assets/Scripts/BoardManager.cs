using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	[SerializeField] private int nPlayers;


	[Header("Player Related")]
	[SerializeField] private BoardControls playerObj;
	[SerializeField] private List<BoardControls> players;
	[SerializeField] private Transform spawnPos;
	[SerializeField] private float spawnRadius=2;


	[Header("Board Related")]
	[SerializeField] private Node startingNode;
	[SerializeField] private GameManager gm;

	
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
		SpawnPlayersInCircle();
    }


	void SpawnPlayersInCircle()
	{
		for ( int i=0 ; i<nPlayers ; i++ )
        {
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
            player.playerId = i; 
            player.name = "Player_" + (i+1); 
            player.manager = this;
            player.gm = this.gm;
            player.currentNode = startingNode;
            // player.sceneName = this.sceneName;

			players.Add(player);
        }
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
