using UnityEngine;
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

	public bool rightBlocked;
	public bool leftBlocked;
	public bool forwardsBlocked;
	public bool backwardsBlocked;

	[HideInInspector]
	public float bumpForce;
	[HideInInspector]
	public bool bumped = false;
	[HideInInspector]
	public float bumpedDuration;

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
		movementVector = new Vector3 (player.GetAxisRaw ("Move Horizontal"), 0, player.GetAxisRaw ("Move Vertical"));

		movementVector = movementVector.normalized * movementSpeed;

		if (leftBlocked && movementVector.x < 0)
			movementVector.x = 0;

		else if (rightBlocked && movementVector.x > 0)
			movementVector.x = 0;

		else if (backwardsBlocked && movementVector.z < 0)
			movementVector.z = 0;

		else if (forwardsBlocked && movementVector.z > 0)
			movementVector.z = 0;

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
		if(!bumped)
			rigidbodyPlayer.MovePosition (transform.position + movementVector * Time.deltaTime);

		rigidbodyPlayer.AddForce (new Vector3 (0, -gravityForce, 0), ForceMode.Force);
	}
	
	void PlayerJump ()
	{
		rigidbodyPlayer.velocity = new Vector3(rigidbodyPlayer.velocity.x, jumpForce, rigidbodyPlayer.velocity.z);
	}


	public void Bump (BumpedDirection direction)
	{
		if(!bumped)
		{
			Debug.Log ("Bumped");

			bumped = true;

			switch (direction)
			{
			case BumpedDirection.Left:
				rigidbodyPlayer.AddForce (Vector3.left * bumpForce, ForceMode.Impulse);
				break;
			case BumpedDirection.Forward:
				rigidbodyPlayer.AddForce (Vector3.forward * bumpForce, ForceMode.Impulse);
				break;
			case BumpedDirection.Right:
				rigidbodyPlayer.AddForce (Vector3.right * bumpForce, ForceMode.Impulse);
				break;
			case BumpedDirection.Backward:
				rigidbodyPlayer.AddForce (-Vector3.forward * bumpForce, ForceMode.Impulse);
				break;
			}

			StartCoroutine (BumpDuration ());
		}
	}

	IEnumerator BumpDuration ()
	{
		yield return new WaitForSeconds (bumpedDuration);

		bumped = false;
	}
}
