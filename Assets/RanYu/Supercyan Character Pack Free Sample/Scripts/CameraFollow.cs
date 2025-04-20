using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public GameObject PosCamer;
    // 相机与目标之间的偏移量
    public Vector3 offset;
    // 相机跟随的平滑度，值越大跟随越平滑
    public float smoothSpeed = 0.125f;
    // 鼠标灵敏度
    public float mouseSensitivity = 2.0f;
    // 垂直视角的最小角度
    public float minVerticalAngle = -45f;
    // 垂直视角的最大角度
    public float maxVerticalAngle = 45f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // 隐藏鼠标光标并锁定在屏幕中心
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 获取鼠标的输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 计算水平和垂直方向的旋转角度
        rotationY += mouseX;
        rotationX -= mouseY;
        // 限制垂直方向的旋转角度
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // 创建旋转的四元数
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        // 计算相机应该到达的目标位置
        Vector3 desiredPosition = target.position + rotation * offset;
        // 使用平滑插值来移动相机到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // 更新相机的位置
        transform.position = smoothedPosition;

        // 让相机始终看向目标对象
        transform.LookAt(PosCamer.transform);
    }
}