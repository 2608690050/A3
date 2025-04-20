using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void LoadMyScene(string sceneName)
    {
        
        StartCoroutine("PlayMySound", sceneName);
    }

    IEnumerator PlayMySound(string SceneName)
    {
        yield return new WaitForSeconds(3);
       SceneManager.LoadScene(SceneName);

    }
    public void LoadMyScene(int sceneNumber)
    {
        
         StartCoroutine("PlayMySound2", sceneNumber);
    }

    IEnumerator PlayMySound2(int SceneNumber)
    {
        yield return new WaitForSeconds(0);
       SceneManager.LoadScene(SceneNumber);

    }

}

