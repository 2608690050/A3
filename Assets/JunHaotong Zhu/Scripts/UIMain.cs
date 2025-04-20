using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public GameObject MixTip;
    public GameObject sucessPanel;
    public GameObject failTipPanel;

    public Text collectText;
    private void Start()
    {
        
    }
    public void Open2CloseTip(bool isopen)
    {
        MixTip.SetActive(isopen);
    }

    internal void ShowSuccess()
    {
        sucessPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnClickedRestart()
    {
        GameManager.Instance.ReStart();
    }

    public void ShowFailPanel(bool val)
    {
        failTipPanel.SetActive(val);
    }

    public void UpdateCollectCount(int partCount, int noteCount)
    {
        collectText.text = "music note£º" + noteCount + "/2\r\ntool parts£º" + partCount + "/2";
    }
}
