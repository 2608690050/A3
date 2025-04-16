using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 移动和跳跃变量
    private Rigidbody rb;
    private float moveX, moveY;
    private float moveSpeed = 15;
    private int count;
    private Vector3 startPosition;

    // 跳跃参数
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    private float lastJumpTime = -1f;
    public AudioSource jumpAudio;

    // 视角控制参数
    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;

    [Header("UI")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI gameOverText;
    public AudioSource clickAudio;
    public AudioSource waterSplashAudio;

    // Welcome消息相关变量
    [Header("Welcome Message")]
    public TextMeshProUGUI welcomeText;
    public Image backgroundPanel;
    public float messageDuration = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        count = 0;
        SetCountText();
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);

        // 初始化时隐藏Welcome相关UI
        if (welcomeText != null) welcomeText.gameObject.SetActive(false);
        if (backgroundPanel != null) backgroundPanel.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
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
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        rb.AddForce(movement * moveSpeed);
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
        }
        else if (other.gameObject.CompareTag("Sphere"))
        {
            other.gameObject.SetActive(false);
            count = count + 2;
            SetCountText();
            clickAudio.Play();
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
    }

    public void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }
}