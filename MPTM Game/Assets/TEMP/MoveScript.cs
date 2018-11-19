using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

    public float runSpeed = 1f;
    [SerializeField] private Camera _camera;
    KeyCode lastKeyPressed = KeyCode.None;
    bool notMoving = true;
    Vector2 movement = Vector2.zero;

    float leftBound, rightBound, upBound, downBound;
    float horizontalSize, verticalSize;

    SpriteRenderer sprite;

	// Use this for initialization
	void Start () {

        leftBound = -1 * GetBoundaryScript.ReturnCamHorizontalBound(_camera);
        rightBound = 1 * GetBoundaryScript.ReturnCamHorizontalBound(_camera);
        downBound = -1 * GetBoundaryScript.ReturnCamVerticalBound(_camera);
        upBound = 1 * GetBoundaryScript.ReturnCamVerticalBound(_camera);

        sprite = GetComponent<SpriteRenderer>();
        horizontalSize = sprite.bounds.extents.x;
        verticalSize = sprite.bounds.extents.y;

    }
	
	// Update is called once per frame
	void Update () {

        FindLastKey();
        StartMoving(lastKeyPressed);

        if (!notMoving)
        {
            StopMoving(lastKeyPressed);
            gameObject.transform.Translate(movement);
        }

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, leftBound + horizontalSize, rightBound - horizontalSize);
        clampedPos.y = Mathf.Clamp(transform.position.y, downBound + verticalSize, upBound - verticalSize);
        transform.position = clampedPos;
    }

    void FindLastKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            notMoving = false;
            lastKeyPressed = KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            notMoving = false;
            lastKeyPressed = KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            notMoving = false;
            lastKeyPressed = KeyCode.S;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            notMoving = false;
            lastKeyPressed = KeyCode.D;
        }
    }

    void StartMoving(KeyCode key)
    {
        if (key == KeyCode.W || key == KeyCode.S)
            movement = new Vector2(0f, Input.GetAxisRaw("Vertical") * runSpeed);
        else if (key == KeyCode.A || key == KeyCode.D)
            movement = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, 0f);
    }

    void StopMoving(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {

        }
    }
}