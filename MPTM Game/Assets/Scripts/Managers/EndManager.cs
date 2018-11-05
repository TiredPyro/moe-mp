using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoBehaviour {
    public Text text, retryText, otherText;
	// Use this for initialization
	void Start () {
        text.text = PlayerProperties.choseCorrectDecision ?
            "You did it right!" :
            "You did it wrong...";
        retryText.text = Economy.mEconomyState == Economy.EconomyState.GlobalRecession ?
            "Retry Scenario (Global Recession)" :
            "Retry Scenario (Overheating)";
        otherText.text = Economy.mEconomyState == Economy.EconomyState.GlobalRecession ?
            "Other Scenario (Overheating)" :
            "Other Scenario (Global Recession)";
    }
    public void Retry()
    {
        PlayerProperties.Reset();
        SceneTransitionManager.StartTransition("Map");
    }
    public void Other()
    {
        PlayerProperties.Reset();
        Economy.mEconomyState = Economy.mEconomyState == Economy.EconomyState.GlobalRecession ?
            Economy.EconomyState.Overheating :
            Economy.EconomyState.GlobalRecession;
        SceneTransitionManager.StartTransition("Map");
    }
}
