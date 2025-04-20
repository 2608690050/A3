using UnityEngine;

public class Player : MonoBehaviour
{
    // �����ٶȡ����ź���ת����
    public float moveSpeed = 5.0f;
    public float zoomSpeed = 2.0f;
    public float rotateSpeed = 90.0f; // ��ת�ٶȣ���/�룩

    // ��ʼ״̬��¼
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        // ��¼��ʼ״̬
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // �����ƶ�����
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    // ���ŷ���
    public void ZoomIn()
    {
        transform.localScale += Vector3.one * zoomSpeed * Time.deltaTime;
    }

    public void ZoomOut()
    {
        transform.localScale -= Vector3.one * zoomSpeed * Time.deltaTime;
    }

    // ��ת��������Y�ᣩ
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    // ���÷���
    public void Reset()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
}