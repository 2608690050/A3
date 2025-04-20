using System.Collections.Generic;
using UnityEngine;

namespace Supercyan.FreeSample
{
    public class SimpleSampleCharacterControl : MonoBehaviour
    {
        private enum ControlMode
        {
            Tank,
            Direct
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        [SerializeField] private CharacterController m_characterController = null;

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;

        private bool m_isGrounded;

        private Vector3 m_velocity;

        private void Awake()
        {
            if (!m_animator) { m_animator = GetComponent<Animator>(); }
            if (!m_characterController) { m_characterController = GetComponent<CharacterController>(); }
        }

        private void Update()
        {
            if (!m_jumpInput && Input.GetKey(KeyCode.Space))
            {
                m_jumpInput = true;
            }

            m_isGrounded = m_characterController.isGrounded; // Directly checking if the character is grounded
            m_animator.SetBool("Grounded", m_isGrounded);
            if (m_isGrounded && m_velocity.y < 0)
            {
                m_velocity.y = -2f; // To stick to the ground if necessary
            }

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded;
            m_jumpInput = false;
        }

        private void TankUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            bool walk = Input.GetKey(KeyCode.LeftShift);

            if (v < 0)
            {
                if (walk) { v *= m_backwardsWalkScale; }
                else { v *= m_backwardRunScale; }
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            // Move character based on forward and right directions
            Vector3 move = transform.forward * m_currentV + transform.right * m_currentH;
            m_characterController.Move(move * m_moveSpeed * Time.deltaTime);

            m_animator.SetFloat("MoveSpeed", m_currentV);

            JumpingAndLanding();
        }

        private void DirectUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = Camera.main.transform;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                m_characterController.Move(m_currentDirection * m_moveSpeed * Time.deltaTime);

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding();
        }

        private void JumpingAndLanding()
        {
            // 检查角色是否在地面上
            if (m_isGrounded)
            {
                // 如果跳跃输入被触发且角色在地面上，执行跳跃
                if (m_jumpInput)
                {
                    m_jumpTimeStamp = Time.time;
                    m_velocity.y = m_jumpForce;  // 跳跃时设置y轴速度
                }

                // 如果角色已经着陆，并且速度过快，可以应用重力
                if (m_velocity.y < 0)
                {
                    m_velocity.y = -2f;  // 保证角色贴地，不会悬浮
                }
            }
            else
            {
                // 如果不在地面上，继续应用重力
                m_velocity.y += Physics.gravity.y * Time.deltaTime;
            }

            // 使用 CharacterController 进行移动，包含 y 轴重力和跳跃
            m_characterController.Move(m_velocity * Time.deltaTime);
        }
    }
}
