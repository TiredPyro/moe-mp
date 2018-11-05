using UnityEngine.UI;

public class BarMeter {
    public Scrollbar barMeter;
    public float initialValue;
    public float endValue;

    public BarMeter(Scrollbar _bar, float _init, float _end)
    {
        this.barMeter = _bar;
        this.initialValue = _init;
        this.endValue = _end;
    }
}


