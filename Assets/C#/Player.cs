using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private float moveX, moveY;
    private float moveSpeed = 15;
    private int count;

    // 跳跃参数
    public float jumpForce = 10f;          // 跳跃力度
    public float jumpCooldown = 0.5f;      // 跳跃冷却时间（秒）
    private float lastJumpTime = -1f;      // 上次跳跃时间
    public AudioSource jumpAudio;          // 跳跃音效

    [Header("UI")]
    public TextMeshProUGUI countText;
    public AudioSource clickAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
    }

    // 跳跃输入（带冷却检测）
    public void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && Time.time - lastJumpTime >= jumpCooldown)
        {
            Jump();
        }
    }

    // 跳跃逻辑
    void Jump()
    {
        // 重置垂直速度（确保每次跳跃高度一致）
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // 记录跳跃时间
        lastJumpTime = Time.time;

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    // 原有其他方法保持不变
    public void OnMove(InputValue moveValue)
    {
        Vector2 moveVector = moveValue.Get<Vector2>();
        moveX = moveVector.x;
        moveY = moveVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        rb.AddForce(movement * moveSpeed);
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

        if (other.gameObject.CompareTag("Sphere"))
        {
            other.gameObject.SetActive(false);
            count = count + 2;
            SetCountText();
            clickAudio.Play();
        }
    }

    public void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }
}