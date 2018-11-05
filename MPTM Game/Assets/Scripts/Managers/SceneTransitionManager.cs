using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
	public Image UI_Blackscreen;
	public Text Text_Loading;

	public static readonly float TransitionSpeedMultiplier = 3.0f;

	private static string SceneToLoad = null;
	private static float TransitionElapsed = 0.0f;
	private static TransitionMode eTransitionMode = TransitionMode.None;

	private enum TransitionMode
	{
		None,
		FadingIn,
		FadingOut
	}

	void Start ()
	{
		Text_Loading.raycastTarget = false;
	}
	
	void Update ()
	{
		if (TransitionElapsed > 0.0f)
		{
			if (eTransitionMode == TransitionMode.None)
			{
				TransitionElapsed = 0.0f;
				return;
			}
			TransitionElapsed -= Time.deltaTime * TransitionSpeedMultiplier;
			SetBlackscreenLoadingAlpha(eTransitionMode == TransitionMode.FadingIn ? 1 - TransitionElapsed : TransitionElapsed);
			if (TransitionElapsed <= 0.0f)
				EndTransition();
		}
	}

	public static void StartTransition(string SceneName)
	{
		if (eTransitionMode == TransitionMode.FadingIn)
			return;
		eTransitionMode = TransitionMode.FadingIn;
		TransitionElapsed = 1.0f;
		SceneToLoad = SceneName;
	}

	private static void EndTransition()
	{
		if (SceneToLoad == null)
			return;
		SceneManager.LoadScene(SceneToLoad);
		SceneToLoad = null;

		if (eTransitionMode == TransitionMode.FadingOut)
		{
			TransitionElapsed = 0.0f;
			eTransitionMode = TransitionMode.None;
		} else
		{
			TransitionElapsed = 1.0f;
			eTransitionMode = TransitionMode.FadingOut;
		}
	}
	
	public void SetBlackscreenLoadingAlpha(float a)
	{
		UI_Blackscreen.raycastTarget = a > 0.0f;
		Color c = Color.black;
		c.a = a;
		UI_Blackscreen.color = c;
		c.r = c.g = c.b = 1.0f;
		Text_Loading.color = c;
	}
}
