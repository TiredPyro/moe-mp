using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorManager : MonoBehaviour {
    Camera cam;
    Color currentColor;
    float timerChanger;
    IEnumerator colorChanger;
    public Color changeTo;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        currentColor = cam.backgroundColor;
        timerChanger = 1f;
        colorChanger = changeToColor(changeTo);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            colorChanger = changeToColor(changeTo);
            StartCoroutine(colorChanger);
        }
    }

    IEnumerator changeToColor(Color color)
    {

        while (!(cam.backgroundColor == color))
        {
            CloudManager.instance.speeding = true;
            timerChanger -= Time.deltaTime;
            cam.backgroundColor = Color.Lerp(color, currentColor, timerChanger);
            if (timerChanger <= 0)
            {
                timerChanger = 1f;
                currentColor = cam.backgroundColor;

                //Allows it to change back to a certain color
                Debug.Log("Changed, done");
                CloudManager.instance.speeding = false;
                StopCoroutine(colorChanger);
            }
            yield return null;
        }
    }
}
