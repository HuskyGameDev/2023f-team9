using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/PlayerSpeedUp")]
public class PlayerSpeedUp : PowerUpEffect
{
    public float speedMultiplier = 2f;
    public override void Apply(GameObject target)
    {
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        playerMovement.ApplySpeedUp(speedMultiplier);
        
    }
}
