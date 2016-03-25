using UnityEngine;
using System.Collections;
using Rewired;
using DG.Tweening;

public class PlayerAction : MonoBehaviour
{
	[Header("Available Actions")]
	public bool shootBallAction;
	public bool pickAndThrowAction;

	[Header ("Shoot Ball")]
	public float distanceToShoot = 1.5f;
	public float shootForce = 10;
	public float shootHeight = 0.5f;

	[Header ("Pick and Throw Objects")]
	public Transform holdPoint;
	public PhysicMaterial noFriction;
	public float throwForce = 1.5f;
	public float throwHeight = 1f;
	public float sphereRadius = 1.5f;

	[Header ("Pick and Throw Fruits")]
	public float throwForce2 = 1.5f;
	public float throwHeight2 = 1f;

	private bool canShoot = true;

	private Player player;

	private Rigidbody rigidbodyPlayer;
	private Rigidbody ball;

	private bool holdingObject = false;

	private GameObject holdMovable;

	private float mass;
	private float drag;
	private float angularDrag;
	private bool useGravity;
	private bool iskinematic;

	void Awake ()
	{
		player = ReInput.players.GetPlayer(GetComponent<PlayerMovement>().playerId);

		rigidbodyPlayer = GetComponent<Rigidbody>();

		ball = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Rigidbody>();
	}

	void Update ()
	{
		if (shootBallAction && canShoot && player.GetButtonDown ("Punch"))
			ShootBall ();

		if (pickAndThrowAction && player.GetButtonDown ("Action"))
		{
			if(!holdingObject)
				Pick ();
			
			else
				Throw ();
		}
			
		//Debug.Log (rigidbodyPlayer.velocity);
	}

	void ShootBall ()
	{
		if(Vector3.Distance(ball.transform.position, transform.position) < distanceToShoot)
		{
			canShoot = false;

			//Debug.Log ("Shoot");

			Vector3 direction = ball.transform.position - transform.position;
			direction.Normalize ();
			direction.y = shootHeight;

			ball.AddForce (direction * shootForce, ForceMode.Impulse);

			canShoot = true;
		}
			
	}

	void Pick ()
	{
		if(!holdingObject)
		{
			//Debug.Log ("Pick");

			Collider[] movables = Physics.OverlapSphere (holdPoint.position, sphereRadius);

			for(int i = 0; i < movables.Length; i++)
			{
				if(movables[i].tag == "Movable")
				{
					if (!holdMovable)
						holdMovable = movables [i].gameObject;

					if(Vector3.Distance(transform.position, movables [i].transform.position) < Vector3.Distance(transform.position, holdMovable.transform.position))
						holdMovable = movables [i].gameObject;
				}
			}

			if (holdMovable)
			{
				holdingObject = true;
				holdMovable.tag = "HoldMovable";

				holdMovable.transform.DOLocalRotate (Vector3.zero, 0.1f);

				while(holdMovable.transform.position != holdPoint.transform.position)
				{
					holdMovable.transform.position = Vector3.Lerp (holdMovable.transform.position, holdPoint.transform.position, 0.5f);
				}

				AddAndRemoveRigibody (false);
				holdMovable.GetComponent<Collider>().material = noFriction;
				holdMovable.transform.SetParent (transform);
			}
		}
	}

	void Throw ()
	{
		holdMovable.transform.SetParent (null);

		AddAndRemoveRigibody (true);
		holdMovable.GetComponent<Collider>().material = null;

		if(rigidbodyPlayer.velocity != Vector3.zero)
		{
			Vector3 direction = rigidbodyPlayer.velocity;
			direction.y = throwHeight;

			holdMovable.GetComponent<Rigidbody> ().AddForce (direction * throwForce, ForceMode.VelocityChange);
		}

		holdMovable.tag = "Movable";
		holdMovable = null;

		holdingObject = false;
	}

	void AddAndRemoveRigibody (bool add)
	{
		if(!add)
		{
			mass = holdMovable.GetComponent<Rigidbody> ().mass;
			drag = holdMovable.GetComponent<Rigidbody> ().drag;
			angularDrag = holdMovable.GetComponent<Rigidbody> ().angularDrag;
			useGravity = holdMovable.GetComponent<Rigidbody> ().useGravity;
			iskinematic = holdMovable.GetComponent<Rigidbody> ().isKinematic;

			Destroy (holdMovable.GetComponent<Rigidbody> ());
		}
		else
		{
			holdMovable.AddComponent<Rigidbody> ();

			holdMovable.GetComponent<Rigidbody> ().mass = mass;
			holdMovable.GetComponent<Rigidbody> ().drag = drag;
			holdMovable.GetComponent<Rigidbody> ().angularDrag = angularDrag;
			holdMovable.GetComponent<Rigidbody> ().useGravity = useGravity;
			holdMovable.GetComponent<Rigidbody> ().isKinematic = iskinematic;
		}
	}

}
