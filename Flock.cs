using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Flock_Agent agentPrefab;
    List<Flock_Agent> agents = new List<Flock_Agent>();
    public Flock_Behavior behavior;

    [Range(10, 500)]
    public int startingCount;
    const float agentDensity = 0.3f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.2f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadiusMultiplier;
    public float SquareAvoidanceRadiusMultiplier { get { return squareAvoidanceRadiusMultiplier; } }

    public GameObject flockAzul, flockAmarillo, flockRojo;
    public LayerMask flocks_layer;
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadiusMultiplier = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Flock_Agent newAgent = Instantiate(agentPrefab, Random.insideUnitSphere * startingCount * agentDensity + new Vector3(0.0f, 15.0f, 50.0f), Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this); //inicializa este flock a Flock
            newAgent.tag = "flock";
            /*if (newAgent.GetComponent<MeshRenderer>().sharedMaterial == flockAzul.GetComponent<MeshRenderer>().sharedMaterial)
            {
                newAgent.tag = "flockAzul";

            }else if (newAgent.GetComponent<MeshRenderer>().sharedMaterial == flockAmarillo.GetComponent<MeshRenderer>().sharedMaterial)
            {
                newAgent.tag = "flockAmarillo";

            }else if (newAgent.GetComponent<MeshRenderer>().sharedMaterial == flockRojo.GetComponent<MeshRenderer>().sharedMaterial)
            {
                newAgent.tag = "flockRojo";
            }*/
            agents.Add(newAgent);
            
        }
    }

    void Update()
    {
        foreach (Flock_Agent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //El color de cada elemento cambia según el numero de elementos que tenga cerca
            //agent.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects (Flock_Agent agent) //funcion que devuelve una lista de transform.      #Intentar aqui el filtro de flocks#
    {
        
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius/*, flocks_layer*/);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.agent_collider_ && c.tag == "flock")
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
    /*private void OnDrawGizmos()
    {
        foreach (Flock_Agent agent in agents)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(agent.transform.position, neighborRadius * 2f);
        }
        
    }*/
}
