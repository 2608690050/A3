using Supercyan.FreeSample;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTypingEffect : MonoBehaviour
{
    // �洢������ӵ�����
    public string[] sentences;
    // �ı���ʾ���
    public TMP_Text textComponent;
    // ÿ���ַ�����ʾ���ʱ��
    public float typingSpeed = 0.1f;

    // ��ǰ���ӵ�����
    private int currentSentenceIndex = 0;
    // ��ǰ��ʾ���ַ�����
    private int currentCharIndex = 0;
    // ���ڿ��ƴ���Ч����Э��
    private Coroutine typingCoroutine;
    public SimpleSampleCharacterControl characterControl;
    public CameraFollow cameraFollow;
    void Start()
    {
        // ��ʼ��ʾ��һ������
        StartTyping();
        characterControl.enabled = false;
        cameraFollow.enabled = false;
    }

    void Update()
    {
        // ��������������¼�
        if (Input.GetMouseButtonDown(0))
        {
            // ֹͣ��ǰ�Ĵ���Э��
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            // �ƶ�����һ������
            currentSentenceIndex++;
            if (currentSentenceIndex < sentences.Length)
            {
                // ��ʼ��ʾ��һ������
                StartTyping();
            }
            else
            {
                // ���û�и�����ӣ�����ı�
                textComponent.text = "";
                Destroy(gameObject);
            }
        }
    }

    void StartTyping()
    {
        // ���õ�ǰ�ַ�����
        currentCharIndex = 0;
        // ����ı���ʾ
        textComponent.text = "";
        // ��������Э��
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        // ��ȡ��ǰ����
        string currentSentence = sentences[currentSentenceIndex];
        // ������ʾ����
        while (currentCharIndex < currentSentence.Length)
        {
            // ���ı������׷��һ���ַ�
            textComponent.text += currentSentence[currentCharIndex];
            // �����ַ�����
            currentCharIndex++;
            // �ȴ�һ��ʱ�����ʾ��һ���ַ�
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void OnDestroy()
    {
        characterControl.enabled = true;
        cameraFollow.enabled = true;
    }
}