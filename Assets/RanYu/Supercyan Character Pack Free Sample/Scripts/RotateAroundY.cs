using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    // ������ת�ٶȣ�����Inspector����е���
    public float rotationSpeed = 30f;

    void Update()
    {
        // ʹ��Transform.Rotate������Y����ת
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}