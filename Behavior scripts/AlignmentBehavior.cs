using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock)
    {
        //Si no hay vecinos, devuelve el vector forward
        if(context.Count == 0)
        {
            return agent.transform.forward;
        }

        //Suma todas las direcciones cercanas y hace la media
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += item.transform.forward;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
        

    }


}
