using UnityEngine;

public class BalloonFlight : MonoBehaviour
{
    public float baseBuoyancyForce = 10f; // 基础浮力大小
    public float buoyancyChangeRate = 2f; // 浮力变化速率
    public float movementSpeed = 5f; // 前后左右移动速度
    public float rotationSpeed = 100f; // 旋转速度
    public float maxHeight = 100f; // 最大上升高度
    public Vector3 resetPosition; // 重置位置

    private Rigidbody rb;
    private float currentBuoyancyForce;
    private bool canControl = true; // 控制是否可飞行的标志位
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
            // 高度控制
            if (Input.GetKey(KeyCode.Q) && transform.position.y < maxHeight)
            {
                // 按Q键上升，增加浮力，且高度未达到最大高度
                currentBuoyancyForce += buoyancyChangeRate * Time.fixedDeltaTime;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                // 按E键下降，减少浮力，不受最大高度限制
                currentBuoyancyForce -= buoyancyChangeRate * Time.fixedDeltaTime;
                currentBuoyancyForce = Mathf.Max(currentBuoyancyForce, 0f); // 确保浮力不小于0
            }

            // 施加浮力
            rb.AddForce(Vector3.up * currentBuoyancyForce);

            // 前后左右移动控制
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            Vector3 movementForce = movementDirection * movementSpeed;
            rb.AddForce(transform.TransformDirection(movementForce));

            // 相机控制转向
            float mouseX = Input.GetAxis("Mouse X");
            Quaternion rotation = Quaternion.Euler(0f, mouseX * rotationSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * rotation);

            // 限制高度
            if (transform.position.y > maxHeight)
            {
                Vector3 newPosition = new Vector3(transform.position.x, maxHeight, transform.position.z);
                transform.position = newPosition;
                // 当超过最大高度时，重置浮力为刚好维持在最大高度的浮力
                currentBuoyancyForce = Physics.gravity.magnitude * rb.mass;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("到达山顶"))
        {
            canControl = false; // 碰撞到到达点后，禁止控制
            // 停止热气球的运动
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Destroy(rb);
            Destroy(this);

        }
        else
        {
            // 当碰撞到其他物体时，重置热气球的位置
            transform.position = resetPosition;
            // 重置浮力
            currentBuoyancyForce = baseBuoyancyForce;
            // 重置速度
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // 重置旋转
            transform.rotation = Quaternion.identity;
        }
    }
    private void OnDestroy()
    {
        transform.rotation = Quaternion.identity;
        UI.SetActive(true);
    }
}