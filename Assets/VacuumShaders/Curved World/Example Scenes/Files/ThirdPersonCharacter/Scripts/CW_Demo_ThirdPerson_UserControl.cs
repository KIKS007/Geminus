using UnityEngine;


namespace VacuumShaders.CurvedWorld.Demo
{
    [AddComponentMenu("VacuumShaders/Curved World/Demo/Third Person/UserControl")]
    [RequireComponent(typeof(CW_Demo_ThirdPerson_Character))]
    public class CW_Demo_ThirdPerson_UserControl : MonoBehaviour
    {
        private CW_Demo_ThirdPerson_Character m_Character; // A reference to the CW_Demo_ThirdPerson_Character on the object
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        public bool runner;
        bool uiButtonJump;
        Vector2 touchPivot;

        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<CW_Demo_ThirdPerson_Character>();
        }


        private void Update()
        {
            //Get Jump from keyboard
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }

            //Get Jump from touch-screen
            if (uiButtonJump)
            {
                uiButtonJump = false;
                m_Jump = true;
            }
        }

                
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = 0;
            float v = 0;

            //From touch-screen
            if (Input.touchSupported && Input.touchCount > 0)
            {
                Touch currentTouch = Input.touches[0];

                if (currentTouch.phase == TouchPhase.Began)
                    touchPivot = currentTouch.position;

                if (Input.touches[0].phase == TouchPhase.Moved ||
                    Input.touches[0].phase == TouchPhase.Stationary)
                {
                    Vector2 delta = (currentTouch.position - touchPivot).normalized;

                    h = delta.x;
                    v = delta.y;
                }                
            }
            else   //From keyboard
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }

            if (runner)
                v = 1;


            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;


            // pass all parameters to the character control script
            m_Character.Move(m_Move, false, m_Jump);
            m_Jump = false;


            if (runner)
            {
                Vector3 correctPosition = transform.position;
                correctPosition.z = -4;
                transform.position = correctPosition;
            }
        }


        public void UIJumpButtonOn()
        {
            uiButtonJump = true;
        }
    }
}
