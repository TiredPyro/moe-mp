using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSNEERManager : MonoBehaviour {
    public static SSNEERManager instance;

    private void Start()
    {
        instance = this;
    }
    public void OnClickHigher()
    { 
        Economy.mNEERState = Economy.NEERState.Stronger;
        ResetAttributes();
    }

    public void OnClickLower()
    {
        Economy.mNEERState = Economy.NEERState.Weaker;
        ResetAttributes();
    }

    void ResetAttributes()
    {
        PlayerProperties.RequiredInfoPoints = 0;
        PlayerProperties.ResearchSkillPoints = 0;
        PlayerProperties.QuestionsAsked.Clear();

        ChangeGameStage();

        SceneTransitionManager.StartTransition("Map");
    }

    public void ChangeGameStage()
    {
        switch(Economy.mStage)
        {
            case Economy.GameStage.Stage1:
                Economy.mStage = Economy.GameStage.Stage2;
                break;
            case Economy.GameStage.Stage2:
                Economy.mStage = Economy.GameStage.Stage3;
                break;
            case Economy.GameStage.Stage3:
                break;
        }
    }

}
