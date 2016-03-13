using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using DG.Tweening;

public class VibrationManager : MonoBehaviour
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

	public void Vibrate (PlayerIndex whichPlayer, float leftMotor, float rightMotor, float duration, float startDuration, float stopDuration, Ease easeType)
	{
		
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
