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
    public Button returnButton;    // �������ذ�ť����
    public Button settingsButton;  // �������ð�ť����

    [Header("Graphics Settings")]
    public Slider brightnessSlider;
    public TMP_Text brightnessText;
    public Image brightnessOverlay; // ��͸����ɫUIͼƬ������ȫ��

    [Header("Image Size Settings")]
    public Slider sizeSlider;          // ͼƬ��С����
    public TMP_Text sizeText;          // ��ʾ��ǰ��С���ı�
    public Image targetImage;          // Ҫ������С��ͼƬ
    public Button smallSizeBtn;        // С�ߴ�Ԥ�谴ť
    public Button mediumSizeBtn;       // �гߴ�Ԥ�谴ť
    public Button largeSizeBtn;        // ��ߴ�Ԥ�谴ť

    [Range(0.1f, 2f)] public float minSize = 0.5f;
    [Range(2f, 5f)] public float maxSize = 3f;
    private float defaultSize = 1f;

    private bool isPaused = false;
    private float prevTimeScale;

    void Start()
    {
        // ��ʼ��ͼƬ��С����
        sizeSlider.minValue = minSize;
        sizeSlider.maxValue = maxSize;
        sizeSlider.value = PlayerPrefs.GetFloat("ImageSize", defaultSize);
        UpdateSizeDisplay(sizeSlider.value);
        sizeSlider.onValueChanged.AddListener(OnSizeChanged);

        // ��Ԥ�谴ť
        smallSizeBtn.onClick.AddListener(() => SetPresetSize(0.5f));
        mediumSizeBtn.onClick.AddListener(() => SetPresetSize(1f));
        largeSizeBtn.onClick.AddListener(() => SetPresetSize(1.5f));

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
        UpdateBrightness(brightnessSlider.value);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        // ��ʼ������
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        UpdateVolumeDisplay(volumeSlider.value);

        // Ĭ���������ý���
        settingsPanel.SetActive(false);

        // �ֶ���Slider�¼�
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // �ֶ��󶨷��ذ�ť�¼�
        if (returnButton != null)
            returnButton.onClick.AddListener(OnResumeClicked);

        // �ֶ������ð�ť�¼�
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    void Update()
    {
        // ����ESC����������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    // ��������
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

    // ���ý��濪��
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

    // ���ذ�ť�ص�
    public void OnResumeClicked()
    {
        ToggleSettings(); // ���ÿ��ط���
    }

    // �������ð�ť�ص�
    public void OnSettingsButtonClicked()
    {
        ToggleSettings(); // ��ESC����ͬ����
    }

    public void OnBrightnessChanged(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        UpdateBrightness(value);
    }

    void UpdateBrightness(float value)
    {
        brightnessText.text = $"Brightness: {Mathf.Round(value * 100)}%";
        brightnessOverlay.color = new Color(0, 0, 0, 1 - value); // 0=ȫ�ڣ�1=��ȫ͸��
    }

    // ͼƬ��С�ı�ص�
    public void OnSizeChanged(float newSize)
    {
        newSize = Mathf.Clamp(newSize, minSize, maxSize);
        PlayerPrefs.SetFloat("ImageSize", newSize);
        UpdateSizeDisplay(newSize);
        ApplyImageSize(newSize);
    }

    // ����UI��ʾ
    void UpdateSizeDisplay(float size)
    {
        sizeText.text = $"Subtitle Size: {size:F1}x";
    }

    // Ӧ�ô�С��ͼƬ
    void ApplyImageSize(float size)
    {
        if (targetImage != null)
        {
            targetImage.rectTransform.localScale = new Vector3(size, size, 1f);
        }
    }

    // Ԥ���С��ť
    public void SetPresetSize(float size)
    {
        sizeSlider.value = size; // ����Զ�����OnSizeChanged
    }

}