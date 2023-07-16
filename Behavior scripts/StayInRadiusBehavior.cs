using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay in radius")]
public class StayInRadiusBehavior : Flock_Behavior
{

    public Vector3 center;
    public float radius = 15f;
    public override Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if(t < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t; // multiplicas por el cuadrado para darle un efecto cuadrático
    }

}
