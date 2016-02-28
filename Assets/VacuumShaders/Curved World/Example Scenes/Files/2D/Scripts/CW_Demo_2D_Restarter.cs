using System;
using UnityEngine;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/2D/Restart")]
    public class CW_Demo_2D_Restarter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }
}
