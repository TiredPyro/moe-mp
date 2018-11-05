using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{
    public float speed = 0.1f;
	
	void Update ()
    {
        Vector2 offset = new Vector2(Time.deltaTime * speed, 0); //Moves the background
        GetComponent<Renderer>().material.mainTextureOffset += offset; //Get background texture
    }
}
