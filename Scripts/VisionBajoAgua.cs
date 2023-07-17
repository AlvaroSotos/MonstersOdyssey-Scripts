using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisionBajoAgua : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject Camara;
    public Volume PostProceso;
    private ColorAdjustments filtro;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Agua")
        {
            PostProceso.profile.TryGet<ColorAdjustments>(out filtro);
            filtro.active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Agua")
        {
            PostProceso.profile.TryGet<ColorAdjustments>(out filtro);
            filtro.active = false;
        }
    }
}
