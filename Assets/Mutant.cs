using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Mutant : MonoBehaviour
{
    public float hitPoints = 100f;
    public float turnSpeed = 15f;
    //public Transform target;
    public GameObject target;
    public float ChaseRange;
    private NavMeshAgent agent;
    private float DistancetoTarget;
    private float DistancetoDefault;
    private Animator anim;
    public PlayerLogic playerLogic;
    Vector3 DefaultPosition;
    //public MazeLogic MutantMaze;

    [Header("Mutant SFX")]
    public AudioClip GethitAudio;
    public AudioClip StepAudio;
    public AudioClip AttackSwingAudio;
    public AudioClip AttackConnectAudio;
    public AudioClip DeathAudio;
    AudioSource MutantAudio;

    [Header("Mutant VFX")]
    public ParticleSystem Slash;


    private void Start()
    {
       // MutantMaze = FindObjectOfType<MazeLogic>();
        target = GameObject.FindGameObjectWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        anim.SetFloat("Hitpoint", hitPoints);
        DefaultPosition = this.transform.position;
        MutantAudio = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        DistancetoTarget = Vector3.Distance(target.transform.position, transform.position);
        DistancetoDefault = Vector3.Distance(DefaultPosition, transform.position);

        if (DistancetoTarget <= ChaseRange && hitPoints != 0)
        {
            FaceTarget(target.transform.position);
            if (DistancetoTarget > agent.stoppingDistance + 2f)
            {
                ChaseTarget();
                Slash.Stop();
            }
            else if (DistancetoTarget <= agent.stoppingDistance)
            {
                Attack();
            }
        }
        else if (DistancetoTarget >= ChaseRange * 2)
        {
            agent.SetDestination(DefaultPosition);
            FaceTarget(DefaultPosition);
            if(DistancetoDefault <= agent.stoppingDistance)
            {
                Debug.Log("Time to stop");
                anim.SetBool("Run", false);
                anim.SetBool("Attack", false);
            }
        }
    }

    public void SlashToggleOn(){
        Slash.Play();
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void Attack()
    {
        Debug.Log("attack");
        anim.SetBool("Run", false);
        anim.SetBool("Attack", true);
    }

    public void ChaseTarget()
    {
        agent.SetDestination(target.transform.position);
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
    public void TakeDamage(float damage)
    {
        MutantAudio.clip = GethitAudio;
        MutantAudio.Play();
        hitPoints -= damage;
        anim.SetTrigger("GetHit");
        anim.SetFloat("Hitpoint", hitPoints);
        if (hitPoints <= 0)
        {
            MutantAudio.clip = DeathAudio;
            MutantAudio.Play();
            anim.SetTrigger("Death");
            Destroy(gameObject, 3f);
           // MutantMaze.MutantCount -= 1;
        }

    }

    public void HitConnect()
    {
        MutantAudio.clip = AttackSwingAudio;
        MutantAudio.Play();
        if(DistancetoTarget <= agent.stoppingDistance)
        {
            MutantAudio.clip = AttackConnectAudio;
            MutantAudio.Play();
            target.GetComponent<PlayerLogic>().PlayerGetHit(50f);
        }
    }

    public void step(){
        MutantAudio.clip = StepAudio;
        MutantAudio.Play();
    }
}
