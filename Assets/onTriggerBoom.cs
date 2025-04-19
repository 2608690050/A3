using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class onTriggerBoom : MonoBehaviour
{
    Animator anim;
    AudioSource audio;
    public Image blackScreen;
    private void Start()
    {
        anim = this.GetComponent<Animator>();
        audio = this.GetComponent<AudioSource>();
        this.GetComponent<ColliderTriggerEvent>().triggerEventDo = TriggerCheck;
    }
    bool firstTrigger;
    void TriggerCheck(Collider collider) 
    {
        if (!firstTrigger&&collider.CompareTag("Player"))
        {
            firstTrigger = true;
            anim.SetTrigger("Play");
        }
    }
    void ToBlack()
    {

        SceneManager.LoadScene("end");
    }
    public void PlayAudio() 
    {
        audio.Play();
    }
}
