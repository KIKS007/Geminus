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

public class Ambient : MonoBehaviour
{
	//---------------------//
	//   static & const    //
	//---------------------//
	
	//---------------------//
	//  member variables   //
	//---------------------//
	public GameObject Stars;
	public GameObject SkyBG;
	public GameObject Clouds;
	public GameObject Moon;

	public bool UseFog = true;
	public Color SkyBGColor;
	public Color FogColor;
	public float FogDensity = 0.0017f;
	public FogMode FogModeValue = FogMode.ExponentialSquared;

	public Color AmbientLightColor;
	public Color CloudsColor;
	public Color HorizonColor;
	public Color HorizonColor2;

	//---------------------//
	//    Init / Dispose   //
	//---------------------//

	//---------------------//
	//       handler       //
	//---------------------//
	void Update ()
	{
		RenderAmbient();
	}

	//---------------------//
	//      get / set      //
	//---------------------//

	//---------------------//
	// private & protected //
	//---------------------//
	
	//---------------------//
	//       public        //
	//---------------------//
	public void InitAmbient()
	{
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

		Stars.transform.GetComponent<Renderer>().sharedMaterial.renderQueue = 1;
		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[0].renderQueue = 2;
		Moon.transform.GetComponent<Renderer>().sharedMaterial.renderQueue = 3;
		Clouds.transform.GetComponent<Renderer>().sharedMaterial.renderQueue = 4;
		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[1].renderQueue = 5;
		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[2].renderQueue = 6;
	}

	public void RenderAmbient()
	{
		RenderSettings.ambientLight = AmbientLightColor;
		
		RenderSettings.fog = UseFog;
		RenderSettings.fogMode = FogModeValue;
		RenderSettings.fogColor = FogColor;
		RenderSettings.fogDensity = FogDensity;

		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[0].color = SkyBGColor;
		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[1].color = HorizonColor;
		SkyBG.transform.GetComponent<Renderer>().sharedMaterials[2].color = HorizonColor2;
		Clouds.transform.GetComponent<Renderer>().sharedMaterial.color = CloudsColor;
	}
}
