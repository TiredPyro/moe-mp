using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBoundaryScript {

    public static float ReturnCamVerticalBound(Camera gameCamera)
    {
        return gameCamera.orthographicSize;
    }

    public static float ReturnCamHorizontalBound(Camera gameCamera)
    {
        return gameCamera.orthographicSize * gameCamera.aspect;
    }
}