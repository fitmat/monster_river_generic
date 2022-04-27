using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CZ_InputManager : MonoBehaviour
{
    public static CZ_InputManager instance;
    public Vector2 touchPosition;
    public event Action playerJumpEvent;

    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        // Every update call functions to handle user input
        KeyboardInput();
        TouchInput();
    }

    // Function to handle Keyboard input when playing on PC
    private void KeyboardInput()
    {
        // On pressing Space key, call player 1 jump event
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJumpEvent?.Invoke();
        }
    }

    // Function to handle Touchscreen input when playing on Mobile devices
    private void TouchInput()
    {
        // Detect player touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Calculate touch position
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
            
            playerJumpEvent?.Invoke();
            
        }
    }
}
