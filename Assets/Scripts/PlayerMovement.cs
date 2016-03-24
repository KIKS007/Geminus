using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerMovement : MonoBehaviour 
{
	public int playerId = 0; // The Rewired player id of this character

	[Header ("Movement")]
	public float movementSpeed = 10f;
	public float maxVelocityChange = 10f;
	public float jumpForce = 15f;
	public float groundedRayLength = 0.2f;
	public float gravityForce = 40f;
	public bool physicsMovement;

	private Player player; // The Rewired Player
	private Rigidbody rigidbodyPlayer;
	
	private Vector3 movementVector;

	private float distToGround;


	void Awake ()
	{
		 // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
		player = ReInput.players.GetPlayer(playerId);
		
		rigidbodyPlayer = GetComponent<Rigidbody>();

		distToGround = GetComponent<Collider> ().bounds.extents.y;

	}
	
	// Update is called once per frame
	void Update () 
	{
		GetInput();

		LookForward ();

		Vector3 direction = transform.position;

		Debug.DrawRay(new Vector3(direction.x, direction.y, direction.z), new Vector3(0, -distToGround - groundedRayLength, 0), Color.red);
		Debug.DrawRay(new Vector3(direction.x - 0.3f, direction.y, direction.z), new Vector3(0, -distToGround + 0.1f - groundedRayLength, 0), Color.red);
		Debug.DrawRay(new Vector3(direction.x + 0.3f, direction.y, direction.z), new Vector3(0, -distToGround + 0.1f - groundedRayLength, 0), Color.red);
		Debug.DrawRay(new Vector3(direction.x, direction.y, direction.z - 0.3f), new Vector3(0, -distToGround + 0.1f - groundedRayLength, 0), Color.red);
		Debug.DrawRay(new Vector3(direction.x, direction.y, direction.z + 0.3f), new Vector3(0, -distToGround + 0.1f - groundedRayLength, 0), Color.red);
	}
	
	void FixedUpdate () 
	{
		if (physicsMovement)
			PhysicsMovement ();
		else
			TransformMovement();
	}
	
	void GetInput() 
	{
		movementVector = new Vector3 (player.GetAxis ("Move Horizontal"), 0, player.GetAxis ("Move Vertical"));

		movementVector = movementVector * movementSpeed;

		if(player.GetButton("Jump") && IsGrounded ())
		{
			PlayerJump ();
		}
	}
	
	public bool IsGrounded ()
	{
		Vector3 position = transform.position;

		//return Physics.Raycast (transform.position, -Vector3.up, distToGround + groundedRayLength);


		if(Physics.Raycast (new Vector3(position.x, position.y, position.z), -Vector3.up, distToGround + groundedRayLength))
			return true;
		
		else if(Physics.Raycast (new Vector3(position.x - 0.3f, position.y, position.z), -Vector3.up, distToGround - 0.1f + groundedRayLength))
			return true;
		
		else if(Physics.Raycast (new Vector3(position.x + 0.3f, position.y, position.z), -Vector3.up, distToGround - 0.1f + groundedRayLength))
			return true;
		
		else if(Physics.Raycast (new Vector3(position.x, position.y, position.z - 0.3f), -Vector3.up, distToGround - 0.1f + groundedRayLength))
			return true;
		
		else if(Physics.Raycast (new Vector3(position.x, position.y, position.z + 0.3f), -Vector3.up, distToGround - 0.1f + groundedRayLength))
			return true;

		else
			return false;
	}
	
	void TransformMovement() 
	{
		rigidbodyPlayer.MovePosition (transform.position + movementVector * Time.deltaTime);

		rigidbodyPlayer.AddForce (new Vector3 (0, -gravityForce, 0), ForceMode.Force);
	}

	void PhysicsMovement ()
	{
		// Calculate how fast we should be moving
		movementVector = new Vector3 (player.GetAxis ("Move Horizontal"), 0, player.GetAxis ("Move Vertical"));
		//movementVector = transform.TransformDirection(movementVector);
		movementVector *= movementSpeed;

		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 velocityChange = (movementVector - velocity);

		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		rigidbodyPlayer.AddForce(velocityChange, ForceMode.VelocityChange);

		rigidbodyPlayer.AddForce (new Vector3 (0, -gravityForce, 0), ForceMode.Force);
	}

	void LookForward ()
	{
		if(movementVector != Vector3.zero)
		{
			Quaternion rotation = Quaternion.LookRotation (movementVector);

			transform.rotation = rotation;
		}
	}

	void PlayerJump ()
	{
		rigidbodyPlayer.velocity = new Vector3(rigidbodyPlayer.velocity.x, jumpForce, rigidbodyPlayer.velocity.z);
	}
}
