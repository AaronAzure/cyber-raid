using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class BoardController : MonoBehaviour
{
	public int playerId; [Space]
	public Node currentNode;
	public Node nextNode;
	[Space] public float moveSpeed = 10;
	public float rotateSpeed = 5;
	private float timer = 0;
	public Vector3 posOffset;
	public Vector3 rotOffset;
	public GameObject mapCam;


	[Header("Movement")]
	public int maxMovement=6;
	private int moveCount = 0;
	public TextMeshPro moveCounterTxt;
	private bool canMove;
	private bool canMoveAside;
	private Vector3 asidePos;


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
		// Find next path
		if (currentNode != null && currentNode.nodes != null && currentNode.nodes.Count > 0)
		{
			nextNode = currentNode.nodes[0];
		}
		this.enabled = false;	// Stop update function
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// DONE MOVING, MOVE ASIDE FOR OTHER PLAYERS
		if (canMoveAside)
		{
			// MOVING
			if (this.transform.position != asidePos) 
			{
				var step = moveSpeed * Time.fixedDeltaTime;
				this.transform.position = Vector3.MoveTowards(
					this.transform.position, asidePos, step);
			}
			// REACHED
			else
			{
				this.enabled = false;
				canMoveAside = false;
			}
		}
		// MOVE TO NEXT NODE
		else if (canMove && currentNode != null && nextNode != null && moveCount > 0)
		{
			// MOVING
			if (this.transform.position != nextNode.transform.position + posOffset) 
			{
				var lookPos = nextNode.transform.position + posOffset - transform.position;
				lookPos.y = 90;
				var rotation = Quaternion.LookRotation(lookPos);
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

	public void SetMoveCount(int n=-1)
	{
		moveCount = (n != -1 ? n : Random.Range(1,maxMovement + 1));
		if (moveCounterTxt != null)
		{
			moveCounterTxt.gameObject.SetActive(true);
			moveCounterTxt.text = moveCount.ToString();
		}
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
		if (mapCam != null)
		{
			mapCam.transform.position = transform.position + new Vector3(-20, 15, 0);
			mapCam.SetActive(true);
		}
	}

	public void CloseMap()
	{
		if (mapCam != null)
			mapCam.SetActive(false);
	}
}
