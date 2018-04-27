using UnityEngine.Audio;
using UnityEngine;

//Script taget från Brackey https://www.youtube.com/watch?v=6OT43pvUyfY
//Klass som innehåller en ljudfil med ett namn, en volym, en pitch och en tilldelad mixerkanal. Används av scriptet AudioManager för att skapa ljudokällor.
[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    public AudioMixerGroup output;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
//Daniel Laggar