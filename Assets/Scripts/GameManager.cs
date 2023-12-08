using System;
using UnityEngine;
using UnityEngine.InputSystem;

// when using the GameManager in other scripts make sure you use GameManager.Instance
public class GameManager
{
    private static GameManager instance;
    public static bool ARCADE_CABINET = false;

    // always exists
    public Controls inputActions;
    public float runnerWinHeight;

    // find if needed
    private PlayerTrapped playerTrappedScript = null;
    private NewBoard boardScript = null;
    private Timer timer = null;

    // Access to PlayerMovement
    private PlayerMovement playerMovement = null;

    private GameManager()
    {
        // Do not reference GameObjects here because this is created before the objects
        if (Debug.isDebugBuild)
        {
            Debug.developerConsoleEnabled = true;
            Debug.developerConsoleVisible = true;
            Debug.LogError("Debugging Enabled (not a real error)");
        }

        inputActions = new Controls();
        if (ARCADE_CABINET)
        {
            var rotateLeftIndex = inputActions.Dropper.Rotate.GetBindingIndexForControl(Keyboard.current.qKey);
            var rotateRightIndex = inputActions.Dropper.Rotate.GetBindingIndexForControl(Keyboard.current.eKey);
            inputActions.Dropper.Rotate.ApplyBindingOverride(rotateLeftIndex, "<Keyboard>/u");
            inputActions.Dropper.Rotate.ApplyBindingOverride(rotateRightIndex, "<Keyboard>/i");
        }
        inputActions.Runner.Enable();
        inputActions.Dropper.Enable();

        runnerWinHeight = 8f;

        // Access PlayerMovement
        playerMovement = UnityEngine.Object.FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in the scene.");
        }
    }

    // Accessor for PlayerMovement
    public PlayerMovement PlayerMovement => playerMovement;

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
            if (runnerWon == true)
            {
                levelLoaderScript.loadRunnerEndScreen();
            }
            else
            {
                levelLoaderScript.loadDropperEndScreen();
            }

        }
        catch
        {
            Debug.Log("Could not load end screen");
        }

    }

    public void BlockPlaced()
    {
        if (playerTrappedScript == null)
            playerTrappedScript = UnityEngine.Object.FindFirstObjectByType<PlayerTrapped>();

        playerTrappedScript.CheckPlayerTrapped();
    }

    public void SetAllowPieceSpawn(bool canSpawn)
    {
        if (boardScript == null)
            boardScript = UnityEngine.Object.FindFirstObjectByType<NewBoard>();
        if (timer == null)
            timer = UnityEngine.Object.FindFirstObjectByType<Timer>();

        boardScript.CanSpawnPieces = canSpawn;
        if (canSpawn)
        {
            boardScript.SpawnPiece();
            timer.timerOn = true;
        }
        else
        {
            timer.timerOn = false;
        }
    }
}