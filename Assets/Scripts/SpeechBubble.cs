using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    [SerializeField]
    public List<string> sentences = new List<string>();

    public float secWaitBetween;

    public Sprite sprite;
    public Text text;
    public Canvas canvas;
    private int index = 0;
    private bool talking;

    // Use this for initialization
    void Start()
    {
        //reference to image from resources.
        canvas.enabled = false;
    }

    // Saved as it might be used later to turn the canvas towards the camera, as it might have different angles later on in game.
    //void Update()
    //{
    //    if (canvas.enabled)
    //    {
    //        canvas.transform.LookAt(-Camera.main.transform.position);
    //    }
    //}

    //used for the player to be able of interacting and starting the conversation.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !talking && InputManager.Interact())
        {
            StartCoroutine(Talk());
        }
    }

    //used to display the sentences written in the inspector.
    private IEnumerator Talk()
    {
        talking = true;


        for (int i = 0; i < sentences.Count; i++)
        {
            canvas.enabled = true;

            text.text = sentences[i];

            yield return new WaitForSeconds(secWaitBetween);
        }

        canvas.enabled = false;
        talking = false;
        index = 0;

        yield break;
    }

}// Stina Hedman
