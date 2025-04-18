using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;  // 设置面板（默认隐藏）
    public Slider volumeSlider;
    public TMP_Text volumeText;
    public AudioSource backgroundMusic;
    public Button returnButton;      // 返回按钮（在设置面板内）
    public Button settingsButton;    // 设置按钮（常驻UI，默认可见）

    private bool isPaused = false;
    private float prevTimeScale;

    void Start()
    {
        // 初始化音量
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        UpdateVolumeDisplay(volumeSlider.value);

        // 默认状态设置
        settingsPanel.SetActive(false);    // 设置面板默认隐藏
        settingsButton.gameObject.SetActive(true); // 设置按钮默认显示

        // 手动绑定所有UI事件
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        returnButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    void Update()
    {
        // ESC键开关设置（保持原有功能）
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

            // 打开设置面板时隐藏设置按钮（可选）
            settingsButton.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 关闭设置面板时显示设置按钮
            settingsButton.gameObject.SetActive(true);
        }
    }

    // 返回按钮回调
    public void OnResumeClicked()
    {
        ToggleSettings();
    }

    // 设置按钮回调
    public void OnSettingsButtonClicked()
    {
        ToggleSettings();
    }
}