using System;
using UnityEngine;
using UnityEngine.InputSystem;

// when using the GameManager in other scripts make sure you use GameManager.Instance
public class GameManager
{
    private static GameManager instance;

    public Controls inputActions;
    public float runnerWinHeight;

    private GameManager()
    {
        // Do not reference GameObjects here because this is created before the objects
        inputActions = new Controls();
        inputActions.Runner.Enable();
        inputActions.Dropper.Enable();

        runnerWinHeight = 8f;
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
    public void GameOver(bool runnerWon)
    {
        Debug.Log(runnerWon ? "The runner wins!" : "The dropper wins!");

        try
        {
            LevelLoaderScript levelLoaderScript = UnityEngine.Object.FindObjectOfType<LevelLoaderScript>();
            levelLoaderScript.loadEndScreen();
        }
        catch
        {
            Debug.Log("Could not load end screen");
        }

    }
}