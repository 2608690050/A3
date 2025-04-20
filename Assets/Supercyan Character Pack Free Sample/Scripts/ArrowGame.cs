using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ArrowGame : MonoBehaviour
{
    // �洢 4 �������ͷ�� Sprite ����
    public Sprite[] arrowSprites;
    // �洢 5 �� Image ���������
    public Image[] arrowImages;
    // ��ǰ��Ҫ���µķ����ͷ�����б�
    private List<int> currentArrowIndices;
    // ��ǰ��Ҫ���µķ����ͷ����
    private int currentIndexToCheck;
    // ʱ�����ƣ��룩
    private float timeLimit = 3f;
    // ��ǰ��ʱ
    private float currentTime;
    // ��ƵԴ���
    public AudioSource audioSource;
    // ���������ٶ�
    public float volumeChangeSpeed = 0.5f;
    // Ŀ������
    private float targetVolume;
    // Ҫ��ʾ�Ĵ��� Image ���������
    public Image endGameImage;
    // ��Ϸ�Ƿ�����ı�־
    private bool isGameOver = false;

    void Start()
    {
        // ��ʼ���б�
        currentArrowIndices = new List<int>();
        // ���������ͷ
        GenerateRandomArrows();
        // ��ʼ����ƵԴ����
        audioSource.volume = 0f;
        targetVolume = 0f;
        // ���ؽ�����Ϸ��ͼƬ
        endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 0f);
        // ����Э�̣�10 ���ֹͣ��Ϸ
        StartCoroutine(EndGameAfterTime(10f));
    }

    void GenerateRandomArrows()
    {
        // ����б�
        currentArrowIndices.Clear();
        for (int i = 0; i < arrowImages.Length; i++)
        {
            // �������һ�� 0 �� 3 ֮�������
            int randomIndex = Random.Range(0, arrowSprites.Length);
            // �����������ӵ��б���
            currentArrowIndices.Add(randomIndex);
            // ���� Image �� Sprite
            arrowImages[i].sprite = arrowSprites[randomIndex];
            // �ָ�ͼƬĬ����ɫ
            arrowImages[i].color = Color.white;
        }
        // �ӵ�һ����ͷ��ʼ���
        currentIndexToCheck = 0;
        // ���ü�ʱ
        currentTime = timeLimit;
    }

    void Update()
    {
        if (isGameOver) return;

        // ��ʱ
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            // ʱ�䵽���������������ͷ
            GenerateRandomArrows();
            // ����Ŀ��������Ϊ 0
            targetVolume = 0f;
            return;
        }

        // ��������
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, volumeChangeSpeed * Time.deltaTime);

        if (currentIndexToCheck < currentArrowIndices.Count)
        {
            int currentArrowIndex = currentArrowIndices[currentIndexToCheck];
            // ���ݵ�ǰ��ͷ�����ж���Ҫ���µİ���
            if ((currentArrowIndex == 0 && Input.GetKeyDown(KeyCode.UpArrow)) ||
                (currentArrowIndex == 1 && Input.GetKeyDown(KeyCode.DownArrow)) ||
                (currentArrowIndex == 2 && Input.GetKeyDown(KeyCode.LeftArrow)) ||
                (currentArrowIndex == 3 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                // ������µİ�����ȷ��ͼƬ��ɫ����
                arrowImages[currentIndexToCheck].color = Color.green;
                // ���ԣ�Ŀ��������Ϊ 1
                targetVolume = 1f;
                // �ƶ�����һ����ͷ
                currentIndexToCheck++;
                if (currentIndexToCheck == currentArrowIndices.Count)
                {
                    // ������м�ͷ����ȷ���£��������������ͷ
                    GenerateRandomArrows();
                    // �������ɺ�Ŀ��������Ϊ 0
                    targetVolume = 0f;
                }
                else
                {
                    // ���ü�ʱ����Ϊ������һ����ͷ�ж�
                    currentTime = timeLimit;
                }
            }
            else if (Input.anyKeyDown)
            {
                // ��������˴���İ�����ͼƬ��ɫ���
                arrowImages[currentIndexToCheck].color = Color.red;
                // ����Ŀ��������Ϊ 0
                targetVolume = 0f;
                // �ƶ�����һ����ͷ
                currentIndexToCheck++;
                if (currentIndexToCheck == currentArrowIndices.Count)
                {
                    // ������м�ͷ���ж����ˣ��������������ͷ
                    GenerateRandomArrows();
                    // �������ɺ�Ŀ��������Ϊ 0
                    targetVolume = 0f;
                }
                else
                {
                    // ���ü�ʱ����Ϊ������һ����ͷ�ж�
                    currentTime = timeLimit;
                }
            }
        }
    }

    IEnumerator EndGameAfterTime(float delay)
    {
        // �ȴ�ָ����ʱ��
        yield return new WaitForSeconds(delay);
        // �����Ϸ����
        isGameOver = true;
        // ����Э�̣���������Ϸ��ͼƬ͸���ȴ� 0 ���䵽 1
        StartCoroutine(FadeInEndGameImage(2f));
    }

    IEnumerator FadeInEndGameImage(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (elapsedTime < duration)
        {
            // ���㵱ǰ��͸����
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            // ����ͼƬ��͸����
            endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, alpha);
            // �ۼӾ�����ʱ��
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ��͸��������Ϊ 1
        endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 1f);
        // �˳���Ϸ
        Quit();
    }
    public void Quit()
    {
        // �ڶ���Ӧ�ó������˳���Ϸ
#if UNITY_EDITOR
        // ��Unity�༭���У�ʹ��UnityEditor.EditorApplication.isPlaying��ֹͣ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �ڶ���Ӧ�ó����У�ʹ��Application.Quit()���˳���Ϸ
            Application.Quit();
#endif
    }
}