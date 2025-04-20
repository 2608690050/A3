using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerEvent : MonoBehaviour
{
    public Action<Collider> triggerEventDo;
    public Action<Collision> ColliderEventDo;
    /// <summary>
    /// 处理trigger事件
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (triggerEventDo!=null)
        {
            triggerEventDo(other);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(123);
        if (ColliderEventDo!=null)
        {
            ColliderEventDo(collision);
        }
    }
}
