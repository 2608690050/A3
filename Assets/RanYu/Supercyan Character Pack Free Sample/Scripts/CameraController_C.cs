using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_C : MonoBehaviour
{
    public Transform target; // 跟随的目标，即热气球
    public float distance = 5f; // 相机与目标的距离
    public float height = 2f; // 相机相对于目标的高度
    public float rotationDamping = 3f; // 旋转阻尼

    void LateUpdate()
    {
        if (!target)
            return;

        // 计算相机的目标位置
        Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;

        // 平滑移动相机到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationDamping);

        // 让相机始终看向目标
        transform.LookAt(target);
    }
}
