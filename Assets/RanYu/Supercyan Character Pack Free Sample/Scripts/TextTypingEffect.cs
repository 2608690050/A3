using Supercyan.FreeSample;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTypingEffect : MonoBehaviour
{
    // 存储多个句子的数组
    public string[] sentences;
    // 文本显示组件
    public TMP_Text textComponent;
    // 每个字符的显示间隔时间
    public float typingSpeed = 0.1f;

    // 当前句子的索引
    private int currentSentenceIndex = 0;
    // 当前显示的字符索引
    private int currentCharIndex = 0;
    // 用于控制打字效果的协程
    private Coroutine typingCoroutine;
    public SimpleSampleCharacterControl characterControl;
    public CameraFollow cameraFollow;
    void Start()
    {
        // 开始显示第一个句子
        StartTyping();
        characterControl.enabled = false;
        cameraFollow.enabled = false;
    }

    void Update()
    {
        // 检测键盘左键按下事件
        if (Input.GetMouseButtonDown(0))
        {
            // 停止当前的打字协程
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            // 移动到下一个句子
            currentSentenceIndex++;
            if (currentSentenceIndex < sentences.Length)
            {
                // 开始显示下一个句子
                StartTyping();
            }
            else
            {
                // 如果没有更多句子，清空文本
                textComponent.text = "";
                Destroy(gameObject);
            }
        }
    }

    void StartTyping()
    {
        // 重置当前字符索引
        currentCharIndex = 0;
        // 清空文本显示
        textComponent.text = "";
        // 启动打字协程
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        // 获取当前句子
        string currentSentence = sentences[currentSentenceIndex];
        // 逐字显示句子
        while (currentCharIndex < currentSentence.Length)
        {
            // 在文本组件中追加一个字符
            textComponent.text += currentSentence[currentCharIndex];
            // 增加字符索引
            currentCharIndex++;
            // 等待一段时间后显示下一个字符
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void OnDestroy()
    {
        characterControl.enabled = true;
        cameraFollow.enabled = true;
    }
}