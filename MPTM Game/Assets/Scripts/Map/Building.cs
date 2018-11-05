using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building
{
	public static List<Building> BuildingList = new List<Building>();

	public int buildingID;
	public string buildingName;
	public List<int> npcList;

	public Building(int buildingID, string buildingName, List<int> npcList)
	{
		this.buildingID = buildingID;
		this.buildingName = buildingName;
		this.npcList = npcList;
	}

	public static Building GetBuilding(int buildingID)
	{
		foreach (Building building in BuildingList)
			if (building.buildingID == buildingID)
				return building;
		return null;
	}
}
