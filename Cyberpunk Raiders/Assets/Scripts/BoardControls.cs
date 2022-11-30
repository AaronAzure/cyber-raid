using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
// using Cinemachine;

public class BoardControls : MonoBehaviour
{
	[Header("Rewired")]
	public int playerId; 
	private Player player; 
	[SerializeField] GameObject playerCam; 
	[SerializeField] Rewired.Integration.UnityUI.RewiredStandaloneInputModule rewiredModule;
	

	[Header("Inventory")]
	[SerializeField] int datas;
	[SerializeField] int keys;
	[SerializeField] TextMeshProUGUI datasTxt;
	[SerializeField] TextMeshProUGUI keysTxt;
	[SerializeField] int energy;



	[Space] [Header("Managers")]
	public BoardManager manager; 
	public GameManager gm;


	[Space] [Header("Paths")]
	public Node currentNode;
	public Node nextNode;
	public NodeMaster nodeMaster;
	public GameObject[] models;
	[Space] public float moveSpeed = 20;
	public float rotateSpeed = 5;
	private float timer = 0;
	public BillBoard moveCounter;
	private int nodeInd;


	[SerializeField] GameObject options;
	[SerializeField] GameObject dataUi;
	[SerializeField] Button defaultButton;
	public Vector3 posOffset;
	public Vector3 rotOffset;


	[Header("Movement")]
	public int maxMovement=6;
	private int moveCount = 0;
	public TextMeshPro moveCounterTxt;
	private bool canMove;
	private bool canMoveAside;
	private Vector3 asidePos;


	[Space] [Header("Paths")]
	private bool viewingMap;
	private GameObject mapCam;
	[Space] public float mapSpeed = 20;
	private GameObject mapCamObj;
	public int test;


	public void SetPlayerConfig(int id, Rewired.InputManager rim, GameObject cam, GameObject camObj)
	{
		playerId = id;
		this.name = "Player " + id;
		if (rewiredModule != null)
		{
			rewiredModule.RewiredPlayerIds = new int[]{id};
			rewiredModule.RewiredInputManager = rim;
		}
		mapCam = cam;
		mapCamObj = camObj;
	}

	public void SetUiData(Transform obj)
	{
		if (dataUi != null)
		{
			dataUi.transform.parent = obj;
			dataUi.transform.localScale = new Vector3(1,1,1);
		}
	}


