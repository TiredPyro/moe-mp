using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerChanger : MonoBehaviour {
	public string sortingLayer;
	void Start ()
	{
		GetComponent<Renderer>().sortingLayerName = sortingLayer;
	}
}
