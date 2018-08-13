using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public AudioClip IntroClip;
    public AudioClip MainClip;

    private static MusicPlayer instance = null;
    public static MusicPlayer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartMain()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = MainClip;
        source.Stop();
        source.Play();
    }

    public void StartIntro()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = IntroClip;
        source.Stop();
        source.Play();
    }
}
