using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component to store player properties that will not be lost through scene transitions
public class PlayerProperties
{
	public static float ResearchSkillPoints = 0;
	public static float RequiredInfoPoints = 0;

	public static int CurrentMap = 3;
	public static int DestinationMap = -1;

    public static int CurrentBuilding;
	public static List<Question> QuestionsAsked = new List<Question>();

    public static bool choseCorrectDecision;

    public static void Reset()
    {
        CurrentMap = 0;
        DestinationMap = 0;
        ResearchSkillPoints = 0;
        RequiredInfoPoints = 0;
        CurrentBuilding = 0;
        QuestionsAsked.Clear();
        choseCorrectDecision = false;
        Economy.mNEERState = Economy.NEERState.Neutral;
    }
}
