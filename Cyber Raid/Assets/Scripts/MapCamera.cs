using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
	public float moveSpeed = 5;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
			transform.position += new Vector3(moveSpeed,0,0);
        if (Input.GetKey(KeyCode.DownArrow))
			transform.position += new Vector3(-moveSpeed,0,0);
        if (Input.GetKey(KeyCode.RightArrow))
			transform.position += new Vector3(0,0,-moveSpeed);
        if (Input.GetKey(KeyCode.LeftArrow))
			transform.position += new Vector3(0,0,moveSpeed);
    }
}
