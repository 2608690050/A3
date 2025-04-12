using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Player的Transform
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 3f;

    private float currentXAngle = 0f;
    private float currentYAngle = 0f;

    void LateUpdate()
    {
        // 获取鼠标输入
        currentXAngle += Input.GetAxis("Mouse X") * rotationSpeed;
        currentYAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentYAngle = Mathf.Clamp(currentYAngle, -20f, 60f); // 限制上下角度

        // 计算摄像机位置
        Quaternion rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);
        Vector3 position = rotation * new Vector3(0, height, -distance) + target.position;

        // 应用变换
        transform.rotation = rotation;
        transform.position = position;
    }
}
