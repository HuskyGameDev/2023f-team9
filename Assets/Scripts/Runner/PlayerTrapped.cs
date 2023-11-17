using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class PlayerTrapped : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] private Sprite[] blockSprites;

    // Runtime Variables
    private GameObject cameraObject;
    private Camera cameraComponent;
    private Tilemap tilemap; // the board's tilemap
    private Transform playerTransform;
    private PlayerMovement playerMovementScript;

    void Start()
    {
        playerTransform = this.gameObject.transform.GetChild(0);
        playerMovementScript = this.GetComponentInParent<PlayerMovement>();
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        cameraComponent = cameraObject.GetComponent<Camera>();
        try
        {
            tilemap = FindAnyObjectByType<Tilemap>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
        }
    }

    private Vector3Int GetNextRight(Vector3Int PlayerLocation)
    {
        Vector3Int nextRight = new Vector3Int(NewBoard.Bounds.max.x, PlayerLocation.y);
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

        CheckVerticalTrap(PlayerLocation);
        CheckOneBlockTrap(PlayerLocation);
    }

    /* ========================= *
     *           CHECKS          *
     * ========================= */
    private void CheckVerticalTrap(Vector3Int PlayerLocation)
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
        int gapWidth = Mathf.Abs(nextRight.x - nextLeft.x) - 1;
        if (gapWidth > 2) return;

        // check for walls to high to jump
        for (int i = 1; i <= playerMovementScript.jumpHeight; i++)
        {
            if (
                !tilemap.HasTile(nextRight + i * Vector3Int.up)
                || !tilemap.HasTile(nextLeft + i * Vector3Int.up)
            )
            {
                return;
            }
        }

        // finally, we say the player is trapped
        StartCoroutine(
            VerticalTrap(
                (gapWidth == 1)
                    ? new int[] { nextLeft.x + 1 }
                    : new int[] { nextLeft.x + 1, nextRight.x - 1 }
            )
        );
    }
    private void CheckOneBlockTrap(Vector3Int PlayerLocation)
    {
        /* check for one block trap:
         *      X X X
         *      X O X
         *      X X X
         */
        if (
            tilemap.HasTile(new Vector3Int(PlayerLocation.x - 1, PlayerLocation.y))
            && tilemap.HasTile(new Vector3Int(PlayerLocation.x + 1, PlayerLocation.y))
            && tilemap.HasTile(new Vector3Int(PlayerLocation.x, PlayerLocation.y + 1))
            && tilemap.HasTile(new Vector3Int(PlayerLocation.x, PlayerLocation.y - 1))
        )
        {
            Vector3Int nextFreeUp = new Vector3Int(PlayerLocation.x, NewBoard.Bounds.max.y);
            for (int i = PlayerLocation.y + 2; i < NewBoard.Bounds.max.y; i++)
            {
                Vector3Int checkLocation = new Vector3Int(PlayerLocation.x, i);
                if (!tilemap.HasTile(checkLocation))
                {
                    nextFreeUp = checkLocation;
                    break;
                }
            }
            StartCoroutine(OneBlockTrap(nextFreeUp.y));
        }
    }

    /* ========================= *
     *         ANIMATIONS        *
     * ========================= */
    private IEnumerator ZoomInOnPlayer()
    {
        while (
            cameraComponent.orthographicSize > 3
            || Mathf.Abs(cameraObject.transform.position.y - playerTransform.position.y) > 0.1
            || Mathf.Abs(cameraObject.transform.position.x - playerTransform.position.x) > 0.1
        )
        {
            if (cameraComponent.orthographicSize > 3)
                cameraComponent.orthographicSize *= 0.99f;
            if (cameraObject.transform.position.y < playerTransform.position.y)
                cameraObject.transform.position += new Vector3(0, 0.1f, 0);
            else if (cameraObject.transform.position.y > playerTransform.position.y)
                cameraObject.transform.position -= new Vector3(0, 0.1f, 0);
            if (cameraObject.transform.position.x < playerTransform.position.x)
                cameraObject.transform.position += new Vector3(0.1f, 0, 0);
            else if (cameraObject.transform.position.x > playerTransform.position.x)
                cameraObject.transform.position -= new Vector3(0.1f, 0, 0);

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ResetCamera()
    {
        while (
            cameraComponent.orthographicSize < 10
            || Mathf.Abs(cameraObject.transform.position.y) > 0.1
            || Mathf.Abs(cameraObject.transform.position.x) > 0.1
        )
        {
            if (cameraComponent.orthographicSize < 10)
                cameraComponent.orthographicSize *= 1.01f;
            if (cameraObject.transform.position.y < 0)
                cameraObject.transform.position += new Vector3(0, 0.1f, 0);
            else if (cameraObject.transform.position.y > 0)
                cameraObject.transform.position -= new Vector3(0, 0.1f, 0);
            if (cameraObject.transform.position.x < 0)
                cameraObject.transform.position += new Vector3(0.1f, 0, 0);
            else if (cameraObject.transform.position.x > 0)
                cameraObject.transform.position -= new Vector3(0.1f, 0, 0);

            yield return new WaitForEndOfFrame();
        }
        cameraComponent.orthographicSize = 10;
        cameraComponent.transform.position = new Vector3(0, 0, -10);
    }

    private IEnumerator VerticalTrap(int[] crushX)
    {
        GameManager.Instance.SetAllowPieceSpawn(false);

        yield return ZoomInOnPlayer();

        GameObject crusherBlock = new GameObject("Crusher Block");
        SpriteRenderer spriteRenderer = crusherBlock.AddComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = crusherBlock.AddComponent<BoxCollider2D>();
        Rigidbody2D rigidbody = crusherBlock.AddComponent<Rigidbody2D>();

        spriteRenderer.sprite = blockSprites[UnityEngine.Random.Range(0, blockSprites.Length - 1)];
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.sortingOrder = 2;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.gravityScale = 0.5f;
        rigidbody.mass = 100;
        rigidbody.velocity = new Vector2(0, -5f);

        int blockWidth = crushX.Length;
        spriteRenderer.size = new Vector2(blockWidth, 4);
        boxCollider.size = new Vector2(blockWidth - 0.1f, 4);
        crusherBlock.transform.position = new Vector3(blockWidth == 1 ? crushX[0] + 0.5f : crushX[^1], 0, 0);
    }
    private IEnumerator OneBlockTrap(int nextOpenY)
    {
        GameManager.Instance.SetAllowPieceSpawn(false);

        yield return ZoomInOnPlayer();

        yield return new WaitForSeconds(1);

        playerTransform.position = new Vector3(playerTransform.position.x, nextOpenY);
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, nextOpenY, cameraObject.transform.position.z);

        while (cameraObject.transform.position.y < nextOpenY)
        {
            cameraObject.transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        yield return ResetCamera();
        GameManager.Instance.SetAllowPieceSpawn(true);
    }
}
