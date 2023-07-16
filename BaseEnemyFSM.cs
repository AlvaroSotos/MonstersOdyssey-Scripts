using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyFSM : MonoBehaviour
{
    // script Álvaro 
    public enum MindStates
    {
        kWait,
        kSeek,
        kPursuit,
        kAttack,
        kMeleeAttack,
        kFlee
    }

    /*ParticleSystem System
    {
        get
        {
            if (_CachedSystem == null)
                _CachedSystem = GetComponent<ParticleSystem>();
            return _CachedSystem;
        }
    }*/
    public ParticleSystem TornadoParticleSystem;
    public ParticleSystem Tornado2ParticleSystem;
    public ParticleSystem Tornado3ParticleSystem;
    public ParticleSystem Tornado4ParticleSystem;
    public MindStates current_mind_state_;

    public Sight sight_sensor_;
    public NavMeshAgent agent_;
    public CController controller;
    public GameObject sound_attack;
    public Adaptative_pool pool;

    public Animator anim;
    public GameObject cabeza;
    public float sound_speed;
    public float slow_attack_speed;
    public float enemy_attack_cd;
    public float next_enemy_attack;
    public float attack_distance_ = 0.0f;
    public float stun_attack_distance_ = 0.0f;
    public float stop_attack_distance_multiplier = 1.2f;
    public float stun_time_ = 1.0f;
    public float tornado_positioning_time;
    public float tornado_attack_cd = 3.0f;
    public float tornado_velocity;
    bool hit_stun;
    bool tornado_cd_bool= true;
    public bool tornado= false;
    ParticleSystem.Particle[] particles;

    public Vector3 tornado_attrack_vector;
    public float tornado_attrack_force = 1f;

    public CharacterController controller_player_;
    public WaterController player_;
    public CharacterController ref_tornado;
    public GameObject go_ref_tornado;
    private void Awake()
    {
        agent_ = GetComponentInParent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TornadoParticleSystem.isPlaying == false)
        {
            tornado = false;
        }

        /*
        if (Input.GetKey("1"))
        {
            anim.SetBool("Idle", true);
        }else anim.SetBool("Idle", false);
        if (Input.GetKey("2"))
        {
            anim.SetBool("Nado", true);
        }
        else anim.SetBool("Nado", false);
        if (Input.GetKeyDown("3"))
        {
            anim.SetBool("Ataque_Sonido", true);
        }
        else anim.SetBool("Ataque_Sonido", false);
        if (Input.GetKeyDown("4"))
        {
            anim.SetBool("Ataque_Tornado", true);
        }
        else anim.SetBool("Ataque_Tornado", false);
        */

        if (current_mind_state_ == MindStates.kWait)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Nado", false);
            anim.SetBool("Ataque_Sonido", false);
            anim.SetBool("Ataque_Tornado", false);
            MindWait();

        }else if(current_mind_state_ == MindStates.kSeek)
        {

            anim.SetBool("Idle", true);
            anim.SetBool("Ataque_Sonido", false);
            anim.SetBool("Ataque_Tornado", false);
            MindSeek();

        }else if(current_mind_state_ == MindStates.kPursuit)
        {


            anim.SetBool("Nado", true);
            anim.SetBool("Idle", false);
            MindPursuit();
        }
        else if (current_mind_state_ == MindStates.kAttack)
        {
            

            MindAttack();
        }
        else if (current_mind_state_ == MindStates.kMeleeAttack && tornado_cd_bool /*&& !hit_stun*/)
        {
           
                
                MindStunAttack();

        }
        else if(current_mind_state_ == MindStates.kFlee)
        {
            anim.SetBool("Nado", true);
            MindFlee();
        }
    }
    
    void MindWait()
    {
        BodyWait();
        Debug.Log("Waiting");
        //añadir corrutina stun time
        StartCoroutine(Waiting());
        current_mind_state_ = MindStates.kSeek;

    }
    void BodyWait()
    {
        
        agent_.isStopped = true;
    }
   IEnumerator Waiting()
   {
        yield return new WaitForSeconds(stun_time_);
   }
    void MindSeek()
    {
        BodySeek();
        //Debug.Log("Seeking");
        
        if(sight_sensor_.detected_object_ != null)
        {
            current_mind_state_ = MindStates.kPursuit;
        }
        /*if (sight_sensor_.detected_object_ != null && controller.isAngry)
        {
            current_mind_state_ = MindStates.kFlee;
        }*/

    }
    void BodySeek()
    {
        
        //rotate o algo
        //StartCoroutine(Seeking());

    }
    void MindPursuit()
    {
        BodyPursuit();
        Debug.Log("Following");

        if(sight_sensor_.detected_object_ == null)
        {
            current_mind_state_ = MindStates.kWait;
            return;
        }

        float distance_to_target = Vector3.Distance(transform.position, sight_sensor_.detected_object_.transform.position);
        if(distance_to_target <= stun_attack_distance_)
        {
            current_mind_state_ = MindStates.kMeleeAttack;
        }
        else if(distance_to_target <= attack_distance_ && distance_to_target > stun_attack_distance_)
        {
            current_mind_state_ = MindStates.kAttack;
        }

        /*if (controller.isAngry)
        {
            current_mind_state_ = MindStates.kFlee;
        }*/
        
    }
    void BodyPursuit()
    {
        if (agent_ != null && sight_sensor_.detected_object_ != null)
        {
            
            agent_.isStopped = false;
            agent_.SetDestination(sight_sensor_.detected_object_.transform.position);
        }
    }
    void MindAttack()
    {
        anim.SetBool("Ataque_Sonido", true);
        anim.SetBool("Ataque_Tornado", false);
        anim.SetBool("Nado", false);
        SoundAttack();
        Debug.Log("Attacking");

        if (sight_sensor_.detected_object_ == null)
        {
            current_mind_state_ = MindStates.kWait;
            return;
        }

        float distance_to_target = Vector3.Distance(transform.position, sight_sensor_.detected_object_.transform.position);
        if (distance_to_target > attack_distance_ * stop_attack_distance_multiplier)
        {
            current_mind_state_ = MindStates.kWait;
        }
    }
    
    void MindStunAttack()
    {
        
        StunAttack();
        Debug.Log("Stunning_attack");

        if (sight_sensor_.detected_object_ == null)
        {
            current_mind_state_ = MindStates.kWait;
            return;
        }

        float distance_to_target = Vector3.Distance(transform.position, sight_sensor_.detected_object_.transform.position);
        if (distance_to_target > attack_distance_ * stop_attack_distance_multiplier)
        {
            current_mind_state_ = MindStates.kWait;
        }
    }
    IEnumerator sound_attack_coroutine(/*Vector3 Enemy_position*/)
    {
        sound_attack = pool.GetPoolObject_1();
        sound_attack.SetActive(true);
        
        sound_attack.transform.position = cabeza.transform.position;
        sound_attack.transform.rotation = cabeza.transform.rotation;
        sound_attack.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        sound_attack.GetComponent<Rigidbody>().AddForce(transform.forward * sound_speed, ForceMode.VelocityChange);
        StartCoroutine(attack_scale());
        yield return new WaitForSeconds(2f);
    }
    
    IEnumerator stun_attack_coroutine(/*Vector3 Enemy_position*/)
    {
        sound_attack = pool.GetPoolObject_2();
        sound_attack.SetActive(true);
        sound_attack.transform.position = transform.position;
        sound_attack.transform.rotation = transform.rotation;
        sound_attack.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        sound_attack.GetComponent<Rigidbody>().AddForce(transform.forward * slow_attack_speed, ForceMode.VelocityChange);
        StartCoroutine(stun_coroutine());
        yield return new WaitForSeconds(3f);
    }
    IEnumerator stun_coroutine()
    {
        hit_stun = true;
        yield return new WaitForSeconds(6.0f);
        hit_stun = false;
    }
    IEnumerator attack_scale()
    {
        int length = 300;
        for (int i = 0; i < length; i++)
        {
            sound_attack.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
        
    }
    void SoundAttack()
    {
        agent_.isStopped = true;
        if(sight_sensor_.detected_object_!= null)
        {
            transform.LookAt(sight_sensor_.detected_object_.transform.position);
        }
        //atacar o algo
        if (Time.time > next_enemy_attack)
        {
            next_enemy_attack = Time.time + enemy_attack_cd;
            //StartCoroutine(sound_attack_coroutine(/*sight_sensor_.detected_object_.transform.position*/));
        }
        
    }
    void StunAttack()
    {
        anim.SetBool("Ataque_Tornado", true);
        anim.SetBool("Ataque_Sonido", false);
        anim.SetBool("Nado", false);
        agent_.isStopped = true;
        if (sight_sensor_.detected_object_ != null)
        {
            transform.LookAt(sight_sensor_.detected_object_.transform.position);
            
        }
        //atacar o algo
        if (Time.time > next_enemy_attack && !hit_stun)
        {
            //next_enemy_attack = Time.time + enemy_attack_cd;

            //StartCoroutine(Tornado());
            //particles = new ParticleSystem.Particle[TornadoParticleSystem.main.maxParticles];
            //particles[1].velocity = Vector3.forward * 10 ;

            //StartCoroutine(stun_attack_coroutine(/*sight_sensor_.detected_object_.transform.position*/));
        }
    }
    void MindFlee()
    {
        BodyFlee();
        //Debug.Log("Fleeing");
    }
    void BodyFlee()
    {
        Vector3 FleeVector;
        if (sight_sensor_.detected_object_ != null && agent_ != null)
        {
            FleeVector = Vector3.Reflect(transform.position - sight_sensor_.detected_object_.transform.position, Vector3.up);
            agent_.isStopped = false;
            agent_.SetDestination(FleeVector);
        }
        if (!controller.isAngry)
        {
            current_mind_state_ = MindStates.kWait;
        }
    }
    
     IEnumerator Seeking ()
     {
        agent_.transform.rotation = Quaternion.Euler(0f,-180f, 0f);
        yield return new WaitForSeconds(2f);
        agent_.transform.rotation = Quaternion.Euler(0f, 180f, 0f);   
     }
    void Attackfixer()
    {
        if(tornado_cd_bool == false)
        {
            MindAttack();
        }
    }
    IEnumerator tornado_cd()
    {
        tornado_cd_bool = false;
        yield return new WaitForSeconds(tornado_attack_cd);
        tornado_cd_bool = true;
    }
    IEnumerator Tornado()
    {
        if (tornado_cd_bool)
        {
            ParticleSystem.VelocityOverLifetimeModule tornado_speed = TornadoParticleSystem.velocityOverLifetime;
            ParticleSystem.VelocityOverLifetimeModule tornado_speed2 = Tornado2ParticleSystem.velocityOverLifetime;
            ParticleSystem.VelocityOverLifetimeModule tornado_speed3 = Tornado3ParticleSystem.velocityOverLifetime;
            ParticleSystem.VelocityOverLifetimeModule tornado_speed4 = Tornado4ParticleSystem.velocityOverLifetime;

            TornadoParticleSystem.transform.position = transform.position;
            //go_ref_tornado.transform.position = transform.position;
            if (sight_sensor_.detected_object_ != null)
            {
                //Vector3 tornado_final_position = sight_sensor_.detected_object_.transform.position;       

                //go_ref_tornado.transform.position

                /*Vector3 tornado_dir = (sight_sensor_.detected_object_.transform.position - transform.position).normalized;
                tornado_speed.enabled = true;
                tornado_speed.space = ParticleSystemSimulationSpace.World;
                tornado_speed.x = tornado_dir.x * tornado_velocity;
                tornado_speed.z = tornado_dir.z * tornado_velocity;
                Tornado2ParticleSystem.transform.position = transform.position;
                tornado_speed2.enabled = true;
                tornado_speed2.space = ParticleSystemSimulationSpace.World;
                tornado_speed2.x = tornado_dir.x * tornado_velocity;
                tornado_speed2.z = tornado_dir.z * tornado_velocity;
                Tornado3ParticleSystem.transform.position = transform.position;
                tornado_speed3.enabled = true;
                tornado_speed3.space = ParticleSystemSimulationSpace.World;
                tornado_speed3.x = tornado_dir.x * tornado_velocity;
                tornado_speed3.z = tornado_dir.z * tornado_velocity;
                Tornado4ParticleSystem.transform.position = transform.position;
                tornado_speed4.enabled = true;
                tornado_speed4.space = ParticleSystemSimulationSpace.World;
                tornado_speed4.x = tornado_dir.x * tornado_velocity;
                tornado_speed4.z = tornado_dir.z * tornado_velocity;

                ref_tornado.Move(tornado_dir * tornado_velocity);*/

                TornadoParticleSystem.Play(true);
                StartCoroutine(tornado_cd());

                yield return new WaitForSeconds(tornado_positioning_time);

                /*if(sight_sensor_.detected_object_ != null)
                {
                    TornadoParticleSystem.transform.position = sight_sensor_.detected_object_.transform.position;
                }*/
                //Vector3 final_pos = new Vector3(tornado_dir.x, 0f, tornado_dir.z);

                TornadoParticleSystem.transform.position = go_ref_tornado.transform.position;
                tornado_speed.x = 0f;
                tornado_speed.z = 0f;
                tornado_speed2.x = 0f;
                tornado_speed2.z = 0f;
                tornado_speed3.x = 0f;
                tornado_speed3.z = 0f;
                tornado_speed4.x = 0f;
                tornado_speed4.z = 0f;
                tornado = true;

                //player_.move_direction = (TornadoParticleSystem.transform.position - player_.transform.position).normalized/* * tornado_attrack_force*/; //vector direccion que atrae al jugador al tornado
                //controller_player_.Move(player_.move_direction * tornado_attrack_force);
                
            }


            
        }
       
        



        //ParticleSystem.VelocityOverLifetimeModule[] tornado_speedProperty = new ParticleSystem.VelocityOverLifetimeModule[4];
        /* if (!TornadoParticleSystem.isPlaying)
         {
             for (int i = 0; i < 4; i++)
             {
                 if (i == 0)
                 {
                     tornado_speedProperty[i] = TornadoParticleSystem.velocityOverLifetime;
                     TornadoParticleSystem.transform.position = transform.position;
                     Vector3 tornado_dir = (sight_sensor_.detected_object_.transform.position - transform.position).normalized;
                     tornado_speed.enabled = true;
                     tornado_speed.space = ParticleSystemSimulationSpace.World;
                     tornado_speed.x = tornado_dir.x * tornado_velocity;
                     tornado_speed.z = tornado_dir.z * tornado_velocity;
                     //TornadoParticleSystem.Play(true);

                 }
                 else
                 {
                     tornado_speedProperty[i] = TornadoParticleSystem.gameObject.transform.GetChild(i - 1).gameObject.GetComponent<ParticleSystem>().velocityOverLifetime;
                     Vector3 tornado_dir = (sight_sensor_.detected_object_.transform.position - transform.position).normalized;
                     TornadoParticleSystem.transform.GetChild(i - 1).position = transform.position;
                     tornado_speedProperty[i].enabled = true;
                     tornado_speedProperty[i].space = ParticleSystemSimulationSpace.World;
                     tornado_speedProperty[i].x = tornado_dir.x * tornado_velocity;
                     tornado_speedProperty[i].z = tornado_dir.z * tornado_velocity;
                     //TornadoParticleSystem.transform.GetChild(i - 1).gameObject.GetComponent<ParticleSystem>().Play(true);

                 }

             }
             TornadoParticleSystem.Play(true);
             yield return new WaitForSeconds(tornado_positioning_time);
             tornado_speed.x = 0f;
             tornado_speed.z = 0f;            
         }*/


    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attack_distance_);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stun_attack_distance_);

    }

    
}
