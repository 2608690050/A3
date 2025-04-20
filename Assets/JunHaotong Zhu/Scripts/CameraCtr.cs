using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtr : MonoBehaviour
{
    public Transform target; // ��ɫ��Ŀ��Transform
    public float distance = 5f; // ��������ɫ�ľ���

    public float rotationSpeed = 5f; // �����ת�ٶ�
    public float smoothSpeed = 0.125f; // �����ƽ�������ٶ�

    private float mouseX = 0f; // ���ˮƽ����
    private float mouseY = 0f; // ��괹ֱ����

    private Vector3 currentVelocity = Vector3.zero; // ƽ����ֵ���ٶȻ���

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

        // ��ȡ������벢ƽ������
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -85f, 85f); // ���ƴ�ֱ�Ƕȣ����ⷭת

        // ����Ŀ����ת
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // ����Ŀ��λ��
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance;

        transform.position = desiredPosition;
        // ������������ɫ
        transform.LookAt(target.position);
    }
}
