using UnityEngine;
using System.Collections;

public class PixelPerfectScreen : MonoBehaviour
{
    public int screenVerticalPixels = 240;

    public bool preferUncropped = true;

    private float screenPixelsY;

    private float ratio;

    void Start()
    {
        if (preferUncropped)
        {
            screenPixelsY = (float)Screen.height;
            float screenRatio = screenPixelsY / screenVerticalPixels;
            ratio = Mathf.Floor(screenRatio) / screenRatio;
            transform.localScale *= ratio;
        }
        else
        {
            screenPixelsY = (float)Screen.height;
            float screenRatio = screenPixelsY / screenVerticalPixels;
            ratio = Mathf.Ceil(screenRatio) / screenRatio;
            transform.localScale *= ratio;
        }
    }
}
