using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConversationManager : MonoBehaviour {
    public static ConversationManager instance;
    public GameObject[] Buttons = new GameObject[4]; //Stores all the conversationchoices

    public Canvas canvas;
	//public GameObject questionButtonYes;
	//public GameObject questionButtonNo;
	//public Image RadialContinue;
	//public GameObject ButtonContinueAsking;

    //public List<int> MapID; //Takes from a world manager script. This takes in all the ids of the npcs from the map
    int MapID; //This is the ID of the Map
	public float ContinueElapsedTime = 5.0f;
	public float ButtonDisplaySpeed = 4.0f;
    public float DialougeDisplaySpeed = 4.0f;

    public GameObject DialogueBox;
    public Text nameText;
    public Text CurrentDialogue;

    public Question questionAsked;

	List<Question> questionList;

	Image imageRadial;
	Image imageContinue;
	Text textContinue;

	Color colorRadial;
	Color colorContinue;
	Color colorTextContinue;

	float timeElapsedButtonDisplay;
    float timeElapsedDialogueDisplay;
	float timeElapsedContinue;
    bool stateRunOnce = false;
    bool endConvo = false;
	bool exitConvo = false;
	bool noMoreQuestions = false;
	bool stopUpdatingButtonOpacity = false;
    public enum ConversationState
    {
        INTRODUCTION, //There might be some sort of animation where the player and the interviewee might come in
        QUESTIONING,
        ANSWERING,
        ASKING, //asking if the player wants to question
        END,
		EXIT
    }
    public ConversationState m_Current;

    int talkingCounter = -1; //This is the amount of times the player can talk to an interviewee. If it's -1, it is unlimited for now. Refresh when starting new game depending on NPC

    void OnEnable()
    {
        SimpleEventManager.ChangeState += ChangeState;
    }

    void OnDisable()
    {
        SimpleEventManager.ChangeState -= ChangeState;
    }



    void Start()
    {
		Debug.Log(PlayerProperties.CurrentMap);

        DataParser.Init();


        //MapID.Add(0); //For debugging purposes
        MapID = PlayerProperties.CurrentMap;
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this.gameObject);

        //CurrentDialogue = DialogueBox.GetComponentInChildren<Text>();
        CurrentDialogue.text = "";
        nameText.text = NPC.GetNpc(Map.GetMap(MapID).npc).npcName;

        m_Current = ConversationState.INTRODUCTION;


        //ButtonContinueAsking.GetComponent<Button>().onClick.AddListener(YesButton);
		//questionButtonNo.GetComponent<Button>().onClick.AddListener(NoButton);

		//InstantiateSpeechChoices();

		questionList = NPC.GetNpc(Map.GetMap(MapID).npc).questionList;

        //imageRadial = RadialContinue.GetComponent<Image>();
        //imageContinue = ButtonContinueAsking.GetComponent<Image>();
        //textContinue = ButtonContinueAsking.GetComponentInChildren<Text>();

        //colorRadial = imageRadial.color;
        //colorContinue = imageContinue.color;
        //colorTextContinue = textContinue.color;



	}

    void Update()
    {
        switch (m_Current)
        {
            case ConversationState.INTRODUCTION:
				if (!stateRunOnce)
				{
                    //if (faceAnims != null)
                        //faceAnims.PlayMouthAnimation();
                    InstantiateOpeningLines();
					stateRunOnce = true;
				}

				if (Input.GetMouseButtonDown(0))
					ChangeState();
                break;

            case ConversationState.QUESTIONING:
                if (!stateRunOnce)
				{
					timeElapsedButtonDisplay = 0.0f;
                    //InstantiateSpeechChoices(); //Modified
                    CreateSpeechChoices(CreateQuestionsList());
                    DialogueBox.SetActive(false);
                    stateRunOnce = true;
					stopUpdatingButtonOpacity = false;
                }
				if (!stopUpdatingButtonOpacity)
					UpdateButtonOpacity();
				timeElapsedButtonDisplay += Time.deltaTime * ButtonDisplaySpeed;
                break;

            case ConversationState.ANSWERING:
                if (!stateRunOnce)
				{
                    //if (faceAnims != null)
                        //faceAnims.PlayMouthAnimation();
					DialogueBox.SetActive(true);
					timeElapsedContinue = 0.0f;

					foreach (Question check in Question.ExitConditions)
						if (check == questionAsked)
							exitConvo = true;

					InstantiateSpeechResponse(questionAsked);
					if (!exitConvo)
						InstantiateContinueQuestion();
					stateRunOnce = true;

					int match = 0;
					foreach (Question question in PlayerProperties.QuestionsAsked)
						if (questionList.Contains(question))
							match++;
					if (match == questionList.Count)
					{
						noMoreQuestions = true;
						//ButtonContinueAsking.GetComponentInChildren<Text>().text = "End";
					}
                }
                if (Input.GetMouseButtonDown(0) && DialogueManager.instance.dialogueText.text != DialogueManager.instance.sentence)
                {
                    StopCoroutine(DialogueManager.instance.trigger);
                    DialogueManager.instance.dialogueText.text = DialogueManager.instance.sentence;
                } else if (Input.GetMouseButtonDown(0) && DialogueManager.instance.dialogueText.text == DialogueManager.instance.sentence)
                {
                    endConvo = noMoreQuestions;
                    ChangeState();
                }

				/*
				if (!exitConvo)
				{
					float a = Mathf.Min(1.0f, timeElapsedContinue);

					colorRadial.a = colorContinue.a = colorTextContinue.a = a;

					imageRadial.color = colorRadial;
					imageContinue.color = colorContinue;
					textContinue.color = colorTextContinue;

					RadialContinue.fillAmount = timeElapsedContinue / ContinueElapsedTime;
					timeElapsedContinue += Time.deltaTime;
					if (timeElapsedContinue > ContinueElapsedTime)
					{
						endConvo = true;
						ChangeState();
					}
				}
				*/
				
				break;

            case ConversationState.ASKING:
                if (!stateRunOnce)
                {
                    InstantiateContinueQuestion();
                    stateRunOnce = true;
                }
                break;

			case ConversationState.END:
				if (!stateRunOnce)
				{
                    //if (faceAnims != null)
                        //faceAnims.PlayMouthAnimation();
                    InstantiateClosingLines();
					stateRunOnce = true;
				}

				if (Input.GetMouseButtonDown(0))
					ChangeState();

				break;

			case ConversationState.EXIT:
                if (!stateRunOnce)
                {
                    SetProperties();
                    stateRunOnce = true;
                }
				break;


		}
    }


    IEnumerator TypewriterEffect(string sentence, float speed)
    {
        CurrentDialogue.text = "";
        foreach (char chara in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(speed);
            CurrentDialogue.text += chara;
            yield return null;
        }
    }

    void ChangeState()
	{
		stateRunOnce = false;
		if (endConvo)
		{
			m_Current = ConversationState.END;
			endConvo = false;
			return;
		} else if (exitConvo)
		{
			m_Current = ConversationState.EXIT;
			exitConvo = false;
			return;
		}

		switch (m_Current)
		{
			case ConversationState.INTRODUCTION:
				m_Current = ConversationState.QUESTIONING;
				break;
			case ConversationState.QUESTIONING:
				m_Current = ConversationState.ANSWERING;
				break;
			case ConversationState.ANSWERING:
				m_Current = ConversationState.QUESTIONING;
				break;
			case ConversationState.ASKING:
				m_Current = ConversationState.QUESTIONING;
				break;
			case ConversationState.END:
				m_Current = ConversationState.EXIT;
				break;
			default:
				Debug.LogError("There is no known state to change to"); //Might change to switch case
				break;
		}

    }

	void UpdateButtonOpacity()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			//float alpha = Mathf.Max(0, timeElapsedButtonDisplay - ((i < 2 ? 0 : 1)) / ButtonDisplaySpeed);
			float alpha = Mathf.Max(0, timeElapsedButtonDisplay - (i / ButtonDisplaySpeed));
			Color color = Color.white;
			color.a = alpha;
			Buttons[i].GetComponent<Image>().color = color;
			Buttons[i].GetComponent<Button>().interactable = alpha > 0;
			color.r = color.g = color.b = 0;
			Buttons[i].GetComponentInChildren<Text>().color = color;

			if (i == 3 && alpha >= 1)
				stopUpdatingButtonOpacity = true;
		}
	}

    //Modified
    List<Question> CreateQuestionsList()
    {
        List<Question> totalQuestions = new List<Question>();

        //Storing all the questions in a question list
        foreach (Question question in questionList)
            if (!PlayerProperties.QuestionsAsked.Contains(question))
                totalQuestions.Add(question);

        return totalQuestions;
    }

    //Modified
    int CountQuestionsList(List<Question> totalQuestions)
    {
        return totalQuestions.Count;
    }

    //Modified
    void CreateSpeechChoices(List<Question> totalQuestions)
    {
        if (totalQuestions.Count == 0)
        {
            endConvo = true;
            ChangeState();
            return;
        }

        int count = 0; //What index is the question being assigned to in gameobject buttons
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i == Buttons.Length - 1)
            {
                Question question = Question.ExitConditions[Random.Range(0, Question.ExitConditions.Length)];
                Buttons[count].SetActive(true);
                Buttons[count].GetComponent<ConversationChoice>().CurrentQuestion = question;
                Buttons[count].GetComponent<ConversationChoice>().QuestionText.text = question.question;
                Buttons[count].GetComponent<Image>().color = Color.clear;
                Buttons[count].GetComponent<Button>().interactable = false;
                Buttons[count].GetComponentInChildren<Text>().color = Color.clear;
                continue;
            }

            int totalWeightage = 0;
            foreach (Question question in totalQuestions)
            {
                totalWeightage += question.weightage;
            }
            int r = Random.Range(0, totalWeightage + 1);
            int weightagecount = 0;
            int indexCount = 0;

            while (weightagecount < totalWeightage)
            {
                weightagecount += totalQuestions[indexCount].weightage;
                if (r <= weightagecount)
                {

                    //chosenChoice.CurrentQuestion = totalQuestions[indexCount];

                    Buttons[count].SetActive(true);
                    Buttons[count].GetComponent<ConversationChoice>().CurrentQuestion = totalQuestions[indexCount];
                    Buttons[count].GetComponent<ConversationChoice>().QuestionText.text = totalQuestions[indexCount].question;
                    Buttons[count].GetComponent<Image>().color = Color.clear;
                    Buttons[count].GetComponent<Button>().interactable = false;
                    Buttons[count].GetComponentInChildren<Text>().color = Color.clear;

                    totalQuestions.RemoveAt(indexCount);
                    count++;
                    break;
                }
                else
                {
                    indexCount++;
                }
            }
        }
    }

    void InstantiateSpeechChoices()
    {
        //questionButtonNo.SetActive(false);
		//ButtonContinueAsking.SetActive(false);
		//RadialContinue.gameObject.SetActive(false);
        //Instantiate buttons here then assign them accordingly 
        List<Question> totalQuestions = new List<Question>();

        //Storing all the questions in a question list
        foreach (Question question in questionList)
            if (!PlayerProperties.QuestionsAsked.Contains(question))
            {
                totalQuestions.Add(question);
                /* If the idea of having specific questions for the player to research depending on the topic currently researching
                switch (Economy.mTopicState)
                {
                    case Economy.TopicState.Freeroaming:
                        totalQuestions.Add(question);
                        break;
                    case Economy.TopicState.Import:
                        //If Question.Topic == Import then add it in. Same goes with the rest of the game states
                        break;
                }
                */ 
            }
        print(totalQuestions.Count);

		if (totalQuestions.Count == 0)
		{
			endConvo = true;
			ChangeState();
			return;
		}

        int count = 0; //What index is the question being assigned to in gameobject buttons
        for (int i = 0; i < Buttons.Length; i++)
        {
			if (i == Buttons.Length - 1)
			{
				Question question = Question.ExitConditions[Random.Range(0, Question.ExitConditions.Length)];
				Buttons[count].SetActive(true);
				Buttons[count].GetComponent<ConversationChoice>().CurrentQuestion = question;
				Buttons[count].GetComponent<ConversationChoice>().QuestionText.text = question.question;
				Buttons[count].GetComponent<Image>().color = Color.clear;
				Buttons[count].GetComponent<Button>().interactable = false;
				Buttons[count].GetComponentInChildren<Text>().color = Color.clear;
                continue;
			}
            
            int totalWeightage = 0;
            foreach (Question question in totalQuestions)
            {
                totalWeightage += question.weightage;
            }
            int r = Random.Range(0, totalWeightage + 1);
            int weightagecount = 0;
            int indexCount = 0;

            while (weightagecount < totalWeightage)
            {
                weightagecount += totalQuestions[indexCount].weightage;
                if (r <= weightagecount)
                {

                    //chosenChoice.CurrentQuestion = totalQuestions[indexCount];

                    Buttons[count].SetActive(true);
                    Buttons[count].GetComponent<ConversationChoice>().CurrentQuestion = totalQuestions[indexCount];
                    Buttons[count].GetComponent<ConversationChoice>().QuestionText.text = totalQuestions[indexCount].question;
					Buttons[count].GetComponent<Image>().color = Color.clear;
					Buttons[count].GetComponent<Button>().interactable = false;
					Buttons[count].GetComponentInChildren<Text>().color = Color.clear;

					totalQuestions.RemoveAt(indexCount);
                    count++;
                    break;
                }
                else
                {
                    indexCount++;
                }
            }
        }
	}

	void InstantiateOpeningLines()
	{
		List<string> openingLines = NPC.GetNpc(Map.GetMap(MapID).npc).openingLines;
		if (openingLines.Count == 0)
			return;
		Dialogue dialogue = new Dialogue();
		dialogue.sentences = new string[] { openingLines[Random.Range(0, openingLines.Count)] };
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}

	void InstantiateClosingLines()
    {
        DialogueBox.SetActive(true);
		foreach (GameObject button in Buttons)
			button.SetActive(false);
		//questionButtonNo.SetActive(false);
		//ButtonContinueAsking.SetActive(false);
		//RadialContinue.gameObject.SetActive(false);

		List<string> closingLines = NPC.GetNpc(Map.GetMap(MapID).npc).closingLines;
		if (closingLines.Count == 0)
			return;
		Dialogue dialogue = new Dialogue();
		dialogue.sentences = new string[] { closingLines[Random.Range(0, closingLines.Count)] };
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}

	void InstantiateSpeechResponse(Question question)
    {
		//Despawn buttons here
		PlayerProperties.QuestionsAsked.Add(question);
        foreach (GameObject button in Buttons)
            button.SetActive(false);
        Dialogue dialogue = new Dialogue();
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = question.GetFormattedAnswer();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }


    void InstantiateContinueQuestion()
    {
		//questionButtonYes.SetActive(true);

		/*
		Dialogue dialogue = new Dialogue();
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Would you like to ask another question?";
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		*/
    }

    public void YesButton()
    {
        endConvo = noMoreQuestions;
        ChangeState();
    }

    public void NoButton()
    {
        endConvo = true;
        ChangeState();
    }

    void SetProperties()
    {
        PlayerProperties.ResearchSkillPoints = (BarManager.instance.researchBar.endValue / BarManager.instance.max) * 100;
        PlayerProperties.RequiredInfoPoints = (BarManager.instance.infoBar.endValue / BarManager.instance.max) * 100;

        //Change this to go back to map everytime he finishes, unless is stage 1, because making a new way of transitioning to puzzle
        if (PlayerProperties.RequiredInfoPoints == 100)
        {
            if (Economy.mNEERState == Economy.NEERState.Stronger || Economy.mNEERState == Economy.NEERState.Weaker) //Basically if the player had made a decision, it's already the second stage
            {
                SSNEERManager.instance.ChangeGameStage();
                SceneTransitionManager.StartTransition("ConfirmSSNEER");
            }
            else //This is for transition to stage 1
                SceneTransitionManager.StartTransition("ChooseSSNEER"); //It will change the game stage at the ssneer manager
        }
        else //If not full then goes back to the map
            SceneTransitionManager.StartTransition("Map");
    }
}
