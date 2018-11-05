using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils
{
	public static float GetOutsideCanvasX(RectTransform rectTransform, Canvas canvas)
	{
		RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
		float space = canvasRectTransform.rect.width - rectTransform.rect.width * rectTransform.localScale.x;
		float pos = rectTransform.rect.width + (space / 2.0f);
		return pos;
	}

	public static float GetOutsideCanvasY(RectTransform rectTransform, Canvas canvas)
	{
		RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
		float space = canvasRectTransform.rect.height - rectTransform.rect.height * rectTransform.localScale.y;
		float pos = rectTransform.rect.height + (space / 2.0f);
		return pos;
	}
}
