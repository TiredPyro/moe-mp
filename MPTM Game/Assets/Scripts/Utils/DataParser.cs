//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;

/*
 * The ConversationParser is to read the set of questions & answers
 * to be utilized by the NPC ingame
 */

public class DataParser
{
	readonly static bool debugNPC = false;
	readonly static bool debugMap = false;
	readonly static bool debugBuilding = false;

	static Dictionary<string, Sprite> cachedSpriteList = new Dictionary<string, Sprite>();

	/*
	public static void ReadNPCData()
	{
		string dir = Directory.GetCurrentDirectory() + "\\NPCs";
		if (!Directory.Exists(dir))
			return;
		Map.MapList.Clear();
		foreach (string file in Directory.GetFiles(dir))
		{
			if (!file.ToLower().EndsWith(".json"))
				continue;
			NPC.NPCList.Add(JsonUtility.FromJson<NPC>(File.ReadAllText(file)));
		}
	}

	public static void ReadMapData()
	{
		string dir = Directory.GetCurrentDirectory() + "\\Maps";
		if (!Directory.Exists(dir))
			return;
		Map.MapList.Clear();
		foreach (string file in Directory.GetFiles(dir))
		{
			if (!file.ToLower().EndsWith(".json"))
				continue;
			Map.MapList.Add(JsonUtility.FromJson<Map>(File.ReadAllText(file)));
		}
	}

	public static void ReadBuildingData()
	{
		string dir = Directory.GetCurrentDirectory() + "\\Buildings";
		if (!Directory.Exists(dir))
			return;
		Building.BuildingList.Clear();
		foreach (string file in Directory.GetFiles(dir))
		{
			if (!file.ToLower().EndsWith(".json"))
				continue;
			Building.BuildingList.Add(JsonUtility.FromJson<Building>(File.ReadAllText(file)));
		}
	}
	*/

	public static void ReadLocalData()
	{
		JSONData.InitLocal();

		if (!JSONData.InitializedLocal())
			return;

		NPC.NPCList.AddRange(JSONData.GetLocalNPCList());
		Map.MapList.AddRange(JSONData.GetLocalMapList());
		Building.BuildingList.AddRange(JSONData.GetLocalBuildingList());
	}

	public static void ReadOnlineData()
	{
		JSONData.InitOnline();

		if (!JSONData.InitializedOnline())
			return;

		NPC.NPCList.AddRange(JSONData.GetOnlineNPCList());
		Map.MapList.AddRange(JSONData.GetOnlineMapList());
		Building.BuildingList.AddRange(JSONData.GetOnlineBuildingList());
	}

	public static void Init()
	{
		ReadLocalData();
		ReadOnlineData();

		if (debugNPC)
		{
			for (int i = 0; i < NPC.NPCList.Count; i++)
			{
				NPC npc = NPC.NPCList[i];
				Debug.Log(npc.GetNPCName() + ", " + npc.GetNPCID().ToString() + ", " + npc.GetSpritePath());
				foreach (Question question in npc.GetQuestionList())
					Debug.Log("    " + question.GetQuestion() + ", " + question.GetFormattedAnswer() + ", " + question.GetResearchPoints() + ", " + question.GetInfoPoints() + ", " + question.GetWeightage());
			}
		}

		if (debugMap)
		{
			for (int i = 0; i < Map.MapList.Count; i++)
			{
				Map map = Map.MapList[i];
				Debug.Log(map.mapName + ", " + map.mapID + ", " + map.backgroundPath + ", " + map.npc);
			}
		}

		if (debugBuilding)
		{
			for (int i = 0; i < Building.BuildingList.Count; i++)
			{
				Building building = Building.BuildingList[i];
				Debug.Log(building.buildingName + ", " + building.buildingID);
				foreach (int npc in building.npcList)
					Debug.Log("   " + npc);
			}
		}

	}

	public static Sprite GetSpriteFromFile(string filePath, float pixelsPerUnit = 100.0f)
	{
		if (JSONData.UseResources)
			return Resources.Load<Sprite>(filePath.Split('.')[0].Replace("\\", "/").Replace("Assets/Data/", JSONData.ResourcesPath));
		Sprite sprite;

		if (cachedSpriteList.ContainsKey(filePath))
		{
			cachedSpriteList.TryGetValue(filePath, out sprite);
			return sprite;
		}

		Texture2D SpriteTexture = LoadTexture(filePath);
		sprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), pixelsPerUnit);
		cachedSpriteList.Add(filePath, sprite);
		//Debug.Log(cachedSpriteList.Count);
		return sprite;
	}

	public static T ReadJSONFromURL<T>(string url)
	{
		using (var webClient = new System.Net.WebClient())
		{
			var json = webClient.DownloadString(url);
			return JsonUtility.FromJson<T>(json);
		}
	}

	public static string ReadFromURL(string url)
	{
		using (var webClient = new System.Net.WebClient())
			return webClient.DownloadString(url);
	}

	static Texture2D LoadTexture(string filePath)
	{
		Texture2D Tex2D;
		byte[] FileData;

		if (File.Exists(filePath))
		{
			FileData = File.ReadAllBytes(filePath);
			Tex2D = new Texture2D(2, 2);
			if (Tex2D.LoadImage(FileData))
				return Tex2D;
		}
		return null;
	}

	static string RemovePrefixingSpaces(string line)
	{
		char[] chars = line.ToCharArray();
		string build = "";
		bool ignore = false;

		foreach (char c in chars)
		{
			if (c != ' ')
			{
				ignore = true;
				build += c;
			}
			else if (ignore)
				build += c;
		}

		return build;
	}
}
