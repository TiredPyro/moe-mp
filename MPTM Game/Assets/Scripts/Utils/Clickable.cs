using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Clickable : MonoBehaviour
{
	static ArrayList clickables = new ArrayList();
	static float lastCheckedTime;
	SpriteRenderer spriteRenderer;
	bool clicking;
	private Sprite stillSprite;
	public Sprite hoverSprite;
	public Sprite clickSprite;
	public ActivationType activationType = ActivationType.OnClickRelease;
	
	public enum ActivationType
	{
		OnClickRelease, OnClickPress, ClickAndHold
	}

	void Start()
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			clickables.Add(this);
			stillSprite = spriteRenderer.sprite;
		}
	}

	void Update()
	{
		float currentTime = Time.time;
		if (currentTime != lastCheckedTime)
		{
			lastCheckedTime = currentTime;
			
			var input = Input.mousePosition;
			input.z = -Camera.main.transform.position.z;
			input = Camera.main.ScreenToWorldPoint(input);
			
			foreach (Clickable clickable in clickables)
				if (clickable != null)
					if (WithinSprite(input, clickable))
					{
						if (Input.GetMouseButtonDown(0) && SwitchSpriteClick())
						{
							clickable.Clicked();
							clickable.clicking = true;
						}
						else if (!clicking && SwitchSpriteHover())
						{
							clickable.Hover();
						}
						else if (clicking && Input.GetMouseButtonUp(0))
						{
							clickable.Hover();
							clickable.clicking = false;
						}
						
						switch (activationType)
						{
							case ActivationType.ClickAndHold:
								if (Input.GetMouseButton(0))
									clickable.Click();
								break;
							case ActivationType.OnClickPress:
								if (Input.GetMouseButtonDown(0))
									clickable.Click();
								break;
							case ActivationType.OnClickRelease:
								if (Input.GetMouseButtonUp(0))
									clickable.Click();
								break;
						}
					}
					else
					{
						clicking = false;
						clickable.Still();
					}
		}
	}

	//This function is needed to be overrided in any parent class
	//so that they can execute their own unique instructions
	public abstract void Click();
	
	void OnDestroy()
	{
		clickables.Remove(this);
	}
	
	bool SwitchSpriteHover()
	{
		return hoverSprite != null && stillSprite != null;
	}
	
	bool SwitchSpriteClick()
	{
		return clickSprite != null && stillSprite != null;
	}
	
	public void Clicked()
	{
		spriteRenderer.sprite = clickSprite;
	}
	
	public void Hover()
	{
		spriteRenderer.sprite = hoverSprite;
	}
	
	public void Still()
	{
		if (stillSprite == null)
			return;
		spriteRenderer.sprite = stillSprite;
	}
	
	public static bool WithinSprite(Vector2 point, Clickable gameObj)
	{
		if (gameObj == null)
			return false;
		SpriteRenderer sprite = gameObj.spriteRenderer;
		if (sprite == null)
			return false;
		if (sprite.color.a <= 0f)
			return false;
		Bounds bounds = sprite.bounds;
		return (point.x >= bounds.min.x && point.y >= bounds.min.y && point.x <= bounds.max.x && point.y <= bounds.max.y);
	}
}
