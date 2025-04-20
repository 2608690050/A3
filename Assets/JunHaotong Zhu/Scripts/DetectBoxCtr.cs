using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBoxCtr : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.ReStart();
        }
    }
}
