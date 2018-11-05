using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	readonly float snapRadius = 1.0f;
	readonly float returnSpeed = 4.0f;

	SpriteRenderer spriteRenderer;
	bool clicking;
	bool Filled;
	Sprite stillSprite;
	Vector3 clickOffset = Vector3.zero;
	Vector3 originalPosition;
	Vector3 endOffset = Vector3.zero;

	public Sprite hoverSprite;
	public Sprite clickSprite;

	public TopicType type;

	public static bool dragging = false;

	public enum TopicType
	{
		PricesOfImports,
		CostPassThroughByFirmsToConsumers,
		Inflation,
		LabourMarketAndBusinessCosts,
		ExternalAndDomesticDemandForSGGoodsAndServices,
		PricesOfSGGoodsAndServices
	}

	void Start()
	{
		originalPosition = transform.position;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		stillSprite = spriteRenderer.sprite;

		transform.GetChild(0).gameObject.GetComponent<Renderer>().sortingLayerName = gameObject.GetComponent<Renderer>().sortingLayerName;
	}

	void Update()
	{
		if (Filled)
			return;

		Vector3 input = Input.mousePosition;
		input.z = -Camera.main.transform.position.z;
		input = Camera.main.ScreenToWorldPoint(input);

		if (WithinSprite(input, this))
		{
			if (!clicking && SwitchSpriteHover())
				Hover();

			if (Input.GetMouseButtonDown(0) && !dragging)
			{
				clickOffset = input - transform.position;
				dragging = true;
			}
		}
		else
		{
			clicking = false;
			Still();
		}

		if (Input.GetMouseButton(0) && clickOffset != Vector3.zero)
		{
			transform.position = input - new Vector3(clickOffset.x, clickOffset.y, 0.0f);
			if (SwitchSpriteClick())
				Clicked();
			else if (SwitchSpriteHover())
				Hover();
			clicking = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			dragging = false;
			clickOffset = Vector3.zero;
			if (SwitchSpriteHover())
				Hover();
			clicking = false;
			endOffset = transform.position - originalPosition;

			foreach (DraggableDestination destination in DraggableDestination.Destinations)
			{
				if (destination.type != type)
					continue;
				if ((transform.position - destination.gameObject.transform.position).sqrMagnitude <= snapRadius)
				{
					transform.position = destination.gameObject.transform.position;
					destination.Filled = true;
					Filled = true;
					Still();
					PuzzleManager.instance.CheckForCompletion();
				}
			}
		}
		else if (transform.position != originalPosition)
		{
			transform.position -= new Vector3(endOffset.x * Time.deltaTime * returnSpeed, endOffset.y * Time.deltaTime * returnSpeed, 0);
			Vector3 difference = transform.position - originalPosition;

			//This took all my brain cells to formulate but it basically checks if the transform has gone past the original position by comparing positives/negatives
			if (endOffset.x >= 0 == difference.x < 0 && endOffset.y >= 0 == difference.y < 0)
				transform.position = originalPosition;
			return;
		}
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

	public static bool WithinSprite(Vector2 point, Draggable gameObj)
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
