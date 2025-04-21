using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class F_Input_ : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerCamera;
    public GameObject Obj;
    public GameObject ObjCamer;
    public GameObject UI;
    public GameObject UI_;
    public GameObject Close;
    bool key;
    public GameObject UI_Tisp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.SetActive(true);
            key = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI.SetActive(false);
            key = false;
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&key)
        {
            Player.SetActive(false);
            PlayerCamera.SetActive(false);
            UI_.SetActive(false);
            Obj.SetActive(true);
            UI_Tisp.SetActive(true);
            ObjCamer.SetActive(true);
            Destroy(Close);
        }
    }
}
