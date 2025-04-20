using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName = "Factory"; 

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
