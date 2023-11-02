using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// power up effect to be inherit from
public abstract class PowerUpEffect : ScriptableObject
{
    // target of the power up if apply to
    public abstract void Apply(GameObject target);
}
