using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTextbox : MonoBehaviour {

    public TextboxState m_current;
    public enum TextboxState
    {
        HIDDEN,
        INCREASING,
        STAYING,
        DECREASING
    }
    float initialElapsed;
    public float elapsed;

    float scale;

    private void Start()
    {
        initialElapsed = elapsed;
        m_current = TextboxState.HIDDEN;

        scale = 0.0f;
    }
    void Update()
    {
        switch (m_current)
        {
            case TextboxState.HIDDEN:
                ElapsedTime(TextboxState.INCREASING);
                break;

            case TextboxState.INCREASING:
                if (scale <= 1.0f)
                    IncreaseSize();
                break;

            case TextboxState.STAYING:
                ElapsedTime(TextboxState.DECREASING);
                break;
                
            case TextboxState.DECREASING:
                if (scale >= 0.0f)
                    DecreaseSize();
                break;
        }

    }

    void ElapsedTime(TextboxState changeState)
    {
        elapsed -= Mathf.Min(elapsed, Time.deltaTime);
        if (elapsed == 0)
        {
            elapsed = initialElapsed;
            m_current = changeState;
        }
    }

    public void IncreaseSize()
    {
        scale += Mathf.Min(1.0f - scale,Time.deltaTime/5f);
        gameObject.transform.localScale = new Vector2(scale, scale);
        if (scale == 1)
        {
            m_current = TextboxState.STAYING;
        }
    }

    public void DecreaseSize()
    {
        scale -= Mathf.Min(scale, Time.deltaTime / 5f);
        gameObject.transform.localScale = new Vector2(scale, scale);
        if (scale == 0)
        {
            m_current = TextboxState.HIDDEN;
        }
    }

}
