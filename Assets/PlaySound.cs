using UnityEngine.Audio;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    AudioManager a;
    //Note to self: Fixa coroutine för att fade:a mella låtar. Använd check för att se vilken låt som spelas.
    private void Awake()
    {
        a = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public string MusicToPlay;
    public string MusicToStop;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            a.Stop(MusicToStop);
            a.Play(MusicToPlay);
            Destroy(gameObject);
        }
    }




}
