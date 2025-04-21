using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Input_ : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerCamera;
    public GameObject Obj;
    public GameObject ObjCamer;
    public GameObject CC;
    public GameObject UI_Tisp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.SetActive(true);
            CC.SetActive(true);
            PlayerCamera.SetActive(true);
            Obj.SetActive(false);
            UI_Tisp.SetActive(false);
            ObjCamer.SetActive(false);
            Destroy(gameObject);

        }
    }
}
