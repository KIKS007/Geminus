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
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(InstantGoodDay))]
[CanEditMultipleObjects]
public class GoodDayEditor : Editor 
{
	//---------------------//
	//   static & const    //
	//---------------------//
	
	//---------------------//
	//  member variables   //
	//---------------------//
	InstantGoodDay editedScript;
	SerializedProperty RenderCamera;
	SerializedProperty IsTimePassEnable;
	SerializedProperty DayDurationInSeconds;
	SerializedProperty TimeOfDay;
	SerializedProperty DailyAnimList;

	//---------------------//
	//    Init / Dispose   //
	//---------------------//
	
	//---------------------//
	//       handler       //
	//---------------------//
	
	public void OnEnable ()
	{
		editedScript = (InstantGoodDay) target;

		RenderCamera				= serializedObject.FindProperty("RenderCameraEditorProperty");

		TimeOfDay 					= serializedObject.FindProperty("TimeOfDayEditorProperty");
		IsTimePassEnable			= serializedObject.FindProperty("IsTimePassEnableEditorProperty");
		DayDurationInSeconds 		= serializedObject.FindProperty("DayDurationInSecondsEditorProperty");

		DailyAnimList				= serializedObject.FindProperty("AdditionalDailyAnimations");
	}
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		Camera editorGameObjectValue;
		float editorFloatValue;
		int editorIntValue;
		bool editorBoolValue;

		GroupSeparator();

		editorGameObjectValue = (Camera) EditorGUILayout.ObjectField("Render Camera:", RenderCamera.objectReferenceValue, typeof(Camera), true);
		if (RenderCamera.objectReferenceValue != editorGameObjectValue)
		{
			RenderCamera.objectReferenceValue = editorGameObjectValue;
		}

		GroupSeparator();
		
		editorFloatValue = EditorGUILayout.Slider("Time of day:", TimeOfDay.floatValue, 0, 23.99f);
		if (TimeOfDay.floatValue != editorFloatValue)
		{
			TimeOfDay.floatValue = editorFloatValue;
			editedScript.SetNumericHour(editorFloatValue);
		}
		
		editorBoolValue = EditorGUILayout.BeginToggleGroup("Time passes?", IsTimePassEnable.boolValue);
		if (IsTimePassEnable.boolValue != editorBoolValue)
		{
			IsTimePassEnable.boolValue = editorBoolValue;
		}
		
		editorIntValue = EditorGUILayout.IntField ("Day duration in seconds:", DayDurationInSeconds.intValue);
		if (DayDurationInSeconds.intValue != editorIntValue)
		{
			DayDurationInSeconds.intValue = editorIntValue;
		}
		EditorGUILayout.EndToggleGroup();

		GroupSeparator();

		EditorGUILayout.PropertyField(DailyAnimList, true);

		GroupSeparator();

		serializedObject.ApplyModifiedProperties();
	}

	//---------------------//
	//      get / set      //
	//---------------------//
	
	//---------------------//
	// private & protected //
	//---------------------//
	private void GroupSeparator()
	{
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}

	//---------------------//
	//       public        //
	//---------------------//
}