	void OnEnable() 
	{
		if (nextNode == null && currentNode != null && currentNode.nodes != null && currentNode.nodes.Count > 0)
		{
			nextNode = currentNode.nodes[0];
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		player = ReInput.players.GetPlayer(playerId);
		// DEACTIVATE PLAYER CAM
		if (playerCam != null)
		{
			playerCam.transform.parent = null;
		}
		if (gm != null)
		{
			int ind = gm.characterInds[playerId];
			if (ind < models.Length)
			{
				var obj = Instantiate(models[ind], transform.position + Vector3.up, Quaternion.identity, transform);
			}
		}
		// Find next path
		if (currentNode != null && currentNode.nodes != null && currentNode.nodes.Count > 0)
		{
			nextNode = currentNode.nodes[0];
		}
		this.enabled = false;	// Stop update function
	}

	// Update is called once per frame
	void Update() 
	{
		if (viewingMap && player.GetButtonDown("B"))
			CloseMap();
	}

	void FixedUpdate()
	{
		// VIEWING MAP (PANNING)
		if (viewingMap)
		{
			MoveMap();
		}
		// DONE MOVING, MOVE ASIDE FOR OTHER PLAYERS
		else if (canMoveAside)
		{
			// MOVING
			if (this.transform.position != asidePos) 
			{
				var step = moveSpeed * Time.fixedDeltaTime;
				transform.position = Vector3.MoveTowards(transform.position, asidePos, step);
			}
			// ROTATE TO CAMERA
			else if (transform.rotation.y > 0.01f || transform.rotation.y < -0.01f) 
			{
				var rotation = Quaternion.LookRotation(Vector3.forward);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotateSpeed);
				test++;
			}
			// REACHED
			else
			{
				// Debug.Log(test);
				this.enabled = false;
				canMoveAside = false;
				EndTurn();
			}
		}
		// MOVE TO NEXT NODE
		else if (canMove && currentNode != null && nextNode != null && moveCount > 0)
		{
			// MOVING
			if (this.transform.position != nextNode.transform.position + posOffset) 
			{
				var lookPos = nextNode.transform.position + posOffset - transform.position;
				lookPos.y = 0;
				var rotation = Quaternion.LookRotation(-lookPos);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotateSpeed);

				var step = moveSpeed * Time.fixedDeltaTime;
				this.transform.position = Vector3.MoveTowards(this.transform.position, nextNode.transform.position + posOffset, step);
			}
			// REACHED NEXT NODE
			else
			{
				moveCount--;

				if (moveCounterTxt != null)
				{
					moveCounterTxt.text = moveCount.ToString();

					if (moveCount <= 0)
						moveCounterTxt.gameObject.SetActive(false);
				}
				currentNode = nextNode;

				int nChoices = currentNode.nodes.Count;
				// Split in road (fork)
				if (nChoices > 1)
				{
					nextNode = (currentNode != null && currentNode.nodes != null && nChoices > 0) 
						? currentNode.nodes[Random.Range(0, currentNode.nodes.Count)] : null;
				}
				// Straight path
				else 
				{
					nextNode = (currentNode != null && currentNode.nodes != null && nChoices > 0) 
						? currentNode.nodes[0] : null;
				}

				if (moveCount <= 0)
				{
					asidePos = transform.position + (playerId == 0 ? new Vector3(2.5f,0,0) : new Vector3(-2.5f,0,0));
					canMove = false;
					canMoveAside = true;
				}
			}
		}
	}

	public void YOUR_TURN()
	{
		if (options != null)
			options.SetActive(true);
		else
			Debug.LogError("", this);
		
		if (defaultButton != null)
			defaultButton.Select();

		if (playerCam != null)
			playerCam.SetActive(true);
	}

	public void SAVE_STATE()
	{
		PlayerPrefsElite.SetInt("datas" + playerId, datas);
		PlayerPrefsElite.SetInt("keys" + playerId, keys);
		PlayerPrefsElite.SetInt("energy" + playerId, energy);
		Debug.Log(currentNode.name.ToString() + " : " + nodeMaster.indMap.ContainsKey(currentNode).ToString());
		PlayerPrefsElite.SetInt("nodeInd" + playerId, nodeMaster.indMap[currentNode]);
	}

	public void LOAD_SAVE_STATE()
	{
		if (PlayerPrefsElite.VerifyInt("datas" + playerId))
			datas = PlayerPrefsElite.GetInt("datas" + playerId);
		if (PlayerPrefsElite.VerifyInt("keys" + playerId))
			keys = PlayerPrefsElite.GetInt("keys" + playerId);
		if (PlayerPrefsElite.VerifyInt("energy" + playerId))
			energy = PlayerPrefsElite.GetInt("energy" + playerId);
		if (PlayerPrefsElite.VerifyInt("nodeInd" + playerId))
			nodeInd = PlayerPrefsElite.GetInt("nodeInd" + playerId);
		nextNode = nodeMaster.nodeMap[nodeInd];
		transform.position = nextNode.transform.position + posOffset;
	}

	public void EndTurn()
	{
		options.SetActive(false);
		playerCam.SetActive(false);
		this.enabled = false;
		manager.NextPlayerTurn();
	}

	public void SetMoveCount(int n=-1)
	{
		moveCount = (n != -1 ? n : Random.Range(1,maxMovement + 1));
		if (moveCounterTxt != null)
		{
			moveCounterTxt.gameObject.SetActive(true);
			moveCounterTxt.text = moveCount.ToString();
		}
		if (options != null)
			options.SetActive(false);
		this.enabled = true;
		StartCoroutine( DelayMove() );
	}

	IEnumerator DelayMove()
	{
		canMoveAside = false;
		canMove = false;
		yield return new WaitForSeconds(0.25f);
		canMove = true;
	}

	public void ViewMap()
	{
		viewingMap = this.enabled = true;
		if (mapCam != null)
		{
			mapCam.transform.position = transform.position + new Vector3(0, 25, -30);
			mapCam.SetActive(true);
		}
		options.SetActive(false);
	}

	public void CloseMap()
	{
		viewingMap = this.enabled = false;
		if (mapCam != null)
			mapCam.SetActive(false);
		options.SetActive(true);
	}

	private void MoveMap()
	{
		float x = player.GetAxis("Move Horizontal");
		float z = player.GetAxis("Move Vertical");

		if (mapCam != null)
		{
			mapCam.transform.position = new Vector3(mapCam.transform.position.x + x, mapCam.transform.position.y, mapCam.transform.position.z + z);
		}
	}
}
