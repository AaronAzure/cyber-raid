using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
	public float moveSpeed = 5;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private bool up;
	[SerializeField] private bool down;
	[SerializeField] private bool right;
	[SerializeField] private bool left;

	private void OnEnable() 
	{
		if (rb != null)
			rb.velocity = Vector3.zero;	
	}

    // Update is called once per frame
    void Update()
    {
		up = Input.GetKey(KeyCode.UpArrow); 
		down = Input.GetKey(KeyCode.DownArrow); 
		right = Input.GetKey(KeyCode.RightArrow); 
		left = Input.GetKey(KeyCode.LeftArrow); 
        // if (Input.GetKey(KeyCode.UpArrow))
		// 	transform.position += new Vector3(moveSpeed,0,0);
        // if (Input.GetKey(KeyCode.DownArrow))
		// 	transform.position += new Vector3(-moveSpeed,0,0);
        // if (Input.GetKey(KeyCode.RightArrow))
		// 	transform.position += new Vector3(0,0,-moveSpeed);
        // if (Input.GetKey(KeyCode.LeftArrow))
		// 	transform.position += new Vector3(0,0,moveSpeed);
    }

	void FixedUpdate() 
	{
		if (up)
			rb.MovePosition(transform.position + new Vector3(moveSpeed,0,0) * Time.fixedDeltaTime);
		if (down)
			rb.MovePosition(transform.position + new Vector3(-moveSpeed,0,0) * Time.fixedDeltaTime);
		if (right)
			rb.MovePosition(transform.position + new Vector3(0,0,-moveSpeed) * Time.fixedDeltaTime);
		if (left)
			rb.MovePosition(transform.position + new Vector3(0,0,moveSpeed) * Time.fixedDeltaTime);
		if (!up && !down && !right && !left)
			rb.velocity = Vector3.zero;
		// Debug.Log(rb.velocity.magnitude);
	}
}
