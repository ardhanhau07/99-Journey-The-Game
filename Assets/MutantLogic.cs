using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MutantLogic : MonoBehaviour
{
   public float hitPoints = 100f;
    public float turnSpeed = 15f;
    public Transform target;
    public float ChaseRange; // Jarak untuk mulai mengejar
    public float AttackRange; // Jarak untuk mulai menyerang
    public Transform[] waypoints; // Waypoints untuk patroli
    public float waypointTolerance = 1f; // Jarak toleransi untuk mencapai waypoint

    private NavMeshAgent agent;
    private float DistancetoTarget;
    private Animator anim;
    private int currentWaypointIndex = 0;
    private bool isDead = false;

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hitPoints -= damage;
        Debug.Log("TakeDamage called, HitPoints: " + hitPoints);
        anim.SetTrigger("GetHit");
        anim.SetFloat("Hitpoint", hitPoints);

        if (hitPoints <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        anim.SetTrigger("Death");
        agent.isStopped = true;
        Destroy(gameObject, 3f); // Hancurkan musuh setelah 3 detik
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("Hitpoint", hitPoints);
    }

    void Update()
    {
        if (isDead) return;

        DistancetoTarget = Vector3.Distance(target.position, transform.position);

        if (DistancetoTarget <= ChaseRange && hitPoints > 0)
        {
            FaceTarget(target.position);

            if (DistancetoTarget > AttackRange)
            {
                ChaseTarget();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            Patrol();
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void Attack()
    {
        Debug.Log("Attack");
        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", true);
        agent.ResetPath();
    }

    public void ChaseTarget()
    {
        Debug.Log("Chasing Target");
        agent.SetDestination(target.position);
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
    }

    public void Patrol()
    {
        if (waypoints.Length == 0) return;

        Debug.Log("Patrolling");
        anim.SetBool("Run", false);
        anim.SetBool("Walk", true);
        anim.SetBool("Attack", false);

        Transform waypoint = waypoints[currentWaypointIndex];
        agent.SetDestination(waypoint.position);

        if (Vector3.Distance(transform.position, waypoint.position) <= waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
