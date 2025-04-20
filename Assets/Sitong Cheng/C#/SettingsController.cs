using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public TMP_Text volumeText;
    public AudioSource backgroundMusic;
    public Button returnButton;    // 新增返回按钮引用
    public Button settingsButton;  // 新增设置按钮引用

    [Header("Graphics Settings")]
    public Slider brightnessSlider;
    public TMP_Text brightnessText;
    public Image brightnessOverlay; // 半透明黑色UI图片，覆盖全屏

    [Header("Image Size Settings")]
    public Slider sizeSlider;          // 图片大小滑块
    public TMP_Text sizeText;          // 显示当前大小的文本
    public Image targetImage;          // 要调整大小的图片
    public Button smallSizeBtn;        // 小尺寸预设按钮
    public Button mediumSizeBtn;       // 中尺寸预设按钮
    public Button largeSizeBtn;        // 大尺寸预设按钮

    [Range(0.1f, 2f)] public float minSize = 0.5f;
    [Range(2f, 5f)] public float maxSize = 3f;
    private float defaultSize = 1f;

    private bool isPaused = false;
    private float prevTimeScale;

    void Start()
    {
        // 初始化图片大小设置
        sizeSlider.minValue = minSize;
        sizeSlider.maxValue = maxSize;
        sizeSlider.value = PlayerPrefs.GetFloat("ImageSize", defaultSize);
        UpdateSizeDisplay(sizeSlider.value);
        sizeSlider.onValueChanged.AddListener(OnSizeChanged);

        // 绑定预设按钮
        smallSizeBtn.onClick.AddListener(() => SetPresetSize(0.5f));
        mediumSizeBtn.onClick.AddListener(() => SetPresetSize(1f));
        largeSizeBtn.onClick.AddListener(() => SetPresetSize(1.5f));

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        UpdateBrightness(brightnessSlider.value);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        // 初始化音量
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        UpdateVolumeDisplay(volumeSlider.value);

        // 默认隐藏设置界面
        settingsPanel.SetActive(false);

        // 手动绑定Slider事件
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // 手动绑定返回按钮事件
        if (returnButton != null)
            returnButton.onClick.AddListener(OnResumeClicked);

        // 手动绑定设置按钮事件
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    void Update()
    {
        // 保留ESC键开关设置
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    // 音量控制
    public void OnVolumeChanged(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        UpdateVolumeDisplay(volume);
    }

    void UpdateVolumeDisplay(float volume)
    {
        volumeText.text = $"Volume: {Mathf.Round(volume * 100)}%";
    }

    // 设置界面开关
    public void ToggleSettings()
    {
        isPaused = !isPaused;
        settingsPanel.SetActive(isPaused);

        if (isPaused)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 返回按钮回调
    public void OnResumeClicked()
    {
        ToggleSettings(); // 复用开关方法
    }

    // 新增设置按钮回调
    public void OnSettingsButtonClicked()
    {
        ToggleSettings(); // 与ESC键相同功能
    }

    public void OnBrightnessChanged(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        UpdateBrightness(value);
    }

    void UpdateBrightness(float value)
    {
        brightnessText.text = $"Brightness: {Mathf.Round(value * 100)}%";
        brightnessOverlay.color = new Color(0, 0, 0, 1 - value); // 0=全黑，1=完全透明
    }

    // 图片大小改变回调
    public void OnSizeChanged(float newSize)
    {
        newSize = Mathf.Clamp(newSize, minSize, maxSize);
        PlayerPrefs.SetFloat("ImageSize", newSize);
        UpdateSizeDisplay(newSize);
        ApplyImageSize(newSize);
    }

    // 更新UI显示
    void UpdateSizeDisplay(float size)
    {
        sizeText.text = $"Subtitle Size: {size:F1}x";
    }

    // 应用大小到图片
    void ApplyImageSize(float size)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = new Vector3(size, size, 1f);
        }
    }

    // 预设大小按钮
    public void SetPresetSize(float size)
    {
        sizeSlider.value = size; // 这会自动触发OnSizeChanged
    }

}