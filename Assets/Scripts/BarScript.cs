using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private Image barImage;

    [SerializeField]
    private Text valueText;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private bool lerpColor;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            fillAmount = barCalc(value, 0, MaxValue, 0, 1);
        }
    }

    // Use this for initialization
    void Start ()
    {
		if (lerpColor)
        {
            barImage.color = fullColor;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        HandleBar();
	}

    //Ser till att mätaren uppdateras om den inte redan är korrekt.
    private void HandleBar()
    {
        if (fillAmount != barImage.fillAmount)
        {
            barImage.fillAmount = Mathf.Lerp(barImage.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }

        if (lerpColor)
        {
            barImage.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
        
        
    }

    //Uträkning för att få currentHealth till ett värde mellan minBar(0) och maxBar(1)
    private float barCalc(float currentHealth, float minHealth, float maxHealth, float minBar, float maxBar)
    {
        return (currentHealth - minHealth) * (maxBar - minBar) / (maxHealth - minHealth) + minBar;
    }

}
