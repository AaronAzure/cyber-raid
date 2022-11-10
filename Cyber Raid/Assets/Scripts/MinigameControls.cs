using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class MinigameControls : MonoBehaviour
{
	public MinigameManager manager; 

	
	[Space] [Header("Rewired")]
	public int playerId; 
	private Player player; 
	public int modelId;
	public GameObject[] models;
	public GameObject model;


	[Space] [Header("Game Conditions")]
	public bool canMove;


	[Space] [Header("Mechanics")]
	public float moveSpeed = 20;
	public float rotateSpeed = 20;
	private Vector3 calcMoveDir;
	[SerializeField] Rigidbody rb;
	private bool receivingKb;
	[SerializeField] float kbForce=20;


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

		if (modelId < models.Length)
		{
			models[modelId].transform.parent = this.transform;
			model = models[modelId];
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !receivingKb)
			CalculateMoveSpeed();
    }

	void FixedUpdate()
	{
		if (canMove && !receivingKb)
			Move();
	}

	void CalculateMoveSpeed()
	{
		float x = player.GetAxis("Move Horizontal");
		float z = player.GetAxis("Move Vertical");
		calcMoveDir = new Vector3(x, 0 , z);
	}


	void Move()
	{
		if (rb != null)
		{
			// rb.velocity = calcMoveDir;

			if (calcMoveDir == Vector3.zero)
				return;
			Quaternion targetRotation = Quaternion.LookRotation(-calcMoveDir);
			targetRotation = Quaternion.RotateTowards(
					transform.rotation,
					targetRotation,
					720 * Time.fixedDeltaTime);
			rb.MovePosition(rb.position + calcMoveDir * moveSpeed * Time.fixedDeltaTime);
			rb.MoveRotation(targetRotation);
			// var rotation = Quaternion.LookRotation(-calcMoveDir);
			// transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotateSpeed);
		}
	}

	private void OnCollisionEnter(Collision other) 
	{
		if (other.gameObject.CompareTag("Player"))	
		{
			StartCoroutine( ApplyKnockback(other.gameObject.transform, kbForce) );
		}
	}

	IEnumerator ApplyKnockback(Transform opponent, float force)
	{
		receivingKb = true;
        Vector3 direction = (opponent.position - this.transform.position).normalized;
        rb.velocity = new Vector3(-direction.x * force, 0, -direction.y * force);
        
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        receivingKb = false;
	}
}
