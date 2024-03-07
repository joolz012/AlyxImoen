using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioSource bgMusic;
    public AudioSource audioSources;
    public AudioClip bdayBgm;
    // Start is called before the first frame update
    void Start()
    {
        bgMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BdayBgm()
    {
        bgMusic.Stop();
        audioSources.PlayOneShot(bdayBgm);
    }
}
