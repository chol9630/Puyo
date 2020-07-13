using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;
        float scalheiheight = ((float)Screen.width/Screen.height)/((float) 9/16);
        float scalewidht = 1f / scalheiheight;
        if(scalheiheight <1)
        {
            rect.height = scalheiheight;
            rect.y = (1f -scalheiheight)/ 2f;
        }
        else
        {
            rect.width = scalewidht;
            rect.x = (1f - scalewidht) / 2f;
        }
        camera.rect = rect;

    }
}
