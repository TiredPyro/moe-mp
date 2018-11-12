using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSceneManager : MonoBehaviour
{
    /*I was thinking of having a list of buttons, which will load up the image sprite of the map (Have that in MapObject class)
     * Each button will have a class that just stores an index which represents the MapID
     * Have a function to have chosen blinking sprites (In the MapObject script)
     * Placements: Have a list of transform to set the places/Have a fixed amount of buttons and have them set
     */

    public GameObject[] buttonList;
    public GameObject mapObjectPrefab;

    public Canvas canvas;

    public float UISpeed = 4.0f;
    //public float UIPhoneSpeed = 1.0f;

    public Image UI_NPCSelectPopup;
    public Image OutClickChecker;

    //public Image UI_Phone;
    //public GameObject UI_Phone_Text;
    public Image UI_NPCThumbnail;
    public Text UI_NPCName;
    public Text UI_NPCDescription;

    Building selectedBuilding;
    int selectedNPC = -1;
    int selectedNPCIndex = -1;

    //bool phoneActivated = false;
    //bool phoneTransitioning = false; // feel free to change the naming
    bool npcSelectPopup = false;
    float popupElapsed = 1.0f;
    //Vector2 initialPhonePosition;

    void Awake()
    {
        Debug.Log(Economy.mNEERState);


        DataParser.Init();
        //phoneActivated = false;
        //initialPhonePosition = UI_Phone.transform.localPosition;
    }

    void Update()
    {
        popupElapsed += Mathf.Min(1.0f - popupElapsed, Time.deltaTime * UISpeed);
        UI_NPCSelectPopup.transform.localPosition = new Vector2
            (0, -UIUtils.GetOutsideCanvasY(UI_NPCSelectPopup.rectTransform, canvas) *
            (npcSelectPopup ? (1 - popupElapsed) : popupElapsed));
        /*
        if (Input.GetKeyDown(KeyCode.Space) && !phoneTransitioning)
            StartCoroutine(MovePhone());
            */
	}
    /*
    IEnumerator MovePhone()
	{
		phoneTransitioning = true;
        if (phoneActivated == true)
        {
            UI_Phone_Text.SetActive(!phoneActivated);
            UI_Phone.transform.GetChild(0).GetComponent<Animator>().SetBool("Shrink", phoneActivated);
            yield return new WaitForSeconds(0.25f);
        }

        Vector2 startPosition = phoneActivated ? Vector2.zero : initialPhonePosition;
        Vector2 newPosition = phoneActivated ? initialPhonePosition : Vector2.zero;
        Vector2 scaleSmall = Vector2.one * 0.15f;
        Vector2 scaleBig = Vector2.one;
        float angle = -90.0f;

        float temp = 0.0f;
        while (temp < 1.0f)
        {
            UI_Phone.transform.localPosition = Vector2.Lerp(startPosition, newPosition, temp); // phone movement
            UI_Phone.transform.localScale = Vector2.Lerp(phoneActivated ? scaleBig : scaleSmall, // phone size
                                                         phoneActivated ? scaleSmall : scaleBig,
                                                         temp);
            UI_Phone.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(phoneActivated ? 0.0f : angle, // phone rotation
                                                                                phoneActivated ? angle : 0.0f,
                                                                                temp));
            temp += Time.deltaTime * UIPhoneSpeed;
            if (temp > 1.0f)
            {
                UI_Phone.transform.localPosition = newPosition;
                UI_Phone.transform.localScale = phoneActivated ? scaleSmall : scaleBig;
                UI_Phone.transform.eulerAngles = new Vector3(0.0f, 0.0f, phoneActivated ? angle : 0.0f);
            }
            yield return null;
        }

        if (phoneActivated == false)
        {
            UI_Phone.transform.GetChild(0).GetComponent<Animator>().SetBool("Shrink", phoneActivated);
            yield return new WaitForSeconds(0.25f);
            UI_Phone_Text.SetActive(!phoneActivated);
        }
        phoneActivated = !phoneActivated;
        phoneTransitioning = false;
    }
    */

    //Modified
    public void BuildingClick(int buildingID)
    {
        if (CheckForQuestionsScript.instance.IsBuildingCleared(buildingID))
            return;

        //Doesn't change NPC popup details incase the previous building selected was the same
        if (selectedBuilding != null && selectedBuilding.buildingID == buildingID)
        {
            if (!CheckForQuestionsScript.instance.NoAvailableQuestions(selectedNPC))
                ToggleNPCSelectPopup();

            return;
        }

        selectedBuilding = Building.GetBuilding(buildingID);
        selectedNPCIndex = 0;
        selectedNPC = selectedBuilding.npcList[selectedNPCIndex];

        for (int i = 0; i < selectedBuilding.npcList.Count; i++)
        {
            if (!CheckForQuestionsScript.instance.NoAvailableQuestions(selectedNPC))
            {
                SetPopupNPCDetails(NPC.GetNpc(selectedBuilding.npcList[selectedNPCIndex]));

                if (!CheckForQuestionsScript.instance.NoAvailableQuestions(selectedNPC))
                    ToggleNPCSelectPopup();

                break;
            }
            else
            {
                selectedNPCIndex++;

                if (selectedNPCIndex < selectedBuilding.npcList.Count)
                    selectedNPC = selectedBuilding.npcList[selectedNPCIndex];
            }
        }
    }

    public void ToggleNPCSelectPopup()
    {
        npcSelectPopup = !npcSelectPopup;
        OutClickChecker.raycastTarget = npcSelectPopup;
        Color b = Color.black;
        b.a = npcSelectPopup ? 0.3f : 0.0f;
        OutClickChecker.color = b;
		//Out-Click Checker is an Image UI GameObject that triggers this function;
		//Enabling this when the popup enters the screen allows the player to "click out" of the popup.

        popupElapsed = 1.0f - popupElapsed;
    }

    public void SetPopupNPCDetails(NPC npc)
    {
		if (npc.thumbnailSpritePath != null && npc.thumbnailSpritePath.Length > 0)
		{
			Sprite sprite = DataParser.GetSpriteFromFile(npc.thumbnailSpritePath);
			UI_NPCThumbnail.sprite = sprite;
			//UI_NPCThumbnail.rectTransform.sizeDelta = sprite.bounds.size / (Mathf.Max(sprite.bounds.size.y, sprite.bounds.size.x) / 250.0f);
		}

        UI_NPCName.text = npc.GetNPCName();
        UI_NPCDescription.text = npc.GetDescription();
    }

    //Modified
    //true = next NPC; false = previous
    public void SwitchNPC(bool next)
    {
        int newNPCIndex = selectedNPCIndex + (next ?
        (selectedNPCIndex == selectedBuilding.npcList.Count - 1 ? -selectedNPCIndex : 1) :
        (selectedNPCIndex == 0 ? selectedBuilding.npcList.Count - 1 : -1));

        if (!CheckForQuestionsScript.instance.NoAvailableQuestions(selectedBuilding.npcList[newNPCIndex]))
        {
            //selectedNPCIndex += next ?
            //(selectedNPCIndex == selectedBuilding.npcList.Count - 1 ? -selectedNPCIndex : 1) :
            //(selectedNPCIndex == 0 ? selectedBuilding.npcList.Count - 1 : -1);

            selectedNPCIndex = newNPCIndex;
            selectedNPC = selectedBuilding.npcList[selectedNPCIndex];

            SetPopupNPCDetails(NPC.GetNpc(selectedNPC));
        }
    }

    public void TalkToNPC()
    {
        PlayerProperties.CurrentBuilding = selectedBuilding.buildingID;
        foreach (Map map in Map.MapList)
            if (map.npc == selectedNPC)
            {
                PlayerProperties.CurrentMap = map.mapID;
                break;
            }
        SceneTransitionManager.StartTransition("Conversation");
    }

    public void TransitionToPuzzle()
    {
        SceneTransitionManager.StartTransition("Puzzle");
    }
}