using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationChoice : MonoBehaviour {
    public Question CurrentQuestion;
    public Button thisButton;
    public Text QuestionText;
    void Awake()
    {
        thisButton = GetComponent<Button>();
        QuestionText = GetComponentInChildren<Text>();
        thisButton.onClick.AddListener(ChangeBarValue);
    }
    
    void ChangeBarValue()
    {
        //Might add an if statement here, if this question has already been answered, the slider does not change the barmeter of both
        BarManager.instance.ChangeValue(CurrentQuestion.researchPoints, CurrentQuestion.infoPoints);
        ConversationManager.instance.questionAsked = CurrentQuestion;
    }

}
