using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_detector : MonoBehaviour
{

    // script Álvaro 
    public Vector3 enemy_position;
    public GameObject target_;

    public bool anyEnemy;
    void Start()
    {

    }

    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        {
            anyEnemy = true;
            enemy_position = other.transform.position;
            target_ = other.gameObject;

            //si tengo al enemigo cerca, almaceno su posicion
        }
    }private void OnTriggerExit(Collider other)
    {
        if(other.tag == "enemy")
        {
            anyEnemy = false;
        }
    }
}
