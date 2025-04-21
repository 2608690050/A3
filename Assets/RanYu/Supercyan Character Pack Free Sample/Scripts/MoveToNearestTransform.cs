using UnityEngine;

public class MoveToNearestTransform : MonoBehaviour
{
    // 存储多个Transform的数组
    public Transform[] targetTransforms;

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否具有"AAA"标签
        if (other.CompareTag("Water"))
        {
            // 调用方法查找最近的Transform并移动角色
            MoveToNearest();
        }
    }

    private void MoveToNearest()
    {
        if (targetTransforms.Length == 0)
        {
            Debug.LogWarning("No target transforms assigned.");
            return;
        }

        // 初始化最短距离为一个较大的值
        float shortestDistance = float.MaxValue;
        Transform nearestTransform = null;

        // 遍历所有Transform
        foreach (Transform target in targetTransforms)
        {
            // 计算角色与当前Transform之间的距离
            float distance = Vector3.Distance(transform.position, target.position);

            // 如果当前距离比最短距离小，则更新最短距离和最近的Transform
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTransform = target;
            }
        }

        // 如果找到了最近的Transform，则将角色的位置设置为该Transform的位置
        if (nearestTransform != null)
        {
            transform.position = nearestTransform.position;
        }
    }
}