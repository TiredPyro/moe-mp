using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public Image UI_Phone;
    public GameObject UI_Phone_Text;
    [SerializeField] GameObject text;

    bool phoneActivated = false;
    bool phoneTransitioning = false; // feel free to change the naming

    Vector2 initialPhonePosition;
    public float UIPhoneSpeed = 1.0f;

    public Text textMesh;

    string finalString;

    private void Awake()
    {
        //I might hardcode the different topics like this
        //Topic.topics.Add(new Topic("GDP",null));
        //Topic.topics.Add(new Topic("AnotherTopic", null));
        phoneActivated = false;
        initialPhonePosition = UI_Phone.transform.localPosition;
    }

    void Start()
    {
        //debugTestQuestion();
        CheckAskedQuestion();

        int lineCount = 0; // added by Yumi

        foreach (Topic topic in Topic.topics)
        {
            foreach (string info in topic.topicInfo)
            {
                topic.topicString += "-" + info + "\n\n";
                lineCount+=2; // added by Yumi
                if (info.Length > 59) // for font size 50, more than 59 characters will go to a new line
                {
                    lineCount++;
                }
            }

            finalString += "\n"
                + topic.topicName + ": "
                + "\n"
                + topic.topicString;

            lineCount += 3; // added by Yumi
        }

        textMesh.text = finalString;
        if (finalString == null)
        {
            textMesh.text = "\nAfter an interview, you can read what information you've collected here.";
        }

        if (lineCount > 0)
        {
            textMesh.GetComponent<RectTransform>().sizeDelta = new Vector2(1270.0f, 54.0f * lineCount); // added by Yumi

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !phoneTransitioning)
            StartCoroutine(MovePhone());
    }

    IEnumerator MovePhone()
    {
        text.SetActive(false);
        phoneTransitioning = true;
        if (phoneActivated == true)
        {
            UI_Phone_Text.SetActive(!phoneActivated);
            UI_Phone.transform.GetChild(0).GetComponent<Animator>().SetBool("Shrink", phoneActivated);
            yield return new WaitForSeconds(0.25f);
        }

        Vector2 startPosition = phoneActivated ? Vector2.zero : initialPhonePosition;
        Vector2 newPosition = phoneActivated ? initialPhonePosition : Vector2.zero;
        Vector2 scaleSmall = Vector2.one * 0.15f;
        Vector2 scaleBig = Vector2.one;
        float angle = -90.0f;

        float temp = 0.0f;
        while (temp < 1.0f)
        {
            UI_Phone.transform.localPosition = Vector2.Lerp(startPosition, newPosition, temp); // phone movement
            UI_Phone.transform.localScale = Vector2.Lerp(phoneActivated ? scaleBig : scaleSmall, // phone size
                                                         phoneActivated ? scaleSmall : scaleBig,
                                                         temp);
            UI_Phone.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(phoneActivated ? 0.0f : angle, // phone rotation
                                                                                phoneActivated ? angle : 0.0f,
                                                                                temp));
            temp += Time.deltaTime * UIPhoneSpeed;
            if (temp > 1.0f)
            {
                UI_Phone.transform.localPosition = newPosition;
                UI_Phone.transform.localScale = phoneActivated ? scaleSmall : scaleBig;
                UI_Phone.transform.eulerAngles = new Vector3(0.0f, 0.0f, phoneActivated ? angle : 0.0f);
            }
            yield return null;
        }

        if (phoneActivated == false)
        {
            UI_Phone.transform.GetChild(0).GetComponent<Animator>().SetBool("Shrink", phoneActivated);
            yield return new WaitForSeconds(0.25f);
            UI_Phone_Text.SetActive(!phoneActivated);
        }
        phoneActivated = !phoneActivated;
        phoneTransitioning = false;
    }

    void CheckAskedQuestion()
    {
        Topic.topics.Clear();
        //Just for readjusting, deleting all the topics
        if (PlayerProperties.QuestionsAsked.Count == 0)
            return;
       
        List<List<string>> info = new List<List<string>>();
        foreach(Question question in PlayerProperties.QuestionsAsked)
        {
            if (question.topic == null)
                continue;
            if (Topic.topics.Count == 0) //If it's the first one, create a new one
            {
                info.Add(new List<string>());
                Debug.Log("Topic list is empty, adding a new one in");
                Topic.topics.Add(new Topic(question.topic, info[0]));
                info[0].Add(question.GetFormattedAnswer()); //For now just add in the answer
            }

            else
            {
                Debug.Log("Skimming through list");
                int topicCount = 0;
                bool foundTopic = false;
                foreach(Topic topic in Topic.topics)
                {
                    if (topic.topicName == question.topic)
                    {
                        foundTopic = true;
                        info[topicCount].Add(question.GetFormattedAnswer());
                    }
                    topicCount++;
                }
                if (foundTopic == false) //If the topic cannot be found, then create a new topic
                {
                    info.Add(new List<string>());
                    Topic.topics.Add(new Topic(question.topic, info[topicCount]));
                    info[topicCount].Add(question.GetFormattedAnswer());
                }
            }
        }
    }

    void debugTestQuestion()
    {
        if (PlayerProperties.QuestionsAsked.Count > 1)
            PlayerProperties.QuestionsAsked.Clear();

        Question q1 = new Question("Nani?", "GDP is increasing according to this guy", 10, 10,10, "GDP");
        Question q3 = new Question("Wat", "GDP is at an increasing rate", 10, 10, 10, "GDP");
        Question q2 = new Question("v", "Inflation has increase", 10, 10, 10, "Inflation");

        PlayerProperties.QuestionsAsked.Add(q1);
        PlayerProperties.QuestionsAsked.Add(q3);
        PlayerProperties.QuestionsAsked.Add(q2);
        CheckAskedQuestion();
    }
}