﻿using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerScript : MonoBehaviour 
{
	public int playerId = 0; // The Rewired player id of this character

	[Header ("Movement")]
	public float movementSpeed;
	public float jumpForce;
	public float gravityForce = 1;
	
	private Player player; // The Rewired Player
	private Rigidbody rigidbodyPlayer;
	
	public Vector3 movementVector;

	private float distToGround;
	
	void Awake ()
	{
		 // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
		player = ReInput.players.GetPlayer(playerId);
		
		rigidbodyPlayer = GetComponent<Rigidbody>();

		distToGround = GetComponent<Collider> ().bounds.extents.y;
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetInput();
	}
	
	void FixedUpdate () 
	{
		PlayerMovement();
	}
	
	void GetInput() 
	{
		movementVector.x = player.GetAxis("Move Horizontal");
		movementVector.y = 0f;
		movementVector.z = player.GetAxis("Move Vertical");

		if(player.GetButton("Jump") && IsGrounded ())
		{
			PlayerJump ();
		}
	}
	
	public bool IsGrounded ()
	{
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.2f);
	}
	
	void PlayerMovement() 
	{
		//rigidbodyPlayer.velocity = movementVector;
		rigidbodyPlayer.MovePosition (transform.position + movementVector * movementSpeed * Time.fixedDeltaTime);

		if(!IsGrounded ())
			rigidbodyPlayer.AddForce (new Vector3 (0, -gravityForce, 0), ForceMode.Force);
	}
	
	void PlayerJump ()
	{
		rigidbodyPlayer.velocity = new Vector3(rigidbodyPlayer.velocity.x, jumpForce, rigidbodyPlayer.velocity.z);
	}
}
