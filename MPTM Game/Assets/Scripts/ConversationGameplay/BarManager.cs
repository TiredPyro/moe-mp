using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    //public Transform researchBar, infoBar;
    public Image researchFill, infoFill;


    public BarMeter2 researchBar, infoBar;
    public float max;

    public static BarManager instance;

    void Start()
    {

        instance = this;

        //researchFill = researchBar.GetChild(0).GetComponent<Image>();
        //infoFill = infoBar.GetChild(0).GetComponent<Image>();

        max = researchFill.rectTransform.sizeDelta.x;

        researchBar = new BarMeter2(researchFill, 0f, 0f); 
        infoBar = new BarMeter2(infoFill, 0f, 0f);

        ClearBarValue(researchBar); // testing
        ClearBarValue(infoBar); // testing
        UpdateBarValue(researchBar, PlayerProperties.ResearchSkillPoints); //Is by 0-100 now, meaning is by percentage increase or decrease. Also, set the static variables from the PlayerProperties here
        UpdateBarValue(infoBar, PlayerProperties.RequiredInfoPoints);


    }

    void Update()
    {
        UpdateBar(researchBar);
        UpdateBar(infoBar);

    }

    void ClearBarValue(BarMeter2 barFill)
    {
        float y = barFill.barMeter.rectTransform.sizeDelta.y;
        barFill.barMeter.rectTransform.sizeDelta = new Vector2(0.0f, y);
    }



    public void ChangeValue(float researchPercentage, float infoPercentage)
    {
        UpdateBarValue(researchBar, researchPercentage);
        UpdateBarValue(infoBar, infoPercentage);
    }


    public void UpdateBarValue(BarMeter2 barFill, float valueIncrease)
    {
        //float x = Mathf.Min(barFill.barMeter.rectTransform.sizeDelta.x + percentageValue * max, max);
        //float y = barFill.barMeter.rectTransform.sizeDelta.y;

        //barFill.rectTransform.sizeDelta = new Vector2(x, y);
        //Since we're going by number etc 5 increase 5% of the meter
        float calChange = (valueIncrease / 100f) * max;

        barFill.endValue = Mathf.Clamp(barFill.initialValue + (calChange), 0f, max);

        barFill.initialValue = barFill.endValue;
    }

    void UpdateBar(BarMeter2 barFill)
    {

        if (barFill.barMeter.rectTransform.sizeDelta.x < barFill.endValue)
        {
            float x = (barFill.barMeter.rectTransform.sizeDelta.x + Mathf.Min(barFill.endValue - barFill.barMeter.rectTransform.sizeDelta.x, Time.deltaTime * 500f));
            float y = barFill.barMeter.rectTransform.sizeDelta.y;
            barFill.barMeter.rectTransform.sizeDelta = new Vector2(x, y);
        }

        else if (barFill.barMeter.rectTransform.sizeDelta.x > barFill.endValue)
        {
            float x = (barFill.barMeter.rectTransform.sizeDelta.x - Mathf.Min(barFill.barMeter.rectTransform.sizeDelta.x - barFill.endValue, Time.deltaTime * 500f));
            float y = barFill.barMeter.rectTransform.sizeDelta.y;
            barFill.barMeter.rectTransform.sizeDelta = new Vector2(x, y);
        }
    }
}