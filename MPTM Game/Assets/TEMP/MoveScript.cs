using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

    public float runSpeed = 1f;
    [SerializeField] private Camera _camera;

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

        float horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        float verticalMovement = Input.GetAxisRaw("Vertical") * runSpeed;

        LastKeyPressed();

        //gameObject.transform.Translate(new Vector2(horizontalMovement, verticalMovement));
        gameObject.transform.Translate(LastKeyPressed());

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, leftBound + horizontalSize, rightBound - horizontalSize);
        clampedPos.y = Mathf.Clamp(transform.position.y, downBound + verticalSize, upBound - verticalSize);
        transform.position = clampedPos;
    }

    Vector2 LastKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.W))
            return new Vector2(0f, 1f);
        else if (Input.GetKeyDown(KeyCode.S))
            return new Vector2(0f, -1f);
        else if (Input.GetKeyDown(KeyCode.D))
            return new Vector2(1f, 0f);
        else if (Input.GetKeyDown(KeyCode.A))
            return new Vector2(-1f, 0f);
        else
            return Vector2.zero;
    }

    KeyCode Check()
    {
        return KeyCode.W;
    }
}