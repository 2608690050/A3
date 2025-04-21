using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ArrowGame : MonoBehaviour
{
    // 存储 4 个方向箭头的 Sprite 数组
    public Sprite[] arrowSprites;
    // 存储 5 个 Image 对象的数组
    public Image[] arrowImages;
    // 当前需要按下的方向箭头索引列表
    private List<int> currentArrowIndices;
    // 当前需要按下的方向箭头索引
    private int currentIndexToCheck;
    // 时间限制（秒）
    private float timeLimit = 3f;
    // 当前计时
    private float currentTime;
    // 音频源组件
    public AudioSource audioSource;
    // 音量渐变速度
    public float volumeChangeSpeed = 0.5f;
    // 目标音量
    private float targetVolume;
    // 要显示的带有 Image 组件的物体
    public Image endGameImage;
    // 游戏是否结束的标志
    private bool isGameOver = false;

    void Start()
    {
        // 初始化列表
        currentArrowIndices = new List<int>();
        // 生成随机箭头
        GenerateRandomArrows();
        // 初始化音频源音量
        audioSource.volume = 0f;
        targetVolume = 0f;
        // 隐藏结束游戏的图片
        endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 0f);
        // 启动协程，10 秒后停止游戏
        StartCoroutine(EndGameAfterTime(10f));
    }

    void GenerateRandomArrows()
    {
        // 清空列表
        currentArrowIndices.Clear();
        for (int i = 0; i < arrowImages.Length; i++)
        {
            // 随机生成一个 0 到 3 之间的索引
            int randomIndex = Random.Range(0, arrowSprites.Length);
            // 将随机索引添加到列表中
            currentArrowIndices.Add(randomIndex);
            // 设置 Image 的 Sprite
            arrowImages[i].sprite = arrowSprites[randomIndex];
            // 恢复图片默认颜色
            arrowImages[i].color = Color.white;
        }
        // 从第一个箭头开始检查
        currentIndexToCheck = 0;
        // 重置计时
        currentTime = timeLimit;
    }

    void Update()
    {
        if (isGameOver) return;

        // 计时
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            // 时间到，重新生成随机箭头
            GenerateRandomArrows();
            // 按错，目标音量设为 0
            targetVolume = 0f;
            return;
        }

        // 音量渐变
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, volumeChangeSpeed * Time.deltaTime);

        if (currentIndexToCheck < currentArrowIndices.Count)
        {
            int currentArrowIndex = currentArrowIndices[currentIndexToCheck];
            // 根据当前箭头索引判断需要按下的按键
            if ((currentArrowIndex == 0 && Input.GetKeyDown(KeyCode.UpArrow)) ||
                (currentArrowIndex == 1 && Input.GetKeyDown(KeyCode.DownArrow)) ||
                (currentArrowIndex == 2 && Input.GetKeyDown(KeyCode.LeftArrow)) ||
                (currentArrowIndex == 3 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                // 如果按下的按键正确，图片颜色变绿
                arrowImages[currentIndexToCheck].color = Color.green;
                // 按对，目标音量设为 1
                targetVolume = 1f;
                // 移动到下一个箭头
                currentIndexToCheck++;
                if (currentIndexToCheck == currentArrowIndices.Count)
                {
                    // 如果所有箭头都正确按下，重新生成随机箭头
                    GenerateRandomArrows();
                    // 重新生成后目标音量设为 0
                    targetVolume = 0f;
                }
                else
                {
                    // 重置计时，因为进入下一个箭头判断
                    currentTime = timeLimit;
                }
            }
            else if (Input.anyKeyDown)
            {
                // 如果按下了错误的按键，图片颜色变红
                arrowImages[currentIndexToCheck].color = Color.red;
                // 按错，目标音量设为 0
                targetVolume = 0f;
                // 移动到下一个箭头
                currentIndexToCheck++;
                if (currentIndexToCheck == currentArrowIndices.Count)
                {
                    // 如果所有箭头都判断完了，重新生成随机箭头
                    GenerateRandomArrows();
                    // 重新生成后目标音量设为 0
                    targetVolume = 0f;
                }
                else
                {
                    // 重置计时，因为进入下一个箭头判断
                    currentTime = timeLimit;
                }
            }
        }
    }

    IEnumerator EndGameAfterTime(float delay)
    {
        // 等待指定的时间
        yield return new WaitForSeconds(delay);
        // 标记游戏结束
        isGameOver = true;
        // 启动协程，将结束游戏的图片透明度从 0 渐变到 1
        StartCoroutine(FadeInEndGameImage(2f));
    }

    IEnumerator FadeInEndGameImage(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (elapsedTime < duration)
        {
            // 计算当前的透明度
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            // 设置图片的透明度
            endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, alpha);
            // 累加经过的时间
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保透明度最终为 1
        endGameImage.color = new Color(endGameImage.color.r, endGameImage.color.g, endGameImage.color.b, 1f);
        // 退出游戏
        Quit();
    }
    public void Quit()
    {
        // 在独立应用程序中退出游戏
#if UNITY_EDITOR
        // 在Unity编辑器中，使用UnityEditor.EditorApplication.isPlaying来停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在独立应用程序中，使用Application.Quit()来退出游戏
            Application.Quit();
#endif
    }
}