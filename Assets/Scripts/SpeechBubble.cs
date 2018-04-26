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

    // Update is called once per frame
    void Update()
    {
        if (canvas.enabled)
        {
            canvas.transform.LookAt(-Camera.main.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !talking && InputManager.Interact())
        {
            Debug.Log("talking");
            StartCoroutine(Talk());
        }
    }

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

}// STina Hedman
