using UnityEngine;

public class BalloonFlight : MonoBehaviour
{
    public float baseBuoyancyForce = 10f; // ����������С
    public float buoyancyChangeRate = 2f; // �����仯����
    public float movementSpeed = 5f; // ǰ�������ƶ��ٶ�
    public float rotationSpeed = 100f; // ��ת�ٶ�
    public float maxHeight = 100f; // ��������߶�
    public Vector3 resetPosition; // ����λ��

    private Rigidbody rb;
    private float currentBuoyancyForce;
    private bool canControl = true; // �����Ƿ�ɷ��еı�־λ
    public GameObject UI;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBuoyancyForce = baseBuoyancyForce;
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            // �߶ȿ���
            if (Input.GetKey(KeyCode.Q) && transform.position.y < maxHeight)
            {
                // ��Q�����������Ӹ������Ҹ߶�δ�ﵽ���߶�
                currentBuoyancyForce += buoyancyChangeRate * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                // ��E���½������ٸ������������߶�����
                currentBuoyancyForce -= buoyancyChangeRate * Time.fixedDeltaTime;
                currentBuoyancyForce = Mathf.Max(currentBuoyancyForce, 0f); // ȷ��������С��0
            }

            // ʩ�Ӹ���
            rb.AddForce(Vector3.up * currentBuoyancyForce);

            // ǰ�������ƶ�����
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            Vector3 movementForce = movementDirection * movementSpeed;
            rb.AddForce(transform.TransformDirection(movementForce));

            // �������ת��
            float mouseX = Input.GetAxis("Mouse X");
            Quaternion rotation = Quaternion.Euler(0f, mouseX * rotationSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * rotation);

            // ���Ƹ߶�
            if (transform.position.y > maxHeight)
            {
                Vector3 newPosition = new Vector3(transform.position.x, maxHeight, transform.position.z);
                transform.position = newPosition;
                // ���������߶�ʱ�����ø���Ϊ�պ�ά�������߶ȵĸ���
                currentBuoyancyForce = Physics.gravity.magnitude * rb.mass;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("����ɽ��"))
        {
            canControl = false; // ��ײ�������󣬽�ֹ����
            // ֹͣ��������˶�
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(rb);
            Destroy(this);

        }
        else
        {
            // ����ײ����������ʱ�������������λ��
            transform.position = resetPosition;
            // ���ø���
            currentBuoyancyForce = baseBuoyancyForce;
            // �����ٶ�
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // ������ת
            transform.rotation = Quaternion.identity;
        }
    }
    private void OnDestroy()
    {
        transform.rotation = Quaternion.identity;
        UI.SetActive(true);
    }
}