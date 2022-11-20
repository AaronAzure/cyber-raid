using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class FoxControls : MonoBehaviour
{
	[Space] [Header("Rewired")]
	public int playerId; 
	private Player player; 


	[Space] [Header("Mechanics")]
	public float moveSpeed = 20;
	public float rotateSpeed = 20;
	private bool isJumping;
	private bool isFalling;
	private bool isGrounded;
	private bool isCrouched;
	public float jumpHeight = 20;
	private float jumpTimer = 1f;
	public float maxJumpTimer = 1f;
	private Vector3 calcMoveDir;
	private Vector3 calcRotDir;
	private Vector2 axisDir;
	[SerializeField] Collider col;
	[SerializeField] Rigidbody rb;
	[SerializeField] Animator anim;
	[SerializeField] float distToGround;



    // Start is called before the first frame update
    void Start()
    {
		player = ReInput.players.GetPlayer(playerId);
		// distToGround = col.bounds.extents.y;;
    }

    // Update is called once per frame
    void Update()
    { 
		CalculateMoveSpeed();
		IsGrounded();

		if (isJumping) 
		{
			if (player.GetButton("B") && jumpTimer <= maxJumpTimer)
			{
				rb.velocity = Vector3.up * jumpHeight;
				jumpTimer += Time.deltaTime;
			}
			else
			{
				isJumping = false;
				jumpTimer = 0;
			}
		}
		if (isGrounded)
			Jump();
		else
		{
			isFalling = rb.velocity.y < 0;
			anim.SetBool("isFalling", isFalling);
		}

		// Debug.Log(isGrounded);
		if (player.GetButtonDown("ZR") && anim != null)
		{
			isCrouched = true;
			anim.SetBool("isCrouching", true);
			anim.SetFloat("crouchSpeed", 1);
		}
		if (player.GetButtonUp("ZR") && anim != null)
		{
			isCrouched = false;
			anim.SetBool("isCrouching", false);
			anim.SetFloat("crouchSpeed", -1);
		}

    }

	void FixedUpdate()
	{
		Move();
	}

	void CalculateMoveSpeed()
	{
		float x = player.GetAxis("Move Horizontal");
		float z = player.GetAxis("Move Vertical");
		axisDir = new Vector2(x,z);
		calcRotDir = new Vector3(x, 0 , z);
		calcMoveDir = new Vector3(x * moveSpeed, rb.velocity.y , z * moveSpeed);
	}


	void IsGrounded() 
	{
		if (isFalling)
		{
			isFalling = false;
			anim.SetBool("isFalling", isFalling);
		}
		// isGrounded = Physics.CapsuleCast(col.bounds.center, )
		// isGrounded = Physics.BoxCast(col.bounds.center, col.bounds.extents, Vector3.down, 
		// 	Quaternion.identity, distToGround, LayerMask.NameToLayer("Ground"));
		// return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
		isGrounded = Physics.Raycast(transform.position, -transform.up, distToGround);
	}

	void Move()
	{
		if (rb != null)
		{
			rb.velocity = isCrouched ? calcMoveDir * 0.25f : calcMoveDir;
			if (anim != null)
			{
				anim.SetBool("isWalking", axisDir.magnitude > 0);
				anim.SetFloat("moveSpeed", axisDir.magnitude * moveSpeed);
			}
			// Debug.Log(axisDir.magnitude);

			if (calcRotDir == Vector3.zero)
				return;
			Quaternion targetRotation = Quaternion.LookRotation(-calcRotDir);
			targetRotation = Quaternion.RotateTowards(
					transform.rotation,
					targetRotation,
					720 * Time.fixedDeltaTime);
			rb.MoveRotation(targetRotation);
		}
	}

	void Jump()
	{
		if (isGrounded && rb != null && player.GetButtonDown("B"))
		{
			isGrounded = false;
			isJumping = true;
			jumpTimer = 0;
			anim.SetTrigger("jump");
		}
	}
}
