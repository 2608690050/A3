using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_C : MonoBehaviour
{
    public Transform target; // �����Ŀ�꣬��������
    public float distance = 5f; // �����Ŀ��ľ���
    public float height = 2f; // ��������Ŀ��ĸ߶�
    public float rotationDamping = 3f; // ��ת����

    void LateUpdate()
    {
        if (!target)
            return;

        // ���������Ŀ��λ��
        Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;

        // ƽ���ƶ������Ŀ��λ��
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationDamping);

        // �����ʼ�տ���Ŀ��
        transform.LookAt(target);
    }
}
