using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    // �ƶ�����Ծ����
    private Rigidbody rb;
    private float moveX, moveY;
    private float moveSpeed = 15;
    private int count;
    private Vector3 startPosition;

    // ��Ծ����
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    private float lastJumpTime = -1f;
    public AudioSource jumpAudio;

    // �ӽǿ��Ʋ���
    public Transform cameraTransform; // �����������
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;

    [Header("UI")]
    public TextMeshProUGUI countText;
    public TextMeshProUGUI gameOverText;
    public AudioSource clickAudio;
    public AudioSource waterSplashAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        count = 0;
        SetCountText();
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);

        // �������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ʹPlayerʼ�����������ǰ��������Y�ᣩ
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
        // �ƶ��������Player��ǰ���򣨼��ӽǷ���
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
    }

    public void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }
}