using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    [SerializeField]
    public List<string> sentences = new List<string>();

    public float secWaitBetween;

    public Image image;
    public Text text;
    public Canvas canvas;
    private int index = 0;
    private bool talking;

    // Use this for initialization
    void Start()
    {
        //reference to image from resources.
        Debug.Log("hey");
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !talking)
        {

            CreateTextBubble();

        }
    }

    private void CreateTextBubble()
    {
        talking = true;

        if (sentences.Count >= index + 1)
        {
            canvas.enabled = true;
            text.text = sentences[index];
            index++;
            StartCoroutine(WaitForNextText());
        }
        else
        {
            canvas.enabled = false;
        }
    }

    private IEnumerator WaitForNextText()
    {
        yield return new WaitForSeconds(secWaitBetween);
        CreateTextBubble();
        yield break;
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    canvas.enabled = false;
        //    index = 0;
        //}
    }
}
