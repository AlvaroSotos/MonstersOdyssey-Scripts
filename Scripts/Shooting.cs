using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public GameObject pistola;
    public PlayerInputActions player_controls;
    public InputAction shoot;
    public Adaptative_pool pool;

    public float cd_time = 0.2f;
    public float Next_bullet;


    bool isShooting;

    public float bullet_speed;
    void Start()
    {
        
        
    }

    private void Awake()
    {

        //pool = GetComponent<Adaptative_pool>();

        player_controls = new PlayerInputActions();

    }


        
       
    
    void Update()
    {
        if (isShooting && Time.time > Next_bullet)
        {
            Next_bullet = Time.time + cd_time;

            bullet = pool.GetPoolObject_1();
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * bullet_speed, ForceMode.VelocityChange);
            
           
        }
    }

    private void OnEnable()
    {
        player_controls.Enable();
    }

    private void OnDisable()
    {
        player_controls.Disable();

    }
}
