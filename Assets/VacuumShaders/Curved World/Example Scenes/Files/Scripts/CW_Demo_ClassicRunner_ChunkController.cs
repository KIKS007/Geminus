using UnityEngine;
using System.Collections;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Classic Runner/Chunk Controller")]

    public class CW_Demo_ClassicRunner_ChunkController : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Variables                                                                 //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        public Transform lastChunk;

        public static CW_Demo_ClassicRunner_ChunkController get;
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        void Start()
        {
            get = this;
        }

        void Update()
        {
            get = this;
        }


        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Custom Functions                                                          //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        public Vector3 GetLastChunkPosition()
        {
            return lastChunk.position;
        }
    }
}