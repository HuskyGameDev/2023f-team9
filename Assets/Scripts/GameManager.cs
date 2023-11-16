using System;
using UnityEngine;
using UnityEngine.InputSystem;

// when using the GameManager in other scripts make sure you use GameManager.Instance
public class GameManager
{
    private static GameManager instance;

    // always exists
    public Controls inputActions;
    public float runnerWinHeight;

    // find if needed
    private PlayerTrapped playerTrappedScript = null;
    private NewBoard boardScript = null;
    private GameObject camera = null;
    private Timer timer = null;

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

    public void ResetCamera()
    {
        if (camera == null)
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.SetPositionAndRotation(new Vector3(0, 0, -10), new Quaternion(0, 0, 0, 0));
        camera.GetComponent<Camera>().orthographicSize = 10;
    }
}