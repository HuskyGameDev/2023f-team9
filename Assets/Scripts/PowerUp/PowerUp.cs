using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;

    void Start()
    {

    }

    // trigger power up on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerUpEffect.Apply(collision.gameObject);
        }
        if (collision.CompareTag("Board"))
        {
            StayOnTopOfBoard(collision.gameObject.GetComponent<Tilemap>());
        }

    }
    private void StayOnTopOfBoard(Tilemap boardTilemap)
    {
        if (boardTilemap != null)
        {
            Vector3Int cellPosition = boardTilemap.WorldToCell(transform.position);
            Vector3 cellCenter = boardTilemap.GetCellCenterWorld(cellPosition);

            // Set the power-up's position to be just above the board
            transform.position = new Vector3(cellCenter.x, cellCenter.y, transform.position.z);
        }



    }
}