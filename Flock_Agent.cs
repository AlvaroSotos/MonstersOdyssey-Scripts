using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flock_Agent : MonoBehaviour
{

    
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider agent_collider;
    public Collider agent_collider_ { get { return agent_collider; } }

    void Start()
    {
        agent_collider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;  //iguala una variable de tipo Flock a una de tipo agentFlock?
    }
    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
    
}
