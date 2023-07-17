using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(Flock_Agent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original)
        {
            Flock_Agent itemAgent = item.GetComponent<Flock_Agent>();
            if(itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) // no entiendo la segunda condicion
            {
                filtered.Add(item);
            }
            /*if(agent.tag == item.tag)
            {
                filtered.Add(item);
                Debug.Log("holi");
            }*/
        }
        return filtered;
    }
}
