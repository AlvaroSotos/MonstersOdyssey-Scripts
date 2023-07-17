using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class Fade : MonoBehaviour
{
    // script Álvaro 

    new MeshRenderer renderer;
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }


    void Start()
    {
        StartCoroutine(Degradaito());
    }

     
    IEnumerator Degradaito()
    {        
        Color c = GetComponent<MeshRenderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            GetComponent<MeshRenderer>().material.color = c;            
            yield return new WaitForSeconds(0.2f);
        }

    }


    void Update()
    {
        
    }
}
