using UnityEngine;
using System.Collections;

public enum BumpedDirection {Left, Forward, Right, Backward}

public class CameraFollowScript : MonoBehaviour 
{
	public Vector3 player1ToScreen;
	public Vector3 pointBetweenTheTwo;

	[Header("Follow Mode")]
	public string following = "Both";
	[Range(0, 3)]
	public int playerFollowed = 0;
	[Space (10)]

	[Header("Follow Both Mode")]
	public string followingBothBy = "Zooming Out";
	[Range(0, 2)]
	public int zoomOutOrBlock = 0;
	[Space (10)]

	[Header("Camera Settings")]
	public float cameraLerp = 0.05f;
	public bool cameraLookAt = false;

	[Header("Zoom Out Mode")]
	public float cameraLerpZoomOut = 0.05f;
	public float distanceBetweenPlayers;
	public float distanceMinZoomOut = 20;
	public float distanceMaxZoomOut = 40;
	public float zoomOutCoefficient = 0.4f;
	public float minDistanceJoint = 50f;

	[Header ("Blocking Mode")]
	public float minDistanceJoint2 = 20f;


	private GameObject player1;
	private GameObject player2;

	private Vector3 originalPosition;
	private Vector3 offsetCamera;

	private Vector3 pointBetweenPlayers;

	private Camera cameraComponent;

	private Vector3 player1WorldToScreen;
	private Vector3 player2WorldToScreen;

	private PlayerMovement player1Script;
	private PlayerMovement player2Script;

	private SpringJoint playersJoint;

	// Use this for initialization
	void Start () 
	{
		/*Debug.Log (Screen.height);
		Debug.Log (Screen.width);*/

		originalPosition = transform.position;
		offsetCamera = originalPosition;

		player1 = GameObject.Find ("Player 1");
		player2 = GameObject.Find ("Player 2");

		player1Script = player1.GetComponent<PlayerMovement> ();
		player2Script = player2.GetComponent<PlayerMovement> ();

		cameraComponent = GetComponent<Camera>();

		playersJoint = player1.GetComponent<SpringJoint> ();
	}

	void Update ()
	{
		player1ToScreen = cameraComponent.WorldToScreenPoint (player1.transform.position);

		distanceBetweenPlayers = Vector3.Distance (player1.transform.position, player2.transform.position);

		pointBetweenTheTwo = pointBetweenPlayers = player1.transform.position + (player2.transform.position - player1.transform.position) * 0.5f;
	}

	void FixedUpdate () 
	{
		switch (playerFollowed)
		{
		case 0:
			CameraFollowBoth ();
			following = "Both";
			break;
		case 1:
			CameraFollowPlayer1 ();
			following = "Player 1";
			break;
		case 2:
			CameraFollowPlayer2 ();
			following = "Player 2";
			break;
		case 3:
			following = "None";
			break;
		}

		switch (zoomOutOrBlock)
		{
		case 0:
			ZoomOut ();
			playersJoint.maxDistance = minDistanceJoint;
			followingBothBy = "Zooming Out";
			break;
		case 1:
			playersJoint.maxDistance = minDistanceJoint2;
			followingBothBy = "Blocking Them";
			break;
		case 2:
			followingBothBy = "Doing Nothing";
			break;
		}
	}

	void ZoomOut ()
	{
		if(distanceBetweenPlayers < distanceMinZoomOut)
		{
			offsetCamera.y = Mathf.Lerp(offsetCamera.y, originalPosition.y, cameraLerpZoomOut);
			offsetCamera.z = Mathf.Lerp(offsetCamera.z, originalPosition.z, cameraLerpZoomOut);
		}

		else if(distanceBetweenPlayers > distanceMinZoomOut && distanceBetweenPlayers < distanceMaxZoomOut)
		{
			float offsetY = originalPosition.y + ((distanceBetweenPlayers - distanceMinZoomOut) * zoomOutCoefficient);
			float offsetZ = originalPosition.z - ((distanceBetweenPlayers - distanceMinZoomOut) * zoomOutCoefficient);

			offsetCamera.y = Mathf.Lerp(offsetCamera.y, offsetY, cameraLerpZoomOut);

			offsetCamera.z = Mathf.Lerp(offsetCamera.z, offsetZ, cameraLerpZoomOut);

		}
		else if(distanceBetweenPlayers > distanceMaxZoomOut)
		{
			//BlockPlayersToScreenEdgesZoomOut ();
		}
	}

	void CameraFollowBoth ()
	{
		transform.position = Vector3.Lerp (transform.position, FollowBothPosition (), cameraLerp);

		if(cameraLookAt)
			transform.LookAt (pointBetweenPlayers);
	}

	void CameraFollowPlayer1 ()
	{
		Vector3 playerPosition = player1.transform.position;
		playerPosition.y -= 1;

		if(cameraLookAt)
			transform.LookAt (Vector3.Lerp (transform.position, playerPosition, cameraLerp));

		playerPosition = playerPosition + originalPosition;

		transform.position = Vector3.Lerp (transform.position, playerPosition, cameraLerp);
	}

	void CameraFollowPlayer2 ()
	{
		Vector3 playerPosition = player2.transform.position;
		playerPosition.y -= 1;

		if(cameraLookAt)
			transform.LookAt (Vector3.Lerp (transform.position, playerPosition, cameraLerp));

		playerPosition = playerPosition + originalPosition;

		transform.position = Vector3.Lerp (transform.position, playerPosition, cameraLerp);
	}

	public Vector3 FollowBothPosition ()
	{
		pointBetweenPlayers = player1.transform.position + (player2.transform.position - player1.transform.position) * 0.5f;

		pointBetweenPlayers.y = 0;

		// Si Player1 est devant, baser position sur lui
		if(player1.transform.position.z < player2.transform.position.z) 
			pointBetweenPlayers.z = player1.transform.position.z + (player2.transform.position.z - player1.transform.position.z) * 0.2f;

		// Si Player2 est devant, baser position sur lui
		else if(player1.transform.position.z > player2.transform.position.z)
			pointBetweenPlayers.z = player2.transform.position.z + (player1.transform.position.z - player2.transform.position.z) * 0.2f;

		return pointBetweenPlayers + offsetCamera;
	}
}
