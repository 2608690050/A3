using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxCreateCtr : MonoBehaviour
{
    private bool isCreated = false;

    public GameObject fox;

    private void Start()
    {
        fox.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && fox != null)
        {
            fox.SetActive(true);
        }
    }
}
