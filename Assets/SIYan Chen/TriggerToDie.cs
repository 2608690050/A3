using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToDie : MonoBehaviour
{

    private void Start()
    {
        this.GetComponent<ColliderTriggerEvent>().ColliderEventDo = TriggerCheck;
    }
    void TriggerCheck(Collision collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            //玩家死亡逻辑
            // 获取当前活动场景的名称
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 重新加载当前场景
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
