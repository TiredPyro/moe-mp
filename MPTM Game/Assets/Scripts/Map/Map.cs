using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
	public static List<Map> MapList = new List<Map>();

	public int mapID;
	public string mapName;
	public string backgroundPath;
	public int npc;

	public Map(int mapID, string mapName, string backgroundPath, int npc)
	{
		this.mapID = mapID;
		this.mapName = mapName;
		this.backgroundPath = backgroundPath;
		this.npc = npc;
	}

	public static Map GetMap(int mapID)
	{
		foreach (Map map in MapList)
			if (map.mapID == mapID)
				return map;
		return null;
	}
}
