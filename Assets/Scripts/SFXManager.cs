using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip gemBreakSFX;
    [SerializeField] AudioClip levelCompleteSFX;


    public static SFXManager instance;

    void Awake()
    {
        instance = this;
    }

    public void PlayGemBreak()
    {
        AudioSource.PlayClipAtPoint(gemBreakSFX, Camera.main.transform.position, 0.5f);
    }

    public void PlayExplosion()
    {       
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, 0.75f);
    }

    public void PlayLevelComplete()
    {
        AudioSource.PlayClipAtPoint(levelCompleteSFX, Camera.main.transform.position);
    }



}
