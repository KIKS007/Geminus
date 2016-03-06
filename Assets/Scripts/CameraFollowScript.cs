using UnityEngine;
using System.Collections;

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

	[Header("Blocking Mode")]
	public float screenWidthEdge = 5f;
	public float screenTopEdge = 1.5f;
	public float screenBottomEdge = 18f;

	[Header("Zoom Out Mode")]
	public float cameraLerpZoomOut = 0.05f;
	public float distanceBetweenPlayers;
	public float distanceMinZoomOut = 20;
	public float distanceMaxZoomOut = 40;

	private GameObject player1;
	private GameObject player2;

	private Vector3 originalPosition;
	private Vector3 offsetCamera;

	private Vector3 pointBetweenPlayers;

	private Camera cameraComponent;

	private Vector3 player1WorldToScreen;
	private Vector3 player2WorldToScreen;


	// Use this for initialization
	void Start () 
	{
		Debug.Log (Screen.height);
		Debug.Log (Screen.width);

		screenWidthEdge = screenWidthEdge / 100 * Screen.width;
		screenTopEdge = screenTopEdge / 100 * Screen.height;
		screenBottomEdge = screenBottomEdge / 100 * Screen.height;

		originalPosition = transform.position;
		offsetCamera = originalPosition;

		player1 = GameObject.Find ("Player 1");
		player2 = GameObject.Find ("Player 2");

		cameraComponent = GetComponent<Camera>();

	}

	void Update ()
	{

		player1ToScreen = cameraComponent.WorldToScreenPoint (player1.transform.position);

		distanceBetweenPlayers = Vector3.Distance (player1.transform.position, player2.transform.position);

		pointBetweenTheTwo = pointBetweenPlayers = player1.transform.position + (player2.transform.position - player1.transform.position) / 2;
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
			followingBothBy = "Zooming Out";
			break;
		case 1:
			BlockPlayersToScreenEdges ();
			followingBothBy = "Blocking Them";
			break;
		case 2:
			followingBothBy = "Doing Nothing";
			break;
		}

		
	}

	void BlockPlayersToScreenEdges ()
	{
		player1WorldToScreen = cameraComponent.WorldToScreenPoint (player1.transform.position);
		player2WorldToScreen = cameraComponent.WorldToScreenPoint (player2.transform.position);

		if(player1WorldToScreen.x <= screenWidthEdge)
		{
			player1WorldToScreen.x = screenWidthEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player1WorldToScreen);
	
			player1.transform.position = new Vector3 (temp.x, player1.transform.position.y, player1.transform.position.z);
		}
		else if(player1WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player1WorldToScreen.x = Screen.width - screenWidthEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player1WorldToScreen);

			player1.transform.position = new Vector3 (temp.x, player1.transform.position.y, player1.transform.position.z);
		}

		if(player2WorldToScreen.x <= screenWidthEdge)
		{
			player2WorldToScreen.x = screenWidthEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player2WorldToScreen);

			player2.transform.position = new Vector3 (temp.x, player2.transform.position.y, player2.transform.position.z);
		}
		else if(player2WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player2WorldToScreen.x = Screen.width - screenWidthEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player2WorldToScreen);

			player2.transform.position = new Vector3 (temp.x, player2.transform.position.y, player2.transform.position.z);
		}
			

		if(player1WorldToScreen.y <= screenBottomEdge)
		{
			player1WorldToScreen.y = screenBottomEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player1WorldToScreen);

			player1.transform.position = new Vector3 (player1.transform.position.x, player1.transform.position.y, temp.z);
		}
		else if(player1WorldToScreen.y >= Screen.height - screenTopEdge)
		{
			player1WorldToScreen.y = Screen.height - screenTopEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player1WorldToScreen);

			player1.transform.position = new Vector3 (player1.transform.position.x, player1.transform.position.y, temp.z);
		}

		if(player2WorldToScreen.y <= screenBottomEdge)
		{
			player2WorldToScreen.y = screenBottomEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player2WorldToScreen);

			player2.transform.position = new Vector3 (player2.transform.position.x, player2.transform.position.y, temp.z);
		}
		else if(player2WorldToScreen.y >= Screen.height - screenTopEdge)
		{
			player2WorldToScreen.y = Screen.height - screenTopEdge;

			Vector3 temp = cameraComponent.ScreenToWorldPoint(player2WorldToScreen);

			player2.transform.position = new Vector3 (player2.transform.position.x, player2.transform.position.y, temp.z);
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
			//float offsetY = originalPosition.y + (distanceBetweenPlayers - distanceMinZoomOut) * 0.2f;
			//float offsetZ = originalPosition.z - (distanceBetweenPlayers - distanceMinZoomOut) * 0.2f;

			//float offsetY = distanceBetweenPlayers * 0.37f + 1;
			//float offsetZ = - distanceBetweenPlayers * 0.37f - 1;

			float offsetY = distanceBetweenPlayers * originalPosition.y / distanceMinZoomOut;
			float offsetZ = - distanceBetweenPlayers * - originalPosition.z / distanceMinZoomOut;

			offsetCamera.y = Mathf.Lerp(offsetCamera.y, offsetY, cameraLerpZoomOut);

			offsetCamera.z = Mathf.Lerp(offsetCamera.z, offsetZ, cameraLerpZoomOut);
		}

		/*if(player1WorldToScreen.x <= screenWidthEdge)
		{
			player1WorldToScreen.x = screenWidthEdge;

			Vector3 temp = transform.position;
			temp.y = temp.y * zoomOut;
			temp.z = temp.z * zoomOut;

			offsetCamera.y += 1;
			offsetCamera.z -= 1;
		}
		else if(player1WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player1WorldToScreen.x = Screen.width - screenWidthEdge;

			player1WorldToScreen.x = screenWidthEdge;

			Vector3 temp = transform.position;
			temp.y = temp.y * zoomOut;
			temp.z = temp.z * zoomOut;

			offsetCamera.y += 1;
			offsetCamera.z -= 1;
		}

		if(player2WorldToScreen.x <= screenWidthEdge)
		{
			player2WorldToScreen.x = screenWidthEdge;


		}
		else if(player2WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player2WorldToScreen.x = Screen.width - screenWidthEdge;


		}


		if(player1WorldToScreen.y <= screenBottomEdge)
		{
			player1WorldToScreen.y = screenBottomEdge;


		}
		else if(player1WorldToScreen.y >= Screen.height - screenTopEdge)
		{
			player1WorldToScreen.y = Screen.height - screenTopEdge;


		}

		if(player2WorldToScreen.y <= screenBottomEdge)
		{
			player2WorldToScreen.y = screenBottomEdge;


		}
		else if(player2WorldToScreen.y >= Screen.height - screenTopEdge)
		{
			player2WorldToScreen.y = Screen.height - screenTopEdge;


		}*/

	}

	void CameraFollowBoth ()
	{
		transform.position = Vector3.Lerp (transform.position, FollowBothPosition (), cameraLerp);

		/*if(cameraLookAt)
			transform.LookAt (Vector3.Lerp (transform.position, pointBetweenPlayers, cameraLerp));*/

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
		pointBetweenPlayers = player1.transform.position + (player2.transform.position - player1.transform.position) / 2;
		pointBetweenPlayers.y = 1;
		//pointBetweenPlayers.z = player1.transform.position.z;

		if(player1.transform.position.z < player2.transform.position.z)
			pointBetweenPlayers.z = player1.transform.position.z + (player2.transform.position.z - player1.transform.position.z) * 0.2f;

		else if(player1.transform.position.z > player2.transform.position.z)
			pointBetweenPlayers.z = player2.transform.position.z + (player1.transform.position.z - player2.transform.position.z) * 0.2f;




		return pointBetweenPlayers + offsetCamera;
	}
}
