using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb2D;

    public float moveSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 movement = new Vector2(Input.GetAxis(InputConsts.HorizontalAxis), Input.GetAxis(InputConsts.VerticalAxis));

        rb2D.velocity = movement * moveSpeed;
	}
}
