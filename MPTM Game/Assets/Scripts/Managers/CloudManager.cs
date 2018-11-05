using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudManager : MonoBehaviour
{
	public Transform Parent_Clouds;
	public GameObject Prefab_Cloud1, Prefab_Cloud2;
	public float Bound_CloudY = 5;
	public float Bound_CloudX = 12;
	public float CloudSpeedMultiplier = 0.5f;
	public float CloudSpawnInterval = 1.0f;
	public float CloudSpeedUpMultiplier = 15.0f;
	public float CloudSpeedUpLimit = 25.0f;

	float cloudElapsed = 0.0f;
    float speedUpMultiplier = 1.0f;
    public bool speeding;

	public static CloudManager instance;

	void Awake()
	{
		instance = this;
	}

	void Start ()
	{
        speeding = false;
		cloudElapsed = CloudSpawnInterval;
		Pool.Clouds = new List<GameObject>();
		for (int i = 0; i < 16; i++)
			SpawnCloud().transform.localPosition = new Vector2(Random.Range(Bound_CloudX, -Bound_CloudX), Random.Range(Bound_CloudY, -Bound_CloudY));
	}


	void Update ()
	{
		//speeding = Input.GetKey(KeyCode.Space);
		speedUpMultiplier = speeding ? Mathf.Min(CloudSpeedUpLimit, speedUpMultiplier + Time.deltaTime * CloudSpeedUpMultiplier) : 
			Mathf.Max(1, speedUpMultiplier - Time.deltaTime * CloudSpeedUpMultiplier);

        foreach (Transform cloud in Parent_Clouds)
		{
			if (!cloud.gameObject.activeInHierarchy)
				continue;

			float x = cloud.transform.localPosition.x + (Time.deltaTime * cloud.transform.lossyScale.x * CloudSpeedMultiplier * speedUpMultiplier);
			cloud.transform.localPosition = new Vector2(x, cloud.transform.localPosition.y);

			if (x > Bound_CloudX)
				Pool.PoolCloud(cloud.gameObject);

		}

        if (cloudElapsed > 0.0f)
        {
			cloudElapsed -= Time.deltaTime * speedUpMultiplier;
            if (cloudElapsed <= 0.0f)
            {
                cloudElapsed = CloudSpawnInterval;
                SpawnCloud();
            }
        }
	}

    public GameObject SpawnCloud()
	{
		GameObject cloud = Pool.GetCloud();

		if (cloud == null)
		{
			cloud = Instantiate(Random.Range(0.0f, 1.0f) > 0.5f ? Prefab_Cloud1 : Prefab_Cloud2);
			float size = Random.Range(1.0f, 3.0f);
			cloud.transform.SetParent(Parent_Clouds);
			cloud.transform.localScale = Vector2.one * size;

			Color color = Color.white;
			color.r = color.g = color.b = Mathf.Pow(size / 3.0f, 0.5f);
			//Constraints the value to become 0.5477 - 1.0 instead of 0.3333 - 1.0 while maintaining the gradient fashion
			SpriteRenderer spriteRenderer = cloud.GetComponent<SpriteRenderer>();
			spriteRenderer.color = color;
			spriteRenderer.sortingOrder = (int)(size * 100.0f);

			//Cloud sorting - by size - so that bigger ones are "in front" and smaller ones are "behind"
			//Removed after switching to SpriteRenderer - now uses sortingOrder
			/*
			List<Transform> clouds = new List<Transform>();
			List<Transform> sorted = new List<Transform>();
			foreach (Transform child in Parent_Clouds)
				clouds.Add(child);

			int index = 0;
			while (clouds.Count > 0)
			{
				Transform lowest = null;
				foreach (Transform child in clouds)
					if (lowest == null || child.localScale.x < lowest.localScale.x)
						lowest = child;

				clouds.Remove(lowest);
				sorted.Add(lowest);
				index++;
			}

			for (int i = 0; i < sorted.Count; i++)
				sorted[i].GetComponent<SpriteRenderer>().sortingOrder = i;
				*/
		}

		cloud.transform.localPosition = new Vector2(-Bound_CloudX, Random.Range(Bound_CloudY, -Bound_CloudY));

		return cloud;
	}
}
