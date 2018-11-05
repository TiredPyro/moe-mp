using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
	public static List<GameObject> Clouds;

	public static void PoolCloud(GameObject cloud)
	{
		cloud.SetActive(false);
		Clouds.Add(cloud);
	}

	public static GameObject GetCloud()
	{
		if (Clouds.Count <= 0)
			return null;
		GameObject cloud = Clouds[0];
		Clouds.RemoveAt(0);
		cloud.SetActive(true);
		return cloud;
	}
}
