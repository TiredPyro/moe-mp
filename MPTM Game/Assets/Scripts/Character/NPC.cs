using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPC
{
	public static List<NPC> NPCList = new List<NPC>();

	public string npcName;
	public int npcID;
	public string spritePath;
	public string thumbnailSpritePath;
	public List<string> openingLines;
	public List<string> closingLines;
	public List<Question> questionList;
	public string npcDescription;

	public NPC(string npcName, int npcID, string spritePath, string thumbnailSpritePath, List<string> openingLines, List<string> closingLines, List<Question> questionList, string npcDescription)
	{
		this.npcName = npcName;
		this.npcID = npcID;
		this.spritePath = spritePath;
		this.thumbnailSpritePath = thumbnailSpritePath;
		this.openingLines = openingLines;
		this.closingLines = closingLines;
		this.questionList = questionList;
		this.npcDescription = npcDescription;
	}
	
	public string GetNPCName()
	{
		return npcName;
	}

	public int GetNPCID()
	{
		return npcID;
	}

	public string GetSpritePath()
	{
		return spritePath;
	}

	public List<Question> GetQuestionList()
	{
		return questionList;
	}

	public string GetDescription()
	{
		return npcDescription;
	}

	public static NPC GetNpc(int id)
	{
		foreach (NPC npc in NPCList)
			if (npc.GetNPCID() == id)
				return npc;
		return null;
	}
}