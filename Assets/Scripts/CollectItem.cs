using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectType
{
    Part = 1,
    Note = 2,
}
public class CollectItem : MonoBehaviour
{
    public CollectType type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            GameManager.Instance.AddCollect(type);
            Destroy(this.gameObject);
        }
    }
}
