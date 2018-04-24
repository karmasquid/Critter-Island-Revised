using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Initialize()
    {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}
