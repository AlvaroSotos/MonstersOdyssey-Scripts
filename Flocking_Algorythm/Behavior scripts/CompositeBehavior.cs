using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : Flock_Behavior
{
    public Flock_Behavior[] behaviors;
    public float[] weights;
    public override Vector3 CalculateMove(Flock_Agent agent, List<Transform> context, Flock flock)
    {
        //Por si hay un error y la info no está bien enlazada
        if (behaviors.Length != weights.Length)
        {
            Debug.LogError("Error missmatch in " + name, this);
            return Vector3.zero;

        }
        Vector3 move = Vector3.zero;

        //itero cada comportamiento en el array y multipico su vector por el peso que tengan, despues lo sumo al vector de movimiento
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i]; //entra en un comportamiento y calcula su vector director, lo multiplica por su peso

            if (partialMove != Vector3.zero )
            {
                if(partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;

            }

        }
        return move;
    }
}
