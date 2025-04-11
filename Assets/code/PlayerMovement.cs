using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    [Header("跳跃参数")]
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheckPoint; // 地面检测点（拖入角色脚部空物体）

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 锁定刚体旋转防止倾斜
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 输入处理
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 基于摄像机水平方向计算移动
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        movement = (camForward * vertical + Camera.main.transform.right * horizontal).normalized;

        // 跳跃检测（确保在地面且按下空格）
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 强制锁定 Y 轴移动（防止飘浮）
        Vector3 horizontalMove = new Vector3(movement.x, 0, movement.z);
        rb.MovePosition(rb.position + horizontalMove * moveSpeed * Time.fixedDeltaTime);

        // 地面检测（从脚部发射射线）
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundCheckDistance, groundLayer);

        // 可选：绘制检测射线（Scene 视图可见）
        Debug.DrawRay(groundCheckPoint.position, Vector3.down * groundCheckDistance, Color.red);
    }
}




