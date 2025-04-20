using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxCtr : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float hideTime = 5f;

    private float timer;
    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > hideTime)
        {
            Destroy(this.gameObject);
        }
        this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("重新开始");
            GameManager.Instance.ReStart();
        }
    }
}
