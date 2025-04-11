using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
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
        SetCountText();
    }

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
        countText.text = " Score: " + count.ToString();
    }



}
