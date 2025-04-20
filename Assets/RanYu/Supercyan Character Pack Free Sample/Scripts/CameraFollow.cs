using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public GameObject PosCamer;
    // �����Ŀ��֮���ƫ����
    public Vector3 offset;
    // ��������ƽ���ȣ�ֵԽ�����Խƽ��
    public float smoothSpeed = 0.125f;
    // ���������
    public float mouseSensitivity = 2.0f;
    // ��ֱ�ӽǵ���С�Ƕ�
    public float minVerticalAngle = -45f;
    // ��ֱ�ӽǵ����Ƕ�
    public float maxVerticalAngle = 45f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // ��������겢��������Ļ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // ��ȡ��������
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ����ˮƽ�ʹ�ֱ�������ת�Ƕ�
        rotationY += mouseX;
        rotationX -= mouseY;
        // ���ƴ�ֱ�������ת�Ƕ�
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // ������ת����Ԫ��
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        // �������Ӧ�õ����Ŀ��λ��
        Vector3 desiredPosition = target.position + rotation * offset;
        // ʹ��ƽ����ֵ���ƶ������Ŀ��λ��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // ���������λ��
        transform.position = smoothedPosition;

        // �����ʼ�տ���Ŀ�����
        transform.LookAt(PosCamer.transform);
    }
}