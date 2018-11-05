using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
	public Button UI_Proceed;
	public ParticleSystem ParticleSystem;

	public static PuzzleManager instance;

	void Awake()
	{
		instance = this;
		UI_Proceed.gameObject.SetActive(false);
	}

	public void CheckForCompletion()
	{
		foreach (DraggableDestination destination in DraggableDestination.Destinations)
            if (!destination.Filled)
                return;
		UI_Proceed.gameObject.SetActive(true);
	}

    public void Proceed()
    {
        SceneTransitionManager.StartTransition("End");
    }

}
