using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtr : MonoBehaviour
{
    public Transform target; // 角色的目标Transform
    public float distance = 5f; // 摄像机与角色的距离

    public float rotationSpeed = 5f; // 鼠标旋转速度
    public float smoothSpeed = 0.125f; // 摄像机平滑跟随速度

    private float mouseX = 0f; // 鼠标水平输入
    private float mouseY = 0f; // 鼠标垂直输入

    private Vector3 currentVelocity = Vector3.zero; // 平滑插值的速度缓存

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned!");
            return;
        }

        transform.position = target.position - target.forward * distance;
        transform.LookAt(target.position);

        mouseX = transform.eulerAngles.y;
        mouseY = transform.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0); 
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 获取鼠标输入并平滑处理
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -85f, 85f); // 限制垂直角度，避免翻转

        // 计算目标旋转
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // 计算目标位置
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;

        transform.position = desiredPosition;
        // 让摄像机看向角色
        transform.LookAt(target.position);
    }
}
