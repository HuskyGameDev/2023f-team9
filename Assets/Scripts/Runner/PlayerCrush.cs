using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCrush : MonoBehaviour
{
    [SerializeField] private AudioClip CrushSound;
    private TilemapCollider2D tilemapCollider; // the board's tilemap collider
    private CircleCollider2D crushCollider;
    private AudioSource audioSource;
    private new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        crushCollider = this.GetComponentInChildren<CircleCollider2D>();
        audioSource = this.GetComponent<AudioSource>();
        rigidbody = this.GetComponentInChildren<Rigidbody2D>();
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
        rigidbody.simulated = false;
        audioSource.PlayOneShot(CrushSound);
        GameManager.Instance.GameOver(false);
    }
}
