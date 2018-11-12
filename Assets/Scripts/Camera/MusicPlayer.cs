using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public AudioClip IntroClip;
    public AudioClip[] AudioClips;

    private int m_currentClip = 0;

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
        source.clip = AudioClips[0];
        source.Stop();
        source.Play();


    }

    IEnumerator PlayNext()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = AudioClips[m_currentClip];
        source.Stop();
        source.Play();

        m_currentClip += m_currentClip % AudioClips.Length;

        yield return new WaitForSeconds(source.clip.length);

        PlayNext();
    }

    public void StartIntro()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = IntroClip;
        source.Stop();
        source.Play();
    }
}
