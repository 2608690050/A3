using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Player��Transform
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 3f;

    private float currentXAngle = 0f;
    private float currentYAngle = 0f;

    void LateUpdate()
    {
        // ��ȡ�������
        currentXAngle += Input.GetAxis("Mouse X") * rotationSpeed;
        currentYAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentYAngle = Mathf.Clamp(currentYAngle, -20f, 60f); // �������½Ƕ�

        // ���������λ��
        Quaternion rotation = Quaternion.Euler(currentYAngle, currentXAngle, 0);
        Vector3 position = rotation * new Vector3(0, height, -distance) + target.position;

        // Ӧ�ñ任
        transform.rotation = rotation;
        transform.position = position;
    }
}
