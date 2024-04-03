using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        [SerializeField]
        Transform autoCam;

        [SerializeField]
        float turnSpeed = 60f;

        [SerializeField]
        private Joystick joystick;
        private bool isJoystickTouched;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_Move;
        private bool m_Jump; // the world-relative desired move direction, calculated from the camForward and user input.
        private bool isLooking;

        [SerializeField]
        float crawlSpeed = 5f;
        private float crawlSpeedNow;

        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            bool crouch = Input.GetKey(KeyCode.C);

            // we use world-relative directions in the case of no main camera

            isJoystickTouched = joystick.IsTouched;

            if (isJoystickTouched)
            {
                // Get the joystick input values
                float joystickInputY = joystick.m_VerticalVirtualAxis.GetValue;
                float joystickInputX = joystick.m_HorizontalVirtualAxis.GetValue;

                // Turn the object on the X and Y axes based on the horizontal input
                float xRotation = joystickInputX * turnSpeed * Time.deltaTime;
                float yRotation = joystickInputY * turnSpeed * Time.deltaTime;

                // Apply the rotations
                if (!isLooking)
                {
                    m_Move = joystickInputY * Vector3.forward + joystickInputX * Vector3.right;
                }
                else
                {
                    autoCam.Rotate(-yRotation, xRotation, 0f);
                    autoCam.rotation = Quaternion.Euler(
                        autoCam.rotation.eulerAngles.x,
                        autoCam.rotation.eulerAngles.y,
                        0f
                    );
                }
            }

#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift))
                m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
