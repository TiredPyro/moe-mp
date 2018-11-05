using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Question
{
	public string question, answer, topic;
	public int researchPoints, infoPoints, weightage;

	public static Question[] ExitConditions = {
		new Question("That is all, thank you.", "No problem; hope I was able to be of assistance.", 0, 0, 0, null),
		new Question("I have to go now, bye.", "Alright, see you.", 0, 0, 0, null),
		new Question("Thanks for your help, I'll be taking my leave.", "You're welcome.", 0, 0, 0, null)
	};

	public Question(string question, string answer, int researchPoints, int infoPoints, int weightage, string topic)
	{
		this.question = question;
		this.answer = answer;
		this.researchPoints = researchPoints;
		this.infoPoints = infoPoints;
		this.weightage = weightage;
		this.topic = topic;
	}
	
	public string GetQuestion()
	{
		return question;
	}

	public string GetFormattedAnswer()
	{
		return answer.
			Replace("<import_rate>", Economy.GetImportPriceRates()).
			Replace("<inflation_rate>", Economy.GetInflationRates()).
			Replace("<gdp_rate>", Economy.GetGDPRates());
	}

	public int GetResearchPoints()
	{
		return researchPoints;
	}

	public int GetInfoPoints()
	{
		return infoPoints;
	}

	public int GetWeightage()
	{
		return weightage;
	}
}
