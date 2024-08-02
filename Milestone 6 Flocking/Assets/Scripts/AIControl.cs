using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIControl : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator animator;
    float speedMultiplier;
    float detectionRadius = 15;
    float fleeRadius = 10;
    float idleDur = 10;
    void ResetAgent()
    {
        speedMultiplier = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMultiplier;
        agent.angularSpeed = 120;
        animator.SetFloat("speedMultiplier", speedMultiplier);
        animator.SetTrigger("isWalking");
        if (Random.Range(1, 10) > 5)
        {
            StartCoroutine(StartIdle());
        }
        else
        {
            agent.ResetPath();
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        animator.SetTrigger("isWalking");
        animator.SetFloat("wOffset", Random.Range(0.1f, 1f));
        if (Random.Range(1, 10) > 5)
        {
            StartCoroutine(StartIdle());
        }

        ResetAgent();
    }

    private IEnumerator StartIdle()
    {
        agent.isStopped = true;
        animator.SetTrigger("isIdle");
        yield return new WaitForSeconds(idleDur);
        agent.isStopped = false;
        animator.SetTrigger("isWalking");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }

    public void DetectNewMonster(Vector3 location)
    {
        agent.isStopped = false;
        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - location).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                animator.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
    public void DetectNewTempGoal(Vector3 location)
    {
        agent.isStopped = false;
        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            Vector3 flockDirection = (this.transform.position - location).normalized;
            Vector3 newGoal = this.transform.position - flockDirection;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                animator.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
}
