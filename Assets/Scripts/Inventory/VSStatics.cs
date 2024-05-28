using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSStatics
{
    public string Name;
    public string Unit;
    public float Rate;

    public VSStatics(string name, string unit, float rate)
    {
        this.Name = name;
        this.Unit = unit;
        this.Rate = rate;
    }
}
