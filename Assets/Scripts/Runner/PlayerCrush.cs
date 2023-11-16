using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCrush : MonoBehaviour
{
    // Components
    private TilemapCollider2D tilemapCollider; // the board's tilemap collider
    private CircleCollider2D crushCollider;

    // Start is called before the first frame update
    void Start()
    {
        crushCollider = this.GetComponentInChildren<CircleCollider2D>();
        try
        {
            tilemapCollider = FindAnyObjectByType<TilemapCollider2D>();
        }
        catch
        {
            // do nothing, it doesn't matter if there is no tilemap for now
        }
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

    public void CheckPlayerTrapped()
    {
        Debug.Log("Checking trapped");

    }
}
