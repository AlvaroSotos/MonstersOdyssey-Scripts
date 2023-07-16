using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flock_Behavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock);
}
