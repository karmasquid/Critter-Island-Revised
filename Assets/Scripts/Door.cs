using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    
    bool GoldKey;
    bool SilverKey;
    bool BronzeKey;

    string keyNeeded;

    // Use this for initialization
    void Start()
    {
        if (GoldKey)
        {
            keyNeeded = "Gold Key";
        }
        else if (SilverKey)
        {
            keyNeeded = "Silver Key";
        }
        else if (BronzeKey)
        {
            keyNeeded = "Bronze Key";
        }
    }
}
