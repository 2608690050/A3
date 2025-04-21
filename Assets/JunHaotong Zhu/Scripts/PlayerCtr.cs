using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public float jumpForce = 5f; // ��Ծ����
    public float downForce = 20f;
    public Animator animator; // ��ɫ��Animator���

    private Rigidbody rb; // ��ɫ��Rigidbody���
    private Vector3 moveDirection; // �ƶ�����
    private bool isGrounded; // �Ƿ�Ӵ�����
    private float groundCheckDistance = 0.1f; // ���������
    private LayerMask groundLayer; // �����

    public Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ȷ��Animator����Ѹ�ֵ
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator is not assigned!");
            }
        }

        // ���õ���㣨��������Layer��"Ground"��
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        // ��ȡ����
        float horizontal = Input.GetAxis("Horizontal"); // �����ƶ�
        float vertical = Input.GetAxis("Vertical"); // ǰ���ƶ�

        // �����ƶ����򣨻��ڽ�ɫ�ĳ���
        moveDirection = cam.transform.forward * vertical + cam.transform.right * horizontal;
        moveDirection.y = 0; // ����Y���ƶ�����ֹ��ɫ����

        // �ƶ���ɫ
        if (moveDirection.magnitude > 0.1f) // ����Ƿ�������
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
