using UnityEngine;
using System.Collections.Generic;

public class CameraResolutions : MonoBehaviour
{
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;

    private void RescaleCamera()
    {
 
        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;
 
        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();
 
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
 
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
 
             camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
 
            Rect rect = camera.rect;
 
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
 
             camera.rect = rect;
        }
 
        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }
 
    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);
 
        Camera.main.rect = nr;
        //GL.Clear(true, true, Color.black);
       
        Camera.main.rect = wp;
 
    }
 
    // Use this for initialization
    void Start () {
        RescaleCamera();
    }
   
    // Update is called once per frame
    void Update () {
        RescaleCamera();
    }
}

/* colours
    Tangerine #FF3700
    Bubble #91D3CC
    yipli logo : #53AEA3

    Message text size - 66 Medium
    Button Text - 53 Bold
    color text #000000

    Switch player
    pro wheeler
    selected player text size semi bold 72px
    other players semi bold 43 px

    character spacing
    Display text medium  - (-4)

    onclick remove shadows
    Launch form yipli panel, Test messgae for Getting user with everyone

    Phone holder tutorial for TV,Phone and PC
    For TV 4 to 5 steps are there
    1. Mat must be connected to Stick via USB
    2. Stick must be connected to TV via HDMI port
    3. Stick must be connected to power source.
    4. User must use the Yipli app in stick to launch any games
    5.

*/