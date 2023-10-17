using System;
using UnityEngine;
using UnityEngine.InputSystem;

// when using the GameManager in other scripts make sure you use GameManager.Instance
public class GameManager
{
    private static GameManager instance;

    public Controls inputActions;

    private GameManager()
    {
        // Do not reference GameObjects here because this is created before the objects
        inputActions = new Controls();
        inputActions.Runner.Enable();
        inputActions.Dropper.Enable();
    }

    public static GameManager Instance
    {
        get
        {
            instance ??= new GameManager();
            return instance;
        }
    }

    // Add game mananger members here
    /* example:
     * public void Pause(bool paused)
     * {
     * ...
     * }
     */
}