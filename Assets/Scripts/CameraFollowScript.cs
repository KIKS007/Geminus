using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour 
{
	public GameObject sphere;
	public Vector3 player1ToScreen;

	public string following = "Both";

	[Range(0, 2)]
	public int playerFollowed = 0;
	[Space (10)]

	public string followingBothBy = "Unnamed";
	[Range(0, 1)]
	public int zoomOutOrBlock = 0;

	[Space (10)]
	public float cameraLerp = 0.05f;
	public bool cameraLookAt = false;

	public float screenWidthEdge = 5f;
	public float screenHeightEdge;

	private GameObject player1;
	private GameObject player2;

	private Vector3 originalPosition;

	private Vector3 pointBetweenPlayers;

	private Camera camera;

	private Vector3 player1WorldToScreen;
	private Vector3 player2WorldToScreen;

	// Use this for initialization
	void Start () 
	{
		Debug.Log (Screen.height);
		Debug.Log (Screen.width);

		screenWidthEdge = screenWidthEdge / 100 * Screen.width;
		screenHeightEdge = screenHeightEdge / 100 * Screen.height;

		originalPosition = transform.position;

		player1 = GameObject.Find ("Player 1");
		player2 = GameObject.Find ("Player 2");

		camera = GetComponent<Camera>();

	}

	void Update ()
	{

		player1ToScreen = camera.WorldToScreenPoint (player1.transform.position);

	}

	void FixedUpdate () 
	{
		/*switch (playerFollowed)
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
		}*/

		BlockPlayersToScreenEdges ();
	}

	void BlockPlayersToScreenEdges ()
	{
		player1WorldToScreen = camera.WorldToScreenPoint (player1.transform.position);
		player2WorldToScreen = camera.WorldToScreenPoint (player2.transform.position);

		if(player1WorldToScreen.x <= screenWidthEdge)
		{
			player1WorldToScreen.x = screenWidthEdge;

			player1.transform.position = camera.ScreenToWorldPoint(player1WorldToScreen);
		}
		else if(player1WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player1WorldToScreen.x = Screen.width - screenWidthEdge;

			player1.transform.position = camera.ScreenToWorldPoint(player1WorldToScreen);
		}

		if(player2WorldToScreen.x <= screenWidthEdge)
		{
			player2WorldToScreen.x = screenWidthEdge;

			player2.transform.position = camera.ScreenToWorldPoint(player2WorldToScreen);
		}
		else if(player2WorldToScreen.x >= Screen.width - screenWidthEdge)
		{
			player2WorldToScreen.x = Screen.width - screenWidthEdge;

			player2.transform.position = camera.ScreenToWorldPoint(player2WorldToScreen);
		}
			

		if(player1WorldToScreen.y <= screenHeightEdge)
		{
			player1WorldToScreen.y = screenHeightEdge;

			player1.transform.position = camera.ScreenToWorldPoint(player1WorldToScreen);
		}
		else if(player1WorldToScreen.y >= Screen.height - screenHeightEdge)
		{
			player1WorldToScreen.y = Screen.height - screenHeightEdge;

			player1.transform.position = camera.ScreenToWorldPoint(player1WorldToScreen);
		}

		if(player2WorldToScreen.y <= screenHeightEdge)
		{
			player2WorldToScreen.y = screenHeightEdge;

			player2.transform.position = camera.ScreenToWorldPoint(player2WorldToScreen);
		}
		else if(player2WorldToScreen.y >= Screen.height - screenHeightEdge)
		{
			player2WorldToScreen.y = Screen.height - screenHeightEdge;

			player2.transform.position = camera.ScreenToWorldPoint(player2WorldToScreen);
		}

	}

	void CameraFollowBoth ()
	{
		transform.position = Vector3.Lerp (transform.position, FollowBothPosition (), cameraLerp);

		if(cameraLookAt)
			transform.LookAt (Vector3.Lerp (transform.position, pointBetweenPlayers, cameraLerp));
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
		pointBetweenPlayers.y -= 1;

		return pointBetweenPlayers + originalPosition;
	}
}
