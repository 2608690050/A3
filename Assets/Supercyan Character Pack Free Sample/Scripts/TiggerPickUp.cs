using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TiggerPickUp : MonoBehaviour
{
    public GameObject _TextUI;
    private Text_Count text_Count;
    private AudioSource audio;
    bool Key;
    // Start is called before the first frame update
    void Start()
    {
        text_Count = GameObject.FindGameObjectWithTag("Text_Count").GetComponent<Text_Count>();
        audio = GameObject.Find("ºÒ∆“Ù–ß").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&Key)
        {
            text_Count.Add();
            audio.Play();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            _TextUI.SetActive(true);
            Key = true;
           
            }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            _TextUI.SetActive(false);
            Key = false;
        }

    }
}
