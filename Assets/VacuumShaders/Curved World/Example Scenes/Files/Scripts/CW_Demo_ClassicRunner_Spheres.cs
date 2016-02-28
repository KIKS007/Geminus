using UnityEngine;
using System.Collections;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Sphere")]
    [RequireComponent(typeof(Rigidbody))]
    public class CW_Demo_ClassicRunner_Spheres : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Variables                                                                 //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        public Vector3 direction;
        Rigidbody rigidBody;


        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            rigidBody.MovePosition(transform.position + direction * Time.deltaTime);

            if (transform.position.z < -20 || transform.position.y < -10)
            {
                transform.position = new Vector3(Random.Range(-2.0f, 2.0f), 10, 75);
            }
        }
    }
}
