using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource efxSource;

    public AudioClip MusicClip;

    //public Slider PauseMscVolSlider;
    //public Slider PauseEfxVolSlicer;
    //public Slider MainMscSlider;
    //public Slider MainEfxVolSlider;

    private float lowPitchLimit = 0.95f;
    private float highPitchLimit = 1.05f;

    public static SoundManager instance;

    public AudioClip musicClip
    {
        set { if (value != MusicClip) PlayMusic(value); this.MusicClip = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //if (SceneManager.GetActiveScene().name == "Andres_SceneDesignArea")
        //{
        //    PlayMusic(MusicClip);
        //}
    }

    // used to play single soundclips.
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;

        efxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // used to play random clips at somewhat random pitch. <------------------- use for walking?!
    public void PlayRandom(params AudioClip[] clips)
    {
        //chose random clip in range.
        int randomIndex = Random.Range(0, clips.Length);

        //set varied pitch of sound.
        float randomPitch = Random.Range(lowPitchLimit, highPitchLimit);

        efxSource.pitch = randomPitch;

        //play clip
        efxSource.Play();

    }

    // invoked when slider button is clicked.
    public void ChangeMusicVol(int value)
    {
        musicSource.volume = value;

        //PauseMscVolSlider.value = musicSource.volume;
        //MainMscSlider.value = musicSource.volume;
    }

    // invoke when slider button is clicked
    public void ChangeEfxVol(int value)
    {
        efxSource.volume = value;

        //PauseEfxVolSlicer.value = efxSource.volume;
        //MainEfxVolSlider.value = efxSource.volume;
    }

}

// Stina Hedman

