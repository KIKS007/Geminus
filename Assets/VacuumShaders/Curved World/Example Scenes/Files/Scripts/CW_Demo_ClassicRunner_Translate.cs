using UnityEngine;
using System.Collections;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Translate")]
    public class CW_Demo_ClassicRunner_Translate : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Variables                                                                 //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        public Vector3 direction;


        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        void Start()
        {

        }

        void Update()
        {
            transform.Translate(direction * Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (transform.position.z < -20)
            {
                if (CW_Demo_ClassicRunner_ChunkController.get != null)
                {
                    //Get last chunk position
                    Vector3 newPos = CW_Demo_ClassicRunner_ChunkController.get.GetLastChunkPosition();
                    //Adjust Z value
                    newPos.z += 10;

                    //Update current chunk position
                    transform.position = newPos;

                    //Make this chunk the last one
                    CW_Demo_ClassicRunner_ChunkController.get.lastChunk = transform;
                }
            }
        }
    }
}
