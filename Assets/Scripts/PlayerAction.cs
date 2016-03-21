using UnityEngine;
using System.Collections;

public static class PlayerAction
{
	public static void ShootBall (Rigidbody ballRigibody, Transform playerPos, float force, float shootHeight)
	{
		Vector3 direction = ballRigibody.transform.position - playerPos.transform.position ;
		direction.y = shootHeight;
		direction.Normalize ();

		ballRigibody.AddForce (direction * force, ForceMode.Impulse);
	}
}
