using UnityEngine;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public float displayTime = 5f; // 对话框显示时间
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        HideDialog();
    }

    public void ShowDialog()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(HideDialogAfterTime());
    }

    private IEnumerator HideDialogAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        HideDialog();
    }

    private void HideDialog()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}