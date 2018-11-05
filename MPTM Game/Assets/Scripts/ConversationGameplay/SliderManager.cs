using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderManager : MonoBehaviour
{
    public static SliderManager instance;
    public Scrollbar research;
    public Scrollbar info;

    BarMeter researchBarmeter;
    BarMeter infoBarmeter;
    void Start()
    {
        researchBarmeter = new BarMeter(research, 0.0f, 0.0f); //At this part here, I'll take reference to a static class data script which stores all the stuff
        infoBarmeter = new BarMeter(info,0.0f,0.0f);
        instance = this;
        if (instance != this)
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //Just for debugging, If the person press a correct sequence, it would give a positive value or negative value
        {
            changeMeterValue(researchBarmeter, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            changeMeterValue(infoBarmeter, 0.5f);
        }
        UpdateMeterValue(researchBarmeter);
        UpdateMeterValue(infoBarmeter);

    }

    public void ChangeValue(float researchChange, float infoChange)
    {
        /*
        //Take in research and skill value
        float researchChangeValue;
        float infoChangeValue;

        //changeMeterValue(researchBarmeter, valueOfResearch);
        //changeMeterValue(infoBarmeter, valueOfInfo);
        */

        changeMeterValue(researchBarmeter, researchChange); 
        changeMeterValue(infoBarmeter, infoChange);
    }

    void changeMeterValue(BarMeter bar, float value)
    {
        bar.endValue = Mathf.Clamp(bar.initialValue + value, 0.0f, 1.0f);

        bar.initialValue = bar.endValue;
    }

    void UpdateMeterValue(BarMeter bar)
    {
        if (bar.barMeter.size < bar.endValue)
        {
            bar.barMeter.size += Mathf.Min(bar.endValue - bar.barMeter.size, Time.deltaTime);
        }
        else if (bar.barMeter.size > bar.endValue)
        {
            bar.barMeter.size -= Mathf.Min(-(bar.endValue - bar.barMeter.size), Time.deltaTime);
        }
    }
}
