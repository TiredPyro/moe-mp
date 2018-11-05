using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmManager : MonoBehaviour {

    public void Yes()
    {
        PlayerProperties.choseCorrectDecision = Check();
        SceneTransitionManager.StartTransition("Puzzle");
    }

    public void No()
    {
        PlayerProperties.choseCorrectDecision = !Check();
        SceneTransitionManager.StartTransition("Puzzle");
    }

    bool Check()
    {
        if (Economy.mEconomyState == Economy.EconomyState.Overheating)
        {
            if (Economy.mNEERState == Economy.NEERState.Stronger)
                return true;
            else
                return false;


        }
        else
        {
            if (Economy.mNEERState == Economy.NEERState.Stronger)
                return false;
            else
                return true;
        }

    }
}
