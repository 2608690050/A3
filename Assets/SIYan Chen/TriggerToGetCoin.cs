using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToGetCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<ColliderTriggerEvent>().triggerEventDo = TriggerDo;
    }

public void TriggerDo(Collider collider) 
    {
        if (collider.CompareTag("Player"))
        {
            //ʰȡ����߼�
            Destroy(this.gameObject);
        }
    }
}
