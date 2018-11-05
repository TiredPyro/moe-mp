using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableDestination : MonoBehaviour
{
	public static List<DraggableDestination> Destinations = new List<DraggableDestination>();
	public Draggable.TopicType type;
	public bool Filled = false;

	void Start()
	{
		Destinations.Add(this);
	}

	void OnDestroy()
	{
		Destinations.Remove(this);
	}
}
