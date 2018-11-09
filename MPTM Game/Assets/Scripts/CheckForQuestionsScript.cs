using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForQuestionsScript : MonoBehaviour {

    public static CheckForQuestionsScript instance;

	// Use this for initialization
	void Start () {

        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool NoAvailableQuestions(int npcID)
    {
        List<Question> questionList = NPC.GetNpc(npcID).questionList;
        List<Question> totalQuestions = new List<Question>();

        //Storing all the questions in a question list
        foreach (Question question in questionList)
            if (!PlayerProperties.QuestionsAsked.Contains(question))
                totalQuestions.Add(question);

        if (totalQuestions.Count == 0)
            return true;
        else
            return false;
    }

    public bool IsBuildingCleared(int buildingID)
    {
        Building theBuilding = Building.GetBuilding(buildingID);
        int NPCIndex = 0;
        int NPCID = theBuilding.npcList[NPCIndex];

        for (int i = 0; i < theBuilding.npcList.Count; i++)
        {
            if (!NoAvailableQuestions(NPCID))
                return false;
            else
            {
                NPCIndex++;

                if (NPCIndex < theBuilding.npcList.Count)
                    NPCID = theBuilding.npcList[NPCIndex];
            }
        }

        return true;
    }
}