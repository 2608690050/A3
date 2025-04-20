using UnityEngine;

public class Player : MonoBehaviour
{
    // 控制速度、缩放和旋转幅度
    public float moveSpeed = 5.0f;
    public float zoomSpeed = 2.0f;
    public float rotateSpeed = 90.0f; // 旋转速度（度/秒）

    // 初始状态记录
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        // 记录初始状态
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // 左右移动方法
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    // 缩放方法
    public void ZoomIn()
    {
        transform.localScale += Vector3.one * zoomSpeed * Time.deltaTime;
    }

    public void ZoomOut()
    {
        transform.localScale -= Vector3.one * zoomSpeed * Time.deltaTime;
    }

    // 旋转方法（绕Y轴）
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    // 重置方法
    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
}