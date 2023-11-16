using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCrush : MonoBehaviour
{
    // Components
    private TilemapCollider2D tilemapCollider; // the board's tilemap collider
    private Tilemap tilemap; // the board's tilemap
    private CircleCollider2D crushCollider;
    private Transform playerTransform;
    private PlayerMovement playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        crushCollider = this.GetComponentInChildren<CircleCollider2D>();
        playerTransform = this.gameObject.transform.GetChild(0);
        playerMovementScript = this.GetComponentInParent<PlayerMovement>();
        try
        {
            tilemapCollider = FindAnyObjectByType<TilemapCollider2D>();
            tilemap = FindAnyObjectByType<Tilemap>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
        }
        //GameManager.Instance.SetAllowPieceSpawn(false); //TEMPORARY
    }

    // Update is called once per frame
    void Update()
    {
        if (crushCollider.IsTouching(tilemapCollider))
        {
            Crush();
        }
    }

    private void Crush()
    {
        GameManager.Instance.GameOver(false);
    }

    /* ======================= *
     *        TRAP TYPES       *
     * ======================= */
    private Vector3Int GetNextRight(Vector3Int PlayerLocation)
    {
        Vector3Int nextRight = new Vector3Int(-NewBoard.Bounds.max.x, PlayerLocation.y);
        for (int i = PlayerLocation.x; i < NewBoard.Bounds.max.x; i++)
        {
            Vector3Int checkLocation = new Vector3Int(i, PlayerLocation.y);
            if (tilemap.HasTile(checkLocation))
            {
                nextRight = checkLocation;
                break;
            }
        }
        return nextRight;
    }
    private Vector3Int GetNextLeft(Vector3Int PlayerLocation)
    {
        Vector3Int nextLeft = new Vector3Int(NewBoard.Bounds.min.x, PlayerLocation.y);
        for (int i = PlayerLocation.x; i > NewBoard.Bounds.min.x; i--)
        {
            Vector3Int checkLocation = new Vector3Int(i, PlayerLocation.y);
            if (tilemap.HasTile(checkLocation))
            {
                nextLeft = checkLocation;
                break;
            }
        }
        return nextLeft;
    }
    private Vector3Int GetNextUp(Vector3Int PlayerLocation)
    {
        Vector3Int nextUp = new Vector3Int(PlayerLocation.x, NewBoard.Bounds.max.y);
        for (int i = PlayerLocation.y; i < NewBoard.Bounds.max.y; i++)
        {
            Vector3Int checkLocation = new Vector3Int(PlayerLocation.x, i);
            if (tilemap.HasTile(checkLocation))
            {
                nextUp = checkLocation;
                break;
            }
        }
        return nextUp;
    }


    public void CheckPlayerTrapped()
    {
        Vector3Int PlayerLocation = new Vector3Int(
            Mathf.FloorToInt(playerTransform.position.x),
            Mathf.FloorToInt(playerTransform.position.y)
        );

        if (CheckVerticalTrap(PlayerLocation))
        {
            Debug.Log("vertical trap");
        }

    }

    private bool CheckVerticalTrap(Vector3Int PlayerLocation)
    {
        /* check for vertical trap:
         *      X   X
         *      X   X
         *      X O X
         */

        Vector3Int nextRight = GetNextRight(PlayerLocation);
        Vector3Int nextLeft = GetNextLeft(PlayerLocation);

        // only trapped if in a 2 block wide space or smaller
        // (it says 3 because that's the difference between the x coordinates)
        if (Mathf.Abs(nextRight.x - nextLeft.x) > 3) return false;

        // check for walls to high to jump
        for (int i = 1; i <= playerMovementScript.jumpHeight; i++)
        {
            if (
                !tilemap.HasTile(nextRight + i * Vector3Int.up)
                || !tilemap.HasTile(nextLeft + i * Vector3Int.up)
            )
            {
                return false;
            }
        }

        // finally, determine the player is trapped
        return true;
    }
}
