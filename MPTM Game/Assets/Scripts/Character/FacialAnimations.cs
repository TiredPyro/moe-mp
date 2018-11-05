using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacialAnimations : MonoBehaviour
{
    //public SpriteRenderer brows;
    //public Sprite[] browsSprites;
    //public SpriteRenderer eyes;
    //public Sprite[] eyesSprites;
    Image mouth;
    public Sprite[] mouthSprites;

    public enum SpeakingType
    {
        FrontFacing, 
        TaxiDriver,
        Etc
    }
    public SpeakingType m_speak;

    public void PlayMouthAnimation()
    {
        switch (m_speak)
        {
            case SpeakingType.FrontFacing:
                StartCoroutine(FrontFacingMouthAnimation(1.0f));
                break;
            case SpeakingType.TaxiDriver:
                //Play taxi talking animation
                break;
        }
    }

    public IEnumerator FrontFacingMouthAnimation(float duration)
    {
        mouth = GetComponent<Image>();
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return new WaitForSeconds(0.15f);
            duration -= 0.15f;
            mouth.sprite = mouthSprites[Random.Range(1, mouthSprites.Length)];
        }
        mouth.sprite = mouthSprites[0];
    }
}