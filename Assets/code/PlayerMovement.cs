using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    private float moveX;
    private float moveY;
    private float moveSpeed = 15;

    private int count;
    public TextMeshProUGUI countText;
    public AudioSource clickAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        //SetCountText();
    }

    public void OnMove(InputValue moveValue)
    {
        Vector2 moveVector = moveValue.Get<Vector2>();
        moveX = moveVector.x;
        moveY = moveVector.y;
    }
    float speed=5;
    void FixedUpdate()
    {
        Vector3 move = Vector3.zero;

        // 根据输入控制移动
        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += Vector3.right;
        }

        // 移动角色
        rb.MovePosition(rb.position + (Vector3)(move * Time.fixedDeltaTime * speed));
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Pickup"))
        //{
        //    other.gameObject.SetActive(false);
        //    count = count + 1;
        //    SetCountText();
        //    clickAudio.Play();
        //}

        //if (other.gameObject.CompareTag("S1"))
        //{
        //    other.gameObject.SetActive(false);
        //    count = count + 2;
        //    SetCountText();
        //    clickAudio.Play();
        //}
    }

    //public void SetCountText()
    //{
    //    countText.text = " Score: " + count.ToString();
    //}
} // 添加这行闭合类的大括号







