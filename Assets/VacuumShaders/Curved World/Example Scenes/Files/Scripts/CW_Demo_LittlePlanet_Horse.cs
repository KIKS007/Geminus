using UnityEngine;
using System.Collections;

namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Little Planet/Horse")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CW_Demo_LittlePlanet_Horse : MonoBehaviour
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Variables                                                                 //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        NavMeshAgent agent;

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed *= Random.Range(1.2f, 0.8f);

            SetDestination();
        }

        void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, agent.destination) < 1)
                SetDestination();
        }

        void OnCollisionEnter(Collision collision)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddForce((transform.forward + Vector3.up) * Random.Range(5.0f, 10.0f), ForceMode.Impulse);


            if (collision.gameObject.name == "Border")
                SetDestination();
        }

        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Custom Functions                                                          //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////

        void SetDestination()
        {
            Vector3 destination = Random.onUnitSphere * 40;
            destination.y = 0.0f;

            agent.SetDestination(destination);
        }
    }
}
