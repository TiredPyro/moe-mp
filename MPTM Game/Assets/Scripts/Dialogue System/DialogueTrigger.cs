using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool fixedDialogue; //TMight be a scene where there is a fixed dialogue 
    public Button triggerButton;

    void Start()
    {

        if (!fixedDialogue)
        {
            string[] example = new string[2];
            example[0] = "Hello";
            example[1] = "Hi";
            CreateDialogue(example);
        }
        else
            TriggerDialogue();

    }

    void CreateDialogue(string[] dialogueSentences)
    {
        /*
        dialogue = new Dialogue(); //This is where it creates a new dialogue. Needs to take note of the answers from the interviewee
        dialogue.sentences = new string[2];
        dialogue.sentences[0] = "Hello";
        dialogue.sentences[1] = "Bye";
        */
        dialogue = new Dialogue();
        dialogue.sentences = new string[dialogueSentences.Length];
        for (int i = 0; i < dialogueSentences.Length; i++)
        {
            dialogue.sentences[i] = dialogueSentences[i];
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if (triggerButton != null)
        {
            triggerButton.onClick.RemoveAllListeners();
            triggerButton.onClick.AddListener(DialogueManager.instance.DisplayNextSentence); //If it has a next button use this, if not then use input.getmousebuttondown
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DialogueManager.instance.DisplayNextSentence();
        }
    }
    /*
    void DebugStartDialogue()
    {
        if (Input.GetKeyDown("x"))
        {
            TriggerDialogue();
        }
    }

    private void Update()
    {
        DebugStartDialogue();
    }
    */
}