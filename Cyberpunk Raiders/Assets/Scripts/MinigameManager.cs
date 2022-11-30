using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
	[SerializeField] private GameManager gm;
	[SerializeField] private int nPlayers;


	[Space] [Header("Player Related")]
	[SerializeField] private MinigameControls playerObj;
	[SerializeField] private List<MinigameControls> players;
	[SerializeField] private Transform spawnPos;
	[SerializeField] private float spawnRadius=2;


	[Space] [Header("Static")]
	[SerializeField] TextMeshProUGUI timerTxt;


	[Space] [Header("Game Conditions")]
	[SerializeField] bool playerCanMove;
	[SerializeField] float timer=30;



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
		players = new List<MinigameControls>();

		SpawnPlayersInCircle();
    }

	void SpawnPlayersInCircle()
	{
		for ( int i=0 ; i<gm.characterInds.Count ; i++ )
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
            MinigameControls player = Instantiate(playerObj, pos, Quaternion.identity, transform);
            player.playerId = i; 
            player.manager = this;
            player.modelId = gm.characterInds[i]; 

			// Specific game conditions
            player.canMove = playerCanMove; 

			players.Add(player);
        }
	}

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

	private void FixedUpdate() 
	{
		if (timer > 0)
			timer -= Time.fixedDeltaTime;
		else if (timer < 0)
			timer = 0;
		
		timerTxt.text = timer.ToString("F2");

		if (timer == 0)
			StartCoroutine( ReturnToBoard() );
	}

	IEnumerator ReturnToBoard()
	{
		this.enabled = false;
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(gm.boardName);
	}

	// IEnumerator StartMinigame()
	// {
	// 	yield return new WaitForSeconds(1);
	// 	SceneManager.LoadScene(2);
	// }
}
