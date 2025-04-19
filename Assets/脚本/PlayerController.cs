using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // 添加这个命名空间用于 TextMeshProUGUI

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private TextMeshProUGUI scoreText; // 添加这个 UI 组件引用

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController characterController;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity;
    private float cameraPitch;

    private int score = 0; // 定义得分变量

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 初始化得分文本
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleLookRotation();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleLookRotation()
    {
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        // 水平旋转（角色）
        transform.Rotate(Vector3.up * lookInput.x * rotationSpeed);

        // 垂直旋转（相机）
        cameraPitch -= lookInput.y * rotationSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
    }

    private void HandleJump()
    {
        if (inputActions.Player.Jump.triggered)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 轻微下压力确保贴地
        }

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreObject"))
        {
            score++;
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
            other.gameObject.SetActive(false); // 隐藏得分对象
        }
    }
}