using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance = null;

    private FMOD.Studio.EventInstance musicIntro;
    private FMOD.Studio.EventInstance musicThemeOne;
    private FMOD.Studio.EventInstance musicThemeTwo;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        MusicStart();
    }

    public void MusicStart()
    {
        MusicStop();
        FMOD.Studio.PLAYBACK_STATE _musicIntro;
        musicIntro.getPlaybackState(out _musicIntro);
        if (_musicIntro != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            musicIntro = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.MUSIC_INTRO);
            musicIntro.start();
        }
    }

    public void MusicOne()
    {
        MusicStop();
        musicThemeOne = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.MUSIC_MAIN_THEME);
        musicThemeOne.start();
    }

    public void MusicTwo()
    {
        MusicStop();
        musicThemeTwo = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.MUSIC_SECOND_THEME);
        musicThemeTwo.start();
    }

    public void MusicStop()
    {

        FMOD.Studio.Bus musicBus = FMODUnity.RuntimeManager.GetBus("bus:/music");
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
