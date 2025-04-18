using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;  // ������壨Ĭ�����أ�
    public Slider volumeSlider;
    public TMP_Text volumeText;
    public AudioSource backgroundMusic;
    public Button returnButton;      // ���ذ�ť������������ڣ�
    public Button settingsButton;    // ���ð�ť����פUI��Ĭ�Ͽɼ���

    private bool isPaused = false;
    private float prevTimeScale;

    void Start()
    {
        // ��ʼ������
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        UpdateVolumeDisplay(volumeSlider.value);

        // Ĭ��״̬����
        settingsPanel.SetActive(false);    // �������Ĭ������
        settingsButton.gameObject.SetActive(true); // ���ð�ťĬ����ʾ

        // �ֶ�������UI�¼�
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        returnButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    void Update()
    {
        // ESC���������ã�����ԭ�й��ܣ�
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

            // ���������ʱ�������ð�ť����ѡ��
            settingsButton.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // �ر��������ʱ��ʾ���ð�ť
            settingsButton.gameObject.SetActive(true);
        }
    }

    // ���ذ�ť�ص�
    public void OnResumeClicked()
    {
        ToggleSettings();
    }

    // ���ð�ť�ص�
    public void OnSettingsButtonClicked()
    {
        ToggleSettings();
    }
}