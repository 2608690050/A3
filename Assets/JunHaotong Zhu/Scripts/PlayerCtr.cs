using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public float jumpForce = 5f; // 跳跃力度
    public float downForce = 20f;
    public Animator animator; // 角色的Animator组件

    private Rigidbody rb; // 角色的Rigidbody组件
    private Vector3 moveDirection; // 移动方向
    private bool isGrounded; // 是否接触地面
    private float groundCheckDistance = 0.1f; // 地面检测距离
    private LayerMask groundLayer; // 地面层

    public Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 确保Animator组件已赋值
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator is not assigned!");
            }
        }

        // 设置地面层（假设地面的Layer是"Ground"）
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // 左右移动
        float vertical = Input.GetAxis("Vertical"); // 前后移动

        // 计算移动方向（基于角色的朝向）
        moveDirection = cam.transform.forward * vertical + cam.transform.right * horizontal;
        moveDirection.y = 0; // 忽略Y轴移动（防止角色飞起）

        // 移动角色
        if (moveDirection.magnitude > 0.1f) // 检测是否有输入
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = targetRotation;// Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
