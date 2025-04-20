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
            //��������߼�
            // ��ȡ��ǰ�����������
            string currentSceneName = SceneManager.GetActiveScene().name;

            // ���¼��ص�ǰ����
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
