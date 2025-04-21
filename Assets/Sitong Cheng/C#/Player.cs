using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // �������Ʋ���
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;
    private float m_currentV = 0;
    private float m_currentH = 0;
    private readonly float m_interpolation = 10;
    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    // �ƶ�����Ծ����
    private Rigidbody rb;
    private float moveX, moveY;
    [SerializeField] private float baseMoveSpeed = 8f; // ���ͻ�������
    [SerializeField] private float speedBoostMultiplier = 1.8f; // ���ٱ���
    private float currentMoveSpeed;
    private int count;
    private Vector3 startPosition;
    private bool hasSpeedBoost = false; // �Ƿ��ü�������
    private bool hasFoundBook = false; // �Ƿ��ҵ�������

    // ��Ծ����
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    private float lastJumpTime = -1f;
    public AudioSource jumpAudio;

    // �ӽǿ��Ʋ���
    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;

    [Header("UI")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI gameOverText;

    public AudioSource clickAudio;
    public AudioSource waterSplashAudio;
    public AudioSource speedBoostAudio; // ������Ч

    [Header("Drag Coefficient")]
    [SerializeField] private float dragGround = 5f; // ��������
    [SerializeField] private float dragAir = 2f; // ��������

    [Header("Messages")]
    public TextMeshProUGUI welcomeText;
    public Image backgroundPanel;
    public Image backgroundBook;
    public float messageDuration = 5f;

    [Header("Animation Settings")]
    [SerializeField] private float walkAnimSpeed = 0.5f;
    [SerializeField] private float runAnimSpeed = 1.5f;
    private bool isRunning = false;

    [Header("Victory Settings")]
    public AudioSource victoryAudio; // ʤ����Ч
    

    private void Awake()
    {
        if (!m_animator) m_animator = GetComponent<Animator>();
        if (!m_rigidBody) m_rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        count = 0;
        currentMoveSpeed = baseMoveSpeed;
        SetCountText();

        // ��ʼ����������UI
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (welcomeText != null) welcomeText.gameObject.SetActive(false);
        if (backgroundPanel != null) backgroundPanel.gameObject.SetActive(false);
        if (backgroundBook != null) backgroundBook.gameObject.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // �ӽǿ��Ʊ��ֲ���
        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Shift���ټ��
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        float moveMagnitude = new Vector2(moveX, moveY).magnitude;

        // ֻ���ڻ�ü��������Ұ�סShiftʱ�Ž��뱼��״̬
        if (hasSpeedBoost && Keyboard.current.leftShiftKey.isPressed && moveMagnitude > 0.1f)
        {
            currentMoveSpeed = baseMoveSpeed * speedBoostMultiplier;
            isRunning = true;
            m_animator.SetFloat("MoveSpeed", runAnimSpeed);
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
            isRunning = false;
            m_animator.SetFloat("MoveSpeed", walkAnimSpeed * moveMagnitude);
        }

    }

    void FixedUpdate()
    {
        // �����Ƿ��ڵ������ò�ͬ����
        m_rigidBody.drag = IsGrounded() ? dragGround : dragAir;

        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        m_rigidBody.AddForce(movement * currentMoveSpeed);

        // ���¶����ӵ�״̬
        m_animator.SetBool("Grounded", m_isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }


    public void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && Time.time - lastJumpTime >= jumpCooldown)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        lastJumpTime = Time.time;
        m_animator.SetTrigger("Jump"); // ������Ծ����
        if (jumpAudio != null) jumpAudio.Play();
    }

    public void OnMove(InputValue moveValue)
    {
        Vector2 moveVector = moveValue.Get<Vector2>();
        moveX = moveVector.x;
        moveY = moveVector.y;
    }

    void ResetPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER!";
            gameOverText.gameObject.SetActive(true);
            Invoke("HideGameOverText", 2f);
        }

        if (waterSplashAudio != null) waterSplashAudio.Play();
    }

    void HideGameOverText()
    {
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
    }

    void CheckWinCondition()
    {
        if (count >= 20 && hasFoundBook)
        {
            ShowWinMessage();
            PlayVictoryAnimation();
        }
        else
        {
            ShowHintMessage();
        }
    }

    private void PlayVictoryAnimation()
    {
        // ����ʤ������
        m_animator.SetTrigger("Win");

        // ����ʤ����Ч����Ч
        victoryAudio.Play();

        // �������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowWinMessage()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "You Win!";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
        // �����������ʤ��Ч��
    }

    void ShowHintMessage()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "The current score may not be enough for 20, or the music book could not be found. Please continue exploring the map. (The music book may be in the cave)";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }



    void ShowWelcomeMessage()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "I was injured by a shell explosion. Oh my god, my arms and legs cannot move normally, and my ears keep buzzing. What's on the wall? Oh, I remember now. I'm a musician, and here are my performance photos.";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }

    void ShowWelcomeMessage2()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "I can't sink down, I want to find my musical inspiration in this mountain, climb to the top of the mountain, and rediscover the feeling of playing. Even with my disabled body, I still want to continue my music career.";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }

    void ShowWelcomeMessage3()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "Congratulations on finding the treasure trove of scores.";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }

    void ShowWelcomeMessage4()
    {
        hasFoundBook = true;
        if (welcomeText != null)
        {
            welcomeText.text = "I have found the lost sheet music and have retrieved some of my musical memories.";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        if (backgroundBook != null)
        {
            backgroundBook.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }

    void ShowWelcomeMessage5()
    {
        hasFoundBook = true;
        if (welcomeText != null)
        {
            welcomeText.text = "Get acceleration potion, long press Shift to get double speed";
            welcomeText.gameObject.SetActive(true);
        }

        if (backgroundPanel != null)
        {
            backgroundPanel.gameObject.SetActive(true);
        }

        Invoke("HideWelcomeMessage", messageDuration);
    }

    void HideWelcomeMessage()
    {
        if (welcomeText != null) welcomeText.gameObject.SetActive(false);
        if (backgroundPanel != null) backgroundPanel.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            clickAudio.Play();
            m_animator.SetTrigger("Pickup"); // ����ʰȡ����
        }
        else if (other.gameObject.CompareTag("Sphere"))
        {
            other.gameObject.SetActive(false);
            count = count + 2;
            SetCountText();
            clickAudio.Play();
            m_animator.SetTrigger("Pickup"); // ����ʰȡ����
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            ResetPlayer();
        }
        else if (other.gameObject.CompareTag("1"))
        {
            ShowWelcomeMessage();
        }
        else if (other.gameObject.CompareTag("2"))
        {
            ShowWelcomeMessage2();
        }
        else if (other.gameObject.CompareTag("Tre-score"))
        {
            ShowWelcomeMessage3();
            other.gameObject.SetActive(false);
            count = count + 15;
            SetCountText();
            clickAudio.Play();

        }
        else if (other.gameObject.CompareTag("Book"))
        {
            ShowWelcomeMessage4();
            other.gameObject.SetActive(false);
            clickAudio.Play();
            m_animator.SetTrigger("Pickup"); // ����ʰȡ����

        }
        else if (other.gameObject.CompareTag("speed"))
        {
            ShowWelcomeMessage5();
            hasSpeedBoost = true;
            if (speedBoostAudio != null) speedBoostAudio.Play();
            Destroy(other.gameObject); // ʰȡ���Ƴ�
            m_animator.SetTrigger("Pickup"); // ����ʰȡ����
        }
        else if (other.gameObject.CompareTag("end"))
        {
            CheckWinCondition();
        }
    }


    public void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }
}