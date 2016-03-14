using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using DG.Tweening;

public class VibrationManager : MonoBehaviour, baiser
{
	private float leftMotorPlayer1;
	private float rightMotorPlayer1;

	private float leftMotorPlayer2;
	private float rightMotorPlayer2;

	private float leftMotorPlayer3;
	private float rightMotorPlayer3;

	private float leftMotorPlayer4;
	private float rightMotorPlayer4;

	private bool player1Vibrating;
	private bool player2Vibrating;
	private bool player3Vibrating;
	private bool player4Vibrating;


	void OnLevelWasLoaded ()
	{
		StopVibration ();
	}

	void Start ()
	{
		StopVibration ();
	}

	void Update ()
	{
		if(player1Vibrating)
			GamePad.SetVibration (PlayerIndex.One, leftMotorPlayer1, rightMotorPlayer1);

		if(player2Vibrating)
			GamePad.SetVibration (PlayerIndex.Two, leftMotorPlayer2, rightMotorPlayer2);

		if(player3Vibrating)
			GamePad.SetVibration (PlayerIndex.Three, leftMotorPlayer3, rightMotorPlayer3);

		if(player4Vibrating)
			GamePad.SetVibration (PlayerIndex.Four, leftMotorPlayer4, rightMotorPlayer4);
	}

	public void Vibrate (PlayerIndex whichPlayer, float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration = 5f, Ease easeType = Ease.Linear)
	{
		if(whichPlayer == PlayerIndex.One && !player1Vibrating)
			StartCoroutine(VibrationPlayer1 (leftMotor, rightMotor, duration, startDuration, stopDuration, easeType));

		if(whichPlayer == PlayerIndex.Two && !player2Vibrating)
			StartCoroutine(VibrationPlayer2 (leftMotor, rightMotor, duration, startDuration, stopDuration, easeType));

		if(whichPlayer == PlayerIndex.Three && !player3Vibrating)
			StartCoroutine(VibrationPlayer3 (leftMotor, rightMotor, duration, startDuration, stopDuration, easeType));

		if(whichPlayer == PlayerIndex.Four && !player4Vibrating)
			StartCoroutine(VibrationPlayer4 (leftMotor, rightMotor, duration, startDuration, stopDuration, easeType));
	}

	IEnumerator VibrationPlayer1 (float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration, Ease easeType)
	{
		player1Vibrating = true;


		yield return null;
	}

	IEnumerator VibrationPlayer2 (float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration, Ease easeType)
	{
		player2Vibrating = true;
	
		yield return null;
	}

	IEnumerator VibrationPlayer3 (float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration, Ease easeType)
	{
		player3Vibrating = true;

		yield return null;
	}

	IEnumerator VibrationPlayer4 (float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration, Ease easeType)
	{
		player4Vibrating = true;

		yield return null;
	}

	public void StopVibration ()
	{
		GamePad.SetVibration (PlayerIndex.One, 0, 0);
		GamePad.SetVibration (PlayerIndex.Two, 0, 0);
		GamePad.SetVibration (PlayerIndex.Three, 0, 0);
		GamePad.SetVibration (PlayerIndex.Four, 0, 0);
	}

	void OnApplicationQuit ()
	{
		StopVibration ();
	}
}


public interface baiser{
	void sodomie(int taileducul, string criededouleur);
	float facial(Vector3 velocitysperm, Mesh faceModel);
}