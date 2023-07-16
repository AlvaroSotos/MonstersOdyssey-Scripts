using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


//[Serializable]
public class WaterController : MonoBehaviour
{

    int cantidad_clicks;//cantidad de clicks para combo combate
    bool puedo_dar_clicks;

    // script Álvaro 
    
    //[Serializable]
    enemy_detector enemy_detected;
    //[SerializeField]
    public GameObject character;
    public GameObject enemy;
    public GameObject main_menu;
    public GameObject options_menu;
    public CharacterController character_controller;
    public Transform cam;
    public CinemachineVirtualCamera virtual_cam;
    public GameObject virtual_camera_GO_;

    public PlayerInputActions player_controls;
    public Animator anim;
    public LayerMask layer_personaje;
    public Collider limit_detector;

    public BaseEnemyFSM EnemyScript;

    float TurnSmoothVelocity;
    float TurnSmoothTime = 0.1f;
    float UnderWaterLimit_Speed = 1f;

    public float camara_agachado;
    public float camara_inicial;
    public AxisState Camara_Agachado;
    public AxisState Camara_In;
    public float velocity = 1.0f;
    public float running_speed = 1f;
    public float crouch_speed;
    public float jump_force;
    public float gravity;
    public float enemycam_distance;
    public float distancia;
    public float dash_cd;
    public float dash_force_ = 1.0f;
    public float stun_velocity = 1.0f;
    float weight;
    float time = 0f;

    Vector3 player_velocity;
    public Vector3 move_direction = Vector3.zero;
    Vector2 input_movement_data = Vector2.zero;
    Vector3 LimitVector;

    bool need_cam_update;
    public bool isGrounded;
    
    public bool ended_dash_anim = true;
    public bool pressed_jump;
    public bool ended_jump_anim = true;
    bool pressed_autoaim;
    bool pressed_run;
    bool pressed_crouch;
    bool pressed_menu;
    public bool avancedegolpe=false;
    public float impulsodegolpe_;
    bool pressed_melee;
    bool isShooting;
    public bool pressed_dash;
    public bool isAngry;
    public bool onLimits = false;

    public ParticleSystem TornadoParticleSystem;
    public float tornado_attrack_force;




    public GameObject[] armas_;

    public void UseWeapons(int weapon_id)
    {
        if (armas_ != null)
        {
            if (weapon_id < armas_.Length)
            {
                armas_[weapon_id].GetComponent<PruebaDaño>().CreaZonaDeDaño();
            }
        }
 
    }
    public void desactivaWeapons(int weapon_id)
    {
        if (armas_ != null)
        {
            if (weapon_id < armas_.Length)
            {
                armas_[weapon_id].GetComponent<PruebaDaño>().EliminaZonaDeDaño();
            }
        }
   
    }

    private void Awake()
    {
        player_controls = new PlayerInputActions();

        player_controls.Player.melee.performed += melee_performed =>
        {
               
            pressed_melee = melee_performed.ReadValueAsButton();
            cantidad_clicks++;
            Debug.Log("CLicks : " + cantidad_clicks);
        };
        player_controls.Player.melee.canceled += melee_canceled =>
        {

                
            pressed_melee = melee_canceled.ReadValueAsButton();
        };

        player_controls.Player.Move.performed += Move_performed =>
        {
            input_movement_data = Move_performed.ReadValue<Vector2>();
        };
        player_controls.Player.Move.canceled += Move_canceled =>
        {
            input_movement_data = Move_canceled.ReadValue<Vector2>();
        };

        player_controls.Player.AutoAim.performed += AutoAim_performed =>
        {
            pressed_autoaim = AutoAim_performed.ReadValueAsButton();
        };
        player_controls.Player.AutoAim.canceled += AutoAim_canceled =>
        {
            pressed_autoaim = AutoAim_canceled.ReadValueAsButton();
        };

        player_controls.Player.Jump.performed += Jump_performed =>
        {
            pressed_jump = Jump_performed.ReadValueAsButton();
        };
        player_controls.Player.Jump.canceled += Jump_canceled =>
        {
            pressed_jump = Jump_canceled.ReadValueAsButton();
        };

        player_controls.Player.Dash.performed += Dash_performed =>
        {
            pressed_dash = Dash_performed.ReadValueAsButton();
        };
        player_controls.Player.Dash.canceled += Dash_performed =>
        {
            pressed_dash = Dash_performed.ReadValueAsButton();
        };
        player_controls.Player.crouch.performed += crouch_performed =>
        {
            pressed_crouch = crouch_performed.ReadValueAsButton();
        };
        player_controls.Player.crouch.canceled += crouch_canceled =>
        {
            pressed_crouch = crouch_canceled.ReadValueAsButton();
        };
        player_controls.Player.Running.performed += Running_performed =>
        {
            pressed_run = Running_performed.ReadValueAsButton();
        };
        player_controls.Player.Running.canceled += Running_canceled =>
        {
            pressed_run = Running_canceled.ReadValueAsButton();
        };

        player_controls.Player.Menu.performed += Menu_performed =>
        {
            pressed_menu = Menu_performed.ReadValueAsButton();
        };
        player_controls.Player.Menu.canceled += Menu_canceled =>
        {
            pressed_menu = Menu_canceled.ReadValueAsButton();
        }; ;

        player_controls.Player.Fire.performed += Fire_performed =>
        {
            isShooting = Fire_performed.ReadValueAsButton();
        };
        player_controls.Player.Fire.canceled += Fire_performed =>
        {
            isShooting = Fire_performed.ReadValueAsButton();
        };
        }

