/**
 * This file and its contents are confidential and intended solely for the 
 * use of Amable Rodríguez or outside parties permitted to view this file and its
 * contents per agreement between Amable Rodríguez and said parties.  
 * Unauthorized publication, use, dissemination, forwarding, printing or 
 * copying of this file and its contents is strictly prohibited.
 *
 * Copyright © 2015 Amable Rodríguez | blexbox Interactive. 
 * All Rights Reserved 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class InstantGoodDay : MonoBehaviour
{
	//---------------------//
	//       public        //
	//---------------------//

	/**
	 * value : between 1 and int.MaxValue
	 */
	public void SetDayDurationInSeconds(int value)
	{
		var animationSpeed = IsTimePassEnableEditorProperty ? ANIMATION_DURATION_IN_SECONDS / value : 0;
		SetAnimationSpeed(animationSpeed);
		DayDurationInSecondsEditorProperty = value;
	}
	
	public int GetDayDurationInSeconds()
	{
		return DayDurationInSecondsEditorProperty;
	}

	/**
	 * value : between 0 and 23.99
	 */
	public void SetNumericHour(float value)
	{
		float normalizedTime = value / 24;
		for(int i=0; i<_animatorList.Count; i++)
		{
			SyncAnimation(_animatorList[i], normalizedTime);
		}
		Ambient.GetComponent<Ambient>().RenderAmbient();
	}

	public float GetNumericHour()
	{
		var currentTime = _animatorList[0].GetCurrentAnimatorStateInfo(0).normalizedTime * 24;
		currentTime %= 24;
		return currentTime;
	}

	/**
	 * value : string time in HH:MM format
	 */
	public void SetMilitaryHour(string value)
	{
		SetNumericHour(ConvertTime(value));
	}

	public string GetMilitaryHour()
	{
		return ConvertTime(GetNumericHour());
	}

	public void PassTime()
	{
		InitAnimationSpeed();
	}
	
	public void StopTime()
	{
		SetAnimationSpeed(0);
	}

	public void SetRenderCamera(Camera camera)
	{
		RenderCameraEditorProperty = camera;
		_cameraClippingPlanesFar = -1;
		UpdateCamera();
	}
	
	public Camera GetRenderCamera()
	{
		return RenderCameraEditorProperty;
	}

	public List<GameObject> GetAdditionalDailyAnimationsList()
	{
		return AdditionalDailyAnimations;
	}

	public void SyncDailyAnimations()
	{
		InitAnimations();
		SetNumericHour(GetNumericHour());
	}

	// --------------------------------------------------------------------------------------- //

	//---------------------//
	//   static & const    //
	//---------------------//
	private const float ANIMATION_DURATION_IN_SECONDS = 24f;

	//---------------------//
	//  member variables   //
	//---------------------//
	public GameObject Ambient;

	public Camera RenderCameraEditorProperty;

	public float TimeOfDayEditorProperty = 9f;
	public bool IsTimePassEnableEditorProperty = true;
	public int DayDurationInSecondsEditorProperty = 300;

	public List<GameObject> AdditionalDailyAnimations;

	private float _cameraClippingPlanesFar;
	private List<Animator> _animatorList;

	//---------------------//
	//    Init / Dispose   //
	//---------------------//
	void Awake ()
	{
		if (Ambient == null)
		{
			Debug.LogError("Ambient component not found, prefab seems broken, please remove the current InstantGoodDay prefab and replace it using a new one from the Assets/blexbox/InstantGoodDay directory");
		}
		
		InitCamera();
		InitAnimations();
		InitDayTime();
	}

	void Start()
	{
		Ambient.GetComponent<Ambient>().InitAmbient();
		Ambient.GetComponent<Ambient>().RenderAmbient();
	}

	//---------------------//
	//       handler       //
	//---------------------//
	void Update ()
	{
		if (RenderCameraEditorProperty != null)
		{
			UpdateCamera();
		}
	}
	
	//---------------------//
	//      get / set      //
	//---------------------//
	private void SetAnimationSpeed(float speed)
	{
		for(int i=0; i<_animatorList.Count; i++)
		{
			_animatorList[i].speed = speed;
		}
	}

	//---------------------//
	// private & protected //
	//---------------------//
	private void InitCamera()
	{
		Camera camera = RenderCameraEditorProperty == null ? FindCameraWithName(new string[] {"Main Camera", "MainCamera", "Camera", "Camara", "FirstPersonCharacter"}) : RenderCameraEditorProperty;

		if (camera == null)
		{
			Debug.LogWarning ("Couldn't find a Render Camera automatically, please assign one manually on the editor");
		}
		else 
		{
			if (camera.gameObject.GetComponents<FlareLayer>().Length <= 0)
				camera.gameObject.AddComponent<FlareLayer>();
			camera.clearFlags = CameraClearFlags.SolidColor;
			SetRenderCamera (camera);
		}
	}

	private Camera FindCameraWithName(string[] possibleCameraNames)
	{
		Camera result = null;
		GameObject go;
		for(int i=0; i<possibleCameraNames.Length; i++)
		{
			if (result == null)
			{
				go = GameObject.Find(possibleCameraNames[i]);
				if (go != null && (Camera) go.GetComponent<Camera>() != null)
				{
					result = go.GetComponent<Camera>();
					break;
				}
			}
		}
		return result;
	}

	private void InitAnimations()
	{
		InitAnimatorList();
		InitAnimationSpeed();
	}

	private void InitAnimatorList()
	{
		_animatorList = new List<Animator>();
		_animatorList.Add(Ambient.GetComponent<Animator>());
		for(int i=0; i<AdditionalDailyAnimations.Count; i++)
		{
			Animator animator = AdditionalDailyAnimations[i].GetComponent<Animator>();
			if (animator == null)
			{
				Debug.LogWarning("Animator component not found on AdditionalDailyAnimations list, index "+i);
				continue;
			}
			_animatorList.Add(animator);
		}
	}

	private void InitAnimationSpeed()
	{
		SetDayDurationInSeconds(DayDurationInSecondsEditorProperty);
	}

	private void InitDayTime()
	{
		SetNumericHour(TimeOfDayEditorProperty);
	}

	private void UpdateCamera()
	{
		AdjustScale();
		MoveToCamera();
	}

	private void AdjustScale()
	{
		if (RenderCameraEditorProperty == null)
		{
			return;
		}

		if (_cameraClippingPlanesFar != RenderCameraEditorProperty.farClipPlane)
		{
			_cameraClippingPlanesFar = RenderCameraEditorProperty.farClipPlane;

			var scale = _cameraClippingPlanesFar / 1000;
			transform.localScale = Vector3.one * scale;
		}
	}

	private void MoveToCamera()
	{
		if (RenderCameraEditorProperty == null)
		{
			return;
		}

		var t = RenderCameraEditorProperty.transform;
		if (transform.position.x != t.position.x || transform.position.y != t.position.y || transform.position.z != t.position.z)
		{
			transform.position = new Vector3(t.position.x, t.position.y, t.position.z);
		}
	}

	private string ConvertTime(float value)
	{
		var hour = Mathf.Floor(value);
		var minute = Mathf.Floor((value - hour) * 60);
		return hour.ToString("00") + ":" + minute.ToString("00");
	}
	
	private float ConvertTime(string value)
	{
		char[] separator = {':'};
		var pair = value.Split(separator);
		string hour = pair[0];
		string minute = pair[1];
		return float.Parse(hour) + float.Parse(minute) / 60;
	}

	private void SyncAnimation(Animator animator, float normalizedTime)
	{
		animator.Play(0, -1, normalizedTime);
		animator.Update(Time.deltaTime);
	}
}
