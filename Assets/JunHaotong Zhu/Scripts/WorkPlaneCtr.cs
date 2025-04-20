using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlaneCtr : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.InMixRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.InMixRange(false);
        }
    }
}
