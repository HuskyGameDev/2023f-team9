using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/PlayerSpeedUp")]
// this is not just speed up but both jump and speed power up
public class PlayerSpeedUp : PowerUpEffect
{
    public float speedMultiplier = 1f;
    public float duration = 6f;
    public int jumpAdder = 1;

    public override void Apply(GameObject target)
    {
        int random = Random.Range(0, 2);
        Debug.Log("random number: " + random);
        // Access PlayerMovement script using GameManager
        PlayerMovement playerMovement = GameManager.Instance.PlayerMovement;
        if (random == 0)
        {
            playerMovement.ApplySpeedUp(speedMultiplier, duration);
        }
        else
        {
            playerMovement.ApplyJumpUp(jumpAdder, duration);
        }
        
       
    }
}
