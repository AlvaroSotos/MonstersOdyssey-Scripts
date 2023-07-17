using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock)
    {
        //si no hay vecinos cerca devuelve un vector cero
        if(context.Count == 0)
        {
            return Vector3.zero;
        }

        //si tengo la misma posicion o muy cercana a la de mis vecinos
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadiusMultiplier)
            {
                nAvoid++;
                avoidanceMove += (agent.transform.position - item.position);
            }
            
        }
        if(nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }

    
}
