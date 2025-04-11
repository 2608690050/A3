using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private float moveX, moveY;
    private float moveSpeed = 15;
    private int count;

    // ��Ծ����
    public float jumpForce = 10f;          // ��Ծ����
    public float jumpCooldown = 0.5f;      // ��Ծ��ȴʱ�䣨�룩
    private float lastJumpTime = -1f;      // �ϴ���Ծʱ��
    public AudioSource jumpAudio;          // ��Ծ��Ч

    [Header("UI")]
    public TextMeshProUGUI countText;
    public AudioSource clickAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
    }

    // ��Ծ���루����ȴ��⣩
    public void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && Time.time - lastJumpTime >= jumpCooldown)
        {
            Jump();
        }
    }

    // ��Ծ�߼�
    void Jump()
    {
        // ���ô�ֱ�ٶȣ�ȷ��ÿ����Ծ�߶�һ�£�
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // ��¼��Ծʱ��
        lastJumpTime = Time.time;

        if (jumpAudio != null)
        {
            jumpAudio.Play();
        }
    }

    // ԭ�������������ֲ���
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