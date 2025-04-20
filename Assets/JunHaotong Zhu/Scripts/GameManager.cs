using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public int maxPartCount = 2;
    public int maxNoteCount = 2;

    private int curPartCount = 0;
    private int curNoteCount = 0;

    private bool inMixRange = false;
    public void InMixRange(bool val)
    {
        inMixRange = val;
        if (CanMix())
            UIMain.Instance.Open2CloseTip(val);
        else
            UIMain.Instance.ShowFailPanel(val);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inMixRange)
        {
            if(CanMix())
            {
                UIMain.Instance.ShowSuccess();
            }
        }
    }

    public bool CanMix()
    {
        if(curNoteCount >= maxNoteCount && curPartCount >= maxPartCount)
        {
            return true;
        }
        return false;
    }
    protected override void OnStart()
    {
        curNoteCount = 0;
        curPartCount = 0;

        UIMain.Instance.UpdateCollectCount(curPartCount, curNoteCount);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void AddCollect(CollectType type)
    {
        if (type == CollectType.Part)
            curPartCount++;
        else
            curNoteCount++;
        UIMain.Instance.UpdateCollectCount(curPartCount, curNoteCount);
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Main");
    }
}
