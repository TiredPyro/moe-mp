using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    public GameObject gCurrentYear, gNextYear;
    public float fNextSpeed;
    public int iStartYear;
    int iCurrentYear;
    bool bChangeComplete;

    Vector2 currentYearPos, nextYearPos;

    void Start()
    {
        InitYearValues();
    }

    void Update()
    {

    }

    void InitYearValues()
    {
        gCurrentYear.GetComponent<Animator>().SetTrigger("Grow");
        currentYearPos = gCurrentYear.transform.localPosition;
        nextYearPos = gNextYear.transform.localPosition;
        iCurrentYear = iStartYear;
        gCurrentYear.GetComponent<Text>().text = iCurrentYear.ToString();
        iCurrentYear++;
        gNextYear.GetComponent<Text>().text = iCurrentYear.ToString();
        bChangeComplete = true;
    }

    public void NextYear()
    {
        if (bChangeComplete)
        {
            gCurrentYear.GetComponent<Animator>().SetTrigger("Shrink");
            StartCoroutine(ChangeYear());
        }
    }

    IEnumerator ChangeYear()
    {
        bChangeComplete = false;
        GameObject temp = gNextYear;
        float newCurrentX = currentYearPos.x - 70.0f;
        float newNextX = nextYearPos.x - 70.0f;
        yield return new WaitForSeconds(0.25f);
        while (gCurrentYear.transform.localPosition.x != newCurrentX)
        {
            gCurrentYear.transform.localPosition = Vector2.MoveTowards(gCurrentYear.transform.localPosition, new Vector2(newCurrentX, currentYearPos.y), fNextSpeed);
            gNextYear.transform.localPosition = Vector2.MoveTowards(gNextYear.transform.localPosition, new Vector2(newNextX, nextYearPos.y), fNextSpeed);
            yield return null;
        }
        iCurrentYear++;
        gNextYear.GetComponent<Animator>().SetTrigger("Grow");
        gNextYear = gCurrentYear;
        gNextYear.transform.localPosition = nextYearPos;
        gNextYear.GetComponent<Text>().text = iCurrentYear.ToString();
        gCurrentYear = temp;
        yield return new WaitForSeconds(0.25f);
        bChangeComplete = true;
    }
}