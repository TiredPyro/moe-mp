using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedDialogueManager : MonoBehaviour {
    public int MapID;
    public static FixedDialogueManager instance;
    public string nextScene;
    // Use this for initialization

    void Awake()
    {
        PlayerProperties.CurrentMap = MapID; //This might be set manually
        instance = this;
       
    }

    private void OnEnable()
    {
        SimpleEventManager.FixedDialogue += TransitionNextScene;
    }

    private void OnDisable()
    {
        SimpleEventManager.FixedDialogue -= TransitionNextScene;
    }

    public void TransitionNextScene()
    {

        SceneTransitionManager.StartTransition(nextScene);
    }
}
