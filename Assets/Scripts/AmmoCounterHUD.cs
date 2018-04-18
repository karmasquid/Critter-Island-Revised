using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterHUD : MonoBehaviour {

    private Text ammoCounter;

    // Use this for initialization
    private void Awake()
    {
        ammoCounter = gameObject.GetComponent<Text>();
    }

    void Start () {
        ammoCounter.text = "25";
	}
	
	// Update is called once per frame
	public void UpdateAmmoCounter( int value)
    {
        ammoCounter.text = value.ToString();
	}
}
