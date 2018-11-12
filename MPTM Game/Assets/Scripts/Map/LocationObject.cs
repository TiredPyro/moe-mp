using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationObject : MonoBehaviour {
    //public Building thisBuilding;
    //public int buildingID; If made it so that it loads up the image of each building, but hardcoding it.
    public int buildingID; //Modified
    public Sprite mapSprite;
    public bool highlightedArea;

    public float topAlpha;
    public float bottomAlpha;
    bool reachedTop;

    public float blinkingSpeed = 5f;
    Color currentColor;
    float a;

    void Start()
    {
        GetComponent<Image>().sprite = mapSprite;
        currentColor = GetComponent<Image>().color;
        a = currentColor.a;
        reachedTop = false;

        //Modified
        if (CheckForQuestionsScript.instance.IsBuildingCleared(buildingID))
            highlightedArea = false;
    }

    void Update()
    {
        if (highlightedArea)
        {
            a = reachedTop ?  a - Mathf.Min(Time.deltaTime * blinkingSpeed, a - bottomAlpha) : a + Mathf.Min(Time.deltaTime * blinkingSpeed, topAlpha - a);
            currentColor = new Color(a, a, a, 1f);
            GetComponent<Image>().color = currentColor;
            if (a == topAlpha)
                reachedTop = !reachedTop;
            else if (a == bottomAlpha)
                reachedTop = !reachedTop;

        }
    }

}
