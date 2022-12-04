using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewOverlay : MonoBehaviour
{
	[SerializeField] GameManager gm;
	[SerializeField] string sceneName;
	[SerializeField] TransitionCanvas tc;


	[Space] [Header("Preview Related")]
	[SerializeField] PreviewControls previewObj;
	[SerializeField] int nReady;
	[SerializeField] int nPlayers;
	[SerializeField] Transform uiHolder;
	private Coroutine co;
	
	
    // Start is called before the first frame update
    void Start()
    {
		if (GameObject.Find("GAME MANAGER") != null)
		{
        	gm = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
			sceneName = gm.sceneName;
			nPlayers = gm.nPlayers;
			SpawnPlayerUis();
		}
		else
		{
			Debug.LogError("GameManager cannot be found!");
			return;
		}
		if (sceneName != "")
        	SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

	void SpawnPlayerUis()
	{
		for ( int i=0 ; i<gm.characterInds.Count ; i++ )
        {
			if (gm.characterInds[i] == -1)
				continue;
            
            var player = Instantiate(previewObj, Vector3.zero, Quaternion.identity, uiHolder);
            player.playerId = i; 
            player.manager = this;
            // player.modelId = gm.characterInds[i]; 
        }
	}

	public void ReadyUp()
	{
		nReady++;
		if (nReady >= nPlayers && co == null)
			co = StartCoroutine( LoadActualMinigame() );
	}

	IEnumerator LoadActualMinigame()
	{
		tc.ToBlack();
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(sceneName);
	}
}
