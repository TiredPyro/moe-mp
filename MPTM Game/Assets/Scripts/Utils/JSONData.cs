using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//More info on: https://docs.google.com/document/d/1O2fUdyfwp7ILvHb3xqxr-csApqxkEdTjD7g91kynW4o/edit?usp=sharing

[Serializable]
public class JSONData
{
	readonly static string URL = "";
	readonly static string LocalPath = Directory.GetCurrentDirectory() + "\\Assets\\Data\\Data.json";

	public readonly static bool UseResources = true;
	public readonly static string ResourcesPath = "Data/";

	public List<NPC> NPCList;
	public List<Map> MapList;
	public List<Building> BuildingList;
	static JSONData lastReadOnline, lastReadLocal;

	public static void InitOnline()
	{
		if (URL != "")
			lastReadOnline = (DataParser.ReadJSONFromURL<JSONData>(URL));
	}

	public static void InitLocal()
	{
		Debug.Log(ResourcesPath + "Data.json");
		if (LocalPath != "")
			lastReadLocal = UseResources ? (JsonUtility.FromJson<JSONData>(Resources.Load<TextAsset>(ResourcesPath + "Data").text))
				: (JsonUtility.FromJson<JSONData>(File.ReadAllText(LocalPath)));
	}

	public static bool InitializedOnline()
	{
		return lastReadOnline != null;
	}

	public static bool InitializedLocal()
	{
		return lastReadLocal != null;
	}

	public static List<NPC> GetOnlineNPCList()
	{
		return lastReadOnline.NPCList;
	}

	public static List<Map> GetOnlineMapList()
	{
		return lastReadOnline.MapList;
	}

	public static List<Building> GetOnlineBuildingList()
	{
		return lastReadOnline.BuildingList;
	}

	public static List<NPC> GetLocalNPCList()
	{
		return lastReadLocal.NPCList;
	}

	public static List<Map> GetLocalMapList()
	{
		return lastReadLocal.MapList;
	}

	public static List<Building> GetLocalBuildingList()
	{
		return lastReadLocal.BuildingList;
	}
}