    private void OnEnable()
    {
        player_controls.Enable();
    }
    private void OnDisable()
    {
        player_controls.Disable();
    }
    void Start()
    {
        
 
    }
    public void Menu_Call()
    {
        if (pressed_menu)
        {
            if (!main_menu.activeInHierarchy)
            {
                main_menu.SetActive(true);
                pressed_menu = false;
                Time.timeScale = 0;
            }
            else
            {
                main_menu.SetActive(false);
                pressed_menu = false;
                Time.timeScale = 1;
            }
        }
    }
    public void Options()
    {
        main_menu.SetActive(false);
        options_menu.SetActive(true);
    }
    public void Back()
    {
        options_menu.SetActive(false);
        main_menu.SetActive(true);
    }
    public void Resume()
    {
        main_menu.SetActive(false);
        pressed_menu = false;
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }
    
    IEnumerator dash()
    {
        dash_force_ = 2.0f;
        pressed_dash = false;
        yield return new WaitForSeconds(0.1f);
        dash_force_ = 1.0f;
    }
    public void JumpAnimationBool()
    {
        ended_jump_anim = true;
        Debug.Log("fin salto");
    }
    public void DashAnimationBool()
    {
        ended_dash_anim = true;
        Debug.Log("fin dash");
        
    }

    void animations()
    {
        if (input_movement_data.magnitude > 0.0f)
        {
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsRunning", false);

            if (pressed_run)
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
                pressed_run = false;
            }
            

        }
        else
        {
            anim.SetBool("IsIdle", true);
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", false);

        }
      
        
        if (pressed_jump )
        {
            if (pressed_run)
            {
                anim.SetBool("Dash", true);
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
                anim.SetBool("Jump", false);

            }
            else
            {
                anim.SetBool("Jump", true);
                anim.SetBool("IsIdle", false);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsRunning", false);
                anim.SetBool("Dash", false);

            }

        }
        else
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Dash", false);
        }

    }
        
    public float ground_ray_dist_;
    void Update()
    {
      
        if (pressed_melee) { iniciar_combo(); }
        //Animator controller//
        animations();

        Menu_Call();

        isGrounded = character_controller.isGrounded;        

        if (input_movement_data != Vector2.zero)
        {
            move_direction = new Vector3(input_movement_data.x, 0.0f, input_movement_data.y);

            float TargetAngle = Mathf.Atan2(move_direction.x, move_direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //cojo el ángulo de rotación, lo paso a grados y le sumo la rotacion de la cam
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref TurnSmoothVelocity, TurnSmoothTime);//le hago smooth al angulo de rotación
            transform.rotation = Quaternion.Euler(0.0f, SmoothAngle, 0.0f);
            move_direction = Quaternion.Euler(0.0f, TargetAngle, 0.0f) * Vector3.forward;

            if (pressed_run  )
            {
                running_speed = 1.5f;
                isAngry = true;
            }
            else
            {
                running_speed = 1.0f;
                isAngry = false;
            }
            if (onLimits)
            {
                StartCoroutine(UnderWaterLimit());
                LimitVector = limit_detector.transform.right * weight;
                move_direction += LimitVector;
            } else { time = 0f; }
            if (EnemyScript.tornado)
            {
                Vector3 tornadoDir = (TornadoParticleSystem.transform.position - transform.position).normalized * tornado_attrack_force; 
                //vector direccion que atrae al jugador al tornado
                
                move_direction += tornadoDir;
            }




        }
        else
        {
            move_direction = LimitVector;
        }
        if (avancedegolpe == true)
        {
            impulsodegolpe_ = 1.3f;

        }
        else
            impulsodegolpe_ = 1f;

        character_controller.Move(move_direction * (velocity /** dash_force_ * crouch_speed*/* stun_velocity * UnderWaterLimit_Speed * impulsodegolpe_* running_speed) * Time.deltaTime);
        Debug.Log("atracción: " + move_direction);
        player_velocity.y += gravity * Time.deltaTime;

        if (isGrounded)
        {
            player_velocity.y = 0.0f;

        }

        if (pressed_jump)
        {
            player_velocity.y = -jump_force * gravity;
            ended_jump_anim = false;
        }
        

        character_controller.Move(player_velocity * Time.deltaTime);


        if (pressed_autoaim && enemy_detected.anyEnemy)
        {
            character.transform.LookAt(enemy_detected.enemy_position);
            virtual_cam.LookAt = enemy_detected.target_.transform;
            virtual_cam.AddCinemachineComponent<CinemachineHardLookAt>();
            need_cam_update = true;
        }
        if (need_cam_update && !pressed_autoaim)
        {
            virtual_cam.LookAt = character.transform;
            virtual_cam.AddCinemachineComponent<CinemachinePOV>();
            need_cam_update = false;
        }

    }


    void iniciar_combo()
    {

        if (cantidad_clicks == 1)
        {
            anim.SetInteger("attack", 1);
        }
    }
    public void Verificar_combo()
    {
        puedo_dar_clicks = false;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Animacion melee") && cantidad_clicks == 1)
        {
            anim.SetInteger("attack", 0);
            puedo_dar_clicks = true;
            cantidad_clicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Animacion melee") && cantidad_clicks >= 2)
        {
            anim.SetInteger("attack", 2);
            puedo_dar_clicks = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("metarig_Animaciones melee2") && cantidad_clicks == 2)
        {
            anim.SetInteger("attack", 0);
            puedo_dar_clicks = true;
            cantidad_clicks = 0;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("metarig_Animaciones melee2") && cantidad_clicks >= 3)
        {
            anim.SetInteger("attack", 3);
            puedo_dar_clicks = true;

        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("metarig_Animaciones patada"))
        {
            anim.SetInteger("attack", 0);
            puedo_dar_clicks = true;
            cantidad_clicks = 0;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StunAttack")
        {
            StartCoroutine(Stun());
            Debug.Log("TE STUNEA");
        }

    }
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "UnderWaterLimit")
        {
            onLimits = true;
            limit_detector = other;
            Debug.Log("estás en el límite");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "UnderWaterLimit")
        {
            onLimits = false;
            LimitVector = Vector3.zero;

            Debug.Log("sales del límite");
        }
    }
    IEnumerator UnderWaterLimit()
    {
        int length = 100;
        for (int i = 0; i < length; i++)
        {
            Debug.Log("tiempo" + time);
            time += 0.0001f;
            weight = time;
            yield return new WaitForSeconds(0.01f);
        }
    }
  
    IEnumerator Stun()
    {
        stun_velocity = 0.0f;
        //Animacion stun
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Stun", false);
        stun_velocity = 1.0f;
    }
    public void avanzo()
    {
        avancedegolpe = true;
    }
    public void no_avanzo()
    {
        avancedegolpe = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 a = transform.InverseTransformDirection(transform.up);
        
        Gizmos.DrawLine(transform.position, a * ground_ray_dist_);
    }

}
