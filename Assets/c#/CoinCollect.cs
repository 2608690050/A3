using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public AudioClip collectSound;  // 收集音效（可在Inspector中拖入）
    private AudioSource audioSource;

    void Start()
    {
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = collectSound;
    }

    // 当玩家触碰金币时触发
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保玩家有"Player"标签
        {
            // 播放音效
            if (collectSound != null)
            {
                audioSource.Play();
            }

            // 禁用碰撞体和渲染（让金币“消失”）
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            // 延迟销毁对象（确保音效播放完毕）
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
