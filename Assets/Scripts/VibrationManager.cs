using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using DG.Tweening;

public class VibrationManager : MonoBehaviour
{
	public float[] playersLeftMotor = new float[4];
	public float[] playersRightMotor = new float[4];

	public bool[] leftMotorVibrating = new bool[4];
	public bool[] rightMotorVibrating = new bool[4];

	void OnLevelWasLoaded ()
	{
		StopVibration ();
	}

	void Start ()
	{
		StopVibration ();

		Vibrate (0, 0.8f, 5, 0, 0, 5, 5);
	}

	void Update ()
	{
		if(leftMotorVibrating [0] == true || rightMotorVibrating [0] == true)
			GamePad.SetVibration (PlayerIndex.One, playersLeftMotor [0], playersRightMotor [0]);

		if(leftMotorVibrating [1] == true || rightMotorVibrating [1] == true)
			GamePad.SetVibration (PlayerIndex.Two, playersLeftMotor [1], playersRightMotor [1]);

		if(leftMotorVibrating [2] == true || rightMotorVibrating [2] == true)
			GamePad.SetVibration (PlayerIndex.Three, playersLeftMotor [2], playersRightMotor [2]);

		if(leftMotorVibrating [3] == true || rightMotorVibrating [3] == true)
			GamePad.SetVibration (PlayerIndex.Four, playersLeftMotor [3], playersRightMotor [3]);
	}
		
	public void Vibrate (int whichPlayer, float leftMotor = 0f, float durationLeftMotor = 0f, float rightMotor = 0f, float durationRightMotor = 0f, float startDuration = 0f, float stopDuration = 0f, Ease easeType = Ease.Linear)
	{
		if(leftMotor != 0)
			StartCoroutine(VibrationLeftMotor (whichPlayer, leftMotor, durationLeftMotor, startDuration, stopDuration, easeType));

		if(rightMotor != 0)
			StartCoroutine(VibrationRightMotor (whichPlayer, rightMotor, durationRightMotor, startDuration, stopDuration, easeType));

	}

	IEnumerator VibrationLeftMotor (int whichPlayer, float leftMotor, float durationLeftMotor, float startDuration, float stopDuration, Ease easeType)
	{
		if(leftMotorVibrating [whichPlayer] == false)
		{
			leftMotorVibrating [whichPlayer] = true;

			Tween myTween = DOTween.To(()=> playersLeftMotor [whichPlayer], x=> playersLeftMotor [whichPlayer] = x, leftMotor, startDuration).SetEase(easeType);

			yield return myTween.WaitForCompletion ();

			yield return new WaitForSeconds (durationLeftMotor);

			myTween = DOTween.To(()=> playersLeftMotor [whichPlayer], x=> playersLeftMotor [whichPlayer] = x, 0, stopDuration).SetEase(easeType);

			yield return myTween.WaitForCompletion ();

			leftMotorVibrating [whichPlayer] = false;
		}

		yield return null;
	}

	IEnumerator VibrationRightMotor (int whichPlayer, float rightMotor, float durationRightMotor, float startDuration, float stopDuration, Ease easeType)
	{
		if(rightMotorVibrating [whichPlayer] == false)
		{
			rightMotorVibrating [whichPlayer] = true;

			Tween myTween = DOTween.To(()=> playersRightMotor [whichPlayer], x=> playersRightMotor [whichPlayer] = x, rightMotor, startDuration).SetEase(easeType);

			yield return myTween.WaitForCompletion ();

			yield return new WaitForSeconds (durationRightMotor);

			myTween = DOTween.To(()=> playersRightMotor [whichPlayer], x=> playersRightMotor [whichPlayer] = x, 0, stopDuration).SetEase(easeType);

			yield return myTween.WaitForCompletion ();

			rightMotorVibrating [whichPlayer] = false;
		}

		yield return null;

	}


	/*IEnumerator VibrationPlayer1 (float leftMotor, float durationLeftMotor, float rightMotor, float durationRightMotor, float startDuration, float stopDuration, Ease easeType)
	{
		player1Vibrating = true;

		Tween myTween;
		Tween myTween2;

		if(leftMotor != 0)
			myTween = DOTween.To(()=> leftMotorPlayer1, x=> leftMotorPlayer1 = x, leftMotor, durationLeftMotor);

		if(rightMotor != 0)
			myTween2 = DOTween.To(()=> rightMotorPlayer1, x=> rightMotorPlayer1 = x, rightMotor, durationRightMotor);



		player1Vibrating = false;
		yield return null;

	}

	IEnumerator VibrationPlayer2 (float leftMotor, float durationLeftMotor, float rightMotor, float durationRightMotor, float startDuration, float stopDuration, Ease easeType)
	{
		player2Vibrating = true;
	
		yield return null;
	}

	IEnumerator VibrationPlayer3 (float leftMotor, float durationLeftMotor, float rightMotor, float durationRightMotor, float startDuration, float stopDuration, Ease easeType)
	{
		player3Vibrating = true;

		yield return null;
	}

	IEnumerator VibrationPlayer4 (float leftMotor, float durationLeftMotor, float rightMotor, float durationRightMotor, float startDuration, float stopDuration, Ease easeType)
	{
		player4Vibrating = true;

		yield return null;
  */

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