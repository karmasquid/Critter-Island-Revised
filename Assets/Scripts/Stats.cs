using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//En klass för att hantera spelar- och fiendeattribut. För att skapa stats initialiserar mán ett nytt önskat attribut.
//Detta görs i scripten EnemyStats och PlayerManager.
[Serializable]
public class Stats {

    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxValue;

    [SerializeField]
    private float currentValue;

    public float CurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            currentValue = Mathf.Clamp(value,0,MaxValue);
            bar.Value = currentValue;       
        }
    }

    public float MaxValue
    {
        get
        {
            return maxValue;
        }

        set
        {
            bar.MaxValue = value;
            maxValue = value;
        }
    }

    public BarScript Bar
    {
        set
        {
            bar = value;
        }
    }

    //Används för att skapa ett nytt attribut
    public void Initialize()
    {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}
//Daniel Laggar