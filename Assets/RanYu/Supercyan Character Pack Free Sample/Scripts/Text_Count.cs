using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_Count : MonoBehaviour
{
    public TMP_Text tMP;
    int Count;
    public GameObject Obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Add()
    {
        Count++;
    }
    // Update is called once per frame
    void Update()
    {
        tMP.text = Count + "/5";
        if(Count>=5)
        {
            Obj.SetActive(true);
        }
    }
}
