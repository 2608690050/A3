using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    [Header("��Ծ����")]
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheckPoint; // ������㣨�����ɫ�Ų������壩

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ����������ת��ֹ��б
        rb.freezeRotation = true;
    }

    void Update()
    {
        // ���봦��
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���������ˮƽ��������ƶ�
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        movement = (camForward * vertical + Camera.main.transform.right * horizontal).normalized;

        // ��Ծ��⣨ȷ���ڵ����Ұ��¿ո�
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // ǿ������ Y ���ƶ�����ֹƮ����
        Vector3 horizontalMove = new Vector3(movement.x, 0, movement.z);
        rb.MovePosition(rb.position + horizontalMove * moveSpeed * Time.fixedDeltaTime);

        // �����⣨�ӽŲ��������ߣ�
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundCheckDistance, groundLayer);

        // ��ѡ�����Ƽ�����ߣ�Scene ��ͼ�ɼ���
        Debug.DrawRay(groundCheckPoint.position, Vector3.down * groundCheckDistance, Color.red);
    }
}




