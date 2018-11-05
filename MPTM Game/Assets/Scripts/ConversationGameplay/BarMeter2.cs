using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarMeter2 {

    public Image barMeter;
    public float initialValue;
    public float endValue;

    public BarMeter2(Image _bar, float _init, float _end)
    {
        this.barMeter = _bar;
        this.initialValue = _init;
        this.endValue = _end;
    }
}
