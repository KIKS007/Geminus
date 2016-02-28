using UnityEngine;
using System.Collections;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Little Planet/Spheres")]
    [RequireComponent(typeof(Rigidbody))]
    public class CW_Demo_LittlePlanet_Spheres : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Variables                                                                 //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        Vector3 origPosition;

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        void Start()
        {
            //Save original position
            origPosition = transform.position;

            //Push up a bit
            origPosition.y = 5;
        }

        void FixedUpdate()
        {
            //If object goes out of bounds return to origianl position
            if (transform.position.y < -5)
                transform.position = origPosition;
        }
    }
}
