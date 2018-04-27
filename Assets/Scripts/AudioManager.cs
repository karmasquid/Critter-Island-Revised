using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    //Script taget från Brackey: https://www.youtube.com/watch?v=6OT43pvUyfY
    //Skapar ljudkällor av klassen Sound
    public Sound[] sounds;

    public static AudioManager instance;

	void Awake ()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //Står här då vi längre fram ska ha en ljudhanterare som följer med mellan senare och tonar ned musik och bakgrundsljud.
        //Just nu har vi en ny ljudhanterare i varje scen.
        //DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    void Start()
    {
        Play("MainMusic");

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        //Kodrader som senare ska användas för att spela upp och stänga av ljud på relevant plats.
        //Skriv detta för att spela ljud på relevant ställe
        //FindObjectOfType<AudioManager>().Play("SoundName");
        //Skriv detta för att stoppa ljud(musik)
        //FindObjectOfType<AudioManager> ().Stop("SoundName");

    }


}
//Daniel Laggar