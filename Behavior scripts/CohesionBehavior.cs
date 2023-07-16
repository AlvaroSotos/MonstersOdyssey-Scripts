using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock)
    {
        //si no hay vecinos, no devuelvas ningún cambio
        if(context.Count == 0)
        {
            return Vector3.zero;
        }

        //Suma todas las posiciones cercanas y hace la media
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //Teniendo el punto medio grupal, mueve al agente al punto medio entre su posicion y el grupal
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }

}
