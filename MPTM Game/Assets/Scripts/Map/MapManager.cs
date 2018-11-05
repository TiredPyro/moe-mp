using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Component to managing the map location instances.
 * 
 * All individual maps will be played in one framework map scene,
 * having the UI, Background, and NPCs added to initiate.
 */
public class MapManager : MonoBehaviour
{
	Map map;
	public Canvas UI_Canvas;
	public Image UI_Background;
	public Image UI_NPC;
	public Text UI_MapName;
	
	public static MapManager instance;
    private void Awake()
    {
        instance = this;
        DataParser.Init();
    }
    void Start()
	{

		map = Map.GetMap(PlayerProperties.CurrentMap);

		if (map.backgroundPath != null && map.backgroundPath.Length > 0)
		{
			if (!map.backgroundPath.ToLower().EndsWith(".png") && !map.backgroundPath.ToLower().EndsWith(".jpg") && !map.backgroundPath.ToLower().EndsWith(".jpeg"))
			{
				GameObject prefab = Instantiate(Resources.Load<GameObject>(map.backgroundPath));
			}
			else
			{
				UI_Background.sprite = DataParser.GetSpriteFromFile(map.backgroundPath);
				UI_Background.color = Color.white;
			}
			UI_MapName.text = map.mapName;
		}

		string spritePath = NPC.GetNpc(map.npc).spritePath;
		if (spritePath != null && spritePath.Length > 0)
		{
			if (!spritePath.ToLower().EndsWith(".png"))
			{
				GameObject prefab = Instantiate(Resources.Load<GameObject>(spritePath));
			}
			else
			{
				Sprite sprite = DataParser.GetSpriteFromFile(NPC.GetNpc(map.npc).spritePath);
				UI_NPC.sprite = sprite;
				UI_NPC.color = Color.white;
                UI_NPC.SetNativeSize();
				//float sizeMultiplier = sprite.bounds.size.y / 450.0f; //Resizes the NPC to a ySize of 850.
				//UI_NPC.rectTransform.sizeDelta = sprite.bounds.size / sizeMultiplier;
				//UI_NPC.transform.localPosition = new Vector2(0, -UIUtils.GetOutsideCanvasY(UI_NPC.rectTransform, UI_Canvas) + UI_NPC.rectTransform.rect.height);
			}
		}
	}
}
