using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 移动和跳跃变量
    private Rigidbody rb;
    private float moveX, moveY;
    [SerializeField] private float baseMoveSpeed = 8f; // 降低基础移速
    [SerializeField] private float speedBoostMultiplier = 1.8f; // 加速倍数
    private float currentMoveSpeed;
    private int count;
    private Vector3 startPosition;
    private bool hasSpeedBoost = false; // 是否获得加速能力
    private bool hasFoundBook = false; // 是否找到乐谱书

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
    public AudioSource speedBoostAudio; // 加速音效

    [Header("Drag Coefficient")]
    [SerializeField] private float dragGround = 5f; // 地面阻尼
    [SerializeField] private float dragAir = 2f; // 空中阻尼

    [Header("Messages")]
    public TextMeshProUGUI welcomeText;
    public Image backgroundPanel;
    public Image backgroundBook;
    public float messageDuration = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        count = 0;
        currentMoveSpeed = baseMoveSpeed;
        SetCountText();

        // 初始化隐藏所有UI
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (welcomeText != null) welcomeText.gameObject.SetActive(false);
        if (backgroundPanel != null) backgroundPanel.gameObject.SetActive(false);
        if (backgroundBook != null) backgroundBook.gameObject.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 视角控制保持不变
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

        // Shift加速检测
        if (hasSpeedBoost && Keyboard.current.leftShiftKey.isPressed)
        {
            currentMoveSpeed = baseMoveSpeed * speedBoostMultiplier;
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }
    }

    void FixedUpdate()
    {
        // 根据是否在地面设置不同阻尼
        rb.drag = IsGrounded() ? dragGround : dragAir;

        Vector3 movement = transform.forward * moveY + transform.right * moveX;
        rb.AddForce(movement * currentMoveSpeed);
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
        }
        else
        {
            ShowHintMessage();
        }
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
        // 可以添加其他胜利效果
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

        }
        else if (other.gameObject.CompareTag("speed"))
        {
            ShowWelcomeMessage5();
            hasSpeedBoost = true;
            if (speedBoostAudio != null) speedBoostAudio.Play();
            Destroy(other.gameObject); // 拾取后移除
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