using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Text dialogueText;
    public float textDelay;
    public bool ActiveDialogue;
    public IEnumerator trigger;

    public string sentence;
    public Queue<string> sentences;
    [SerializeField] bool typewriterEffect;

    public FacialAnimations faceAnims;
    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void OnEnable()
    {
        SimpleEventManager.ContinueDialogue += DisplayNextSentence;
    }

    public void OnDisable()
    {
        SimpleEventManager.ContinueDialogue -= DisplayNextSentence;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        faceAnims = FindObjectOfType<FacialAnimations>();
        ActiveDialogue = true;
        //Debug.Log("Dialogue Start");
        sentences = new Queue<string>();
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        if (faceAnims != null)
        {
            faceAnims.PlayMouthAnimation();
        }
        sentence = sentences.Dequeue();

        if (typewriterEffect)
        {
            StopAllCoroutines();
            StartCoroutine(trigger = TypewriterEffect(sentence, textDelay));
        }
        else if (!typewriterEffect)
            dialogueText.text = sentence;

        if (sentences.Count == 0)
            ActiveDialogue = false;


    }

    void EndDialogue()
    {
        if (FindObjectOfType<DialogueTrigger>().fixedDialogue)
            FixedDialogueManager.instance.TransitionNextScene();
        Debug.Log("Dialogue End");
    }

    IEnumerator TypewriterEffect(string sentence, float speed)
    {
        for (int i = 0; i < sentence.ToCharArray().Length + 1; i++)
        {
            if (dialogueText.text == sentence)
                break;
            dialogueText.text = sentence.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }
        //dialogueText.text = "";
        //foreach (char chara in sentence.ToCharArray())
        //{
        //    yield return new WaitForSeconds(speed);
        //    dialogueText.text += chara;
        //    yield return null;
        //}
    }
}