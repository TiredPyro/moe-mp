using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventManager : MonoBehaviour {
    public delegate void ChangeConversationState(); //This will handle the changing of states, and function when changing states

    public static event ChangeConversationState ChangeState;
    public static event ChangeConversationState ContinueDialogue;

    public static event ChangeConversationState FixedDialogue;

    //public delegate void LoadingChosenMap();
    //public static event LoadingChosenMap LoadUpMap;

    public void TriggerChangeState()
    {
        if (ChangeState != null)
        {
            if (ContinueDialogue != null && DialogueManager.instance.ActiveDialogue == true)
            {
                ContinueDialogue();
            }
            else
            {
                ChangeState();
                Debug.Log("changing state event");
            }
        }
    }
    /*
    public void ChosenCurrentMapID()
    {
        if (LoadUpMap != null)
            LoadUpMap();
    }

    */
    /*
    public void TriggerFixedDialogue()
    {
        if (FixedDialogue != null)
            FixedDialogue();
    }
    */
}
