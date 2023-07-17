using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class test_Cam : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public bool hola;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hola)
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset += new Vector3(0f, 0f, 2f);

        }

    }
}
