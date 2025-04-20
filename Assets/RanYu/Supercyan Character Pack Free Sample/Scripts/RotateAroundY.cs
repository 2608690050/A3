using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    // 定义旋转速度，可在Inspector面板中调整
    public float rotationSpeed = 30f;

    void Update()
    {
        // 使用Transform.Rotate方法绕Y轴旋转
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}