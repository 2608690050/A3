using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public AudioClip collectSound;  // �ռ���Ч������Inspector�����룩
    private AudioSource audioSource;

    void Start()
    {
        // ��ȡ�����AudioSource���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = collectSound;
    }

    // ����Ҵ������ʱ����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ȷ�������"Player"��ǩ
        {
            // ������Ч
            if (collectSound != null)
            {
                audioSource.Play();
            }

            // ������ײ�����Ⱦ���ý�ҡ���ʧ����
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            // �ӳ����ٶ���ȷ����Ч������ϣ�
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
