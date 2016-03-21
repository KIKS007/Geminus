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
	public float gravityForce = 40f;
	public bool physicsMovement;

	[Header ("Shoot Ball")]
	public float shootForce;
	public float distanceToShoot;
	public float shootHeight;

	private Player player; // The Rewired Player
	private Rigidbody rigidbodyPlayer;
	
	private Vector3 movementVector;

	private bool canShoot = true;

	private float distToGround;

	private Rigidbody ball;

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
		ball = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetInput();

		if(canShoot)
			StartCoroutine (ShootBall ());
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
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.2f);
	}
	
	void TransformMovement() 
	{
		if(!bumped)
			rigidbodyPlayer.MovePosition (transform.position + movementVector * Time.deltaTime);

		rigidbodyPlayer.AddForce (new Vector3 (0, -gravityForce, 0), ForceMode.Force);
	}

	void PhysicsMovement ()
	{
		// Calculate how fast we should be moving
		Vector3 targetVelocity = new Vector3 (player.GetAxis ("Move Horizontal"), 0, player.GetAxis ("Move Vertical"));
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= movementSpeed;

		// Apply a force that attempts to reach our target velocity
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		Vector3 velocityChange = (targetVelocity - velocity);

		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		rigidbodyPlayer.AddForce(velocityChange, ForceMode.VelocityChange);

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

	IEnumerator ShootBall ()
	{
		if (player.GetButton ("Action"))
		{
			if(Vector3.Distance(ball.transform.position, transform.position) < distanceToShoot)
			{
				canShoot = false;

				PlayerAction.ShootBall (ball, transform, shootForce, shootHeight);
				Debug.Log ("Shoot");

				yield return new WaitForSeconds (0.5f);

				canShoot = true;
			}
		}

		yield return null;
	}
}
