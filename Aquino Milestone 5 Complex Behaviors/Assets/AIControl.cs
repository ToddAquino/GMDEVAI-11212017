using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class AIControl : MonoBehaviour
{
    Vector3 wanderTarget;
    public NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;
    public int aggroDur = 0;
    public int aggroMaxDur;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    public void Pursue()
    {
        if (target == null)
        {
            return;
        }
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    public void Evade()
    {
        if (target == null)
        {
            return;
        }
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    public void Wander()
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3 (Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    public void Hide()
    {
        if (target == null)
        {
            return;
        }
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;
        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5;
            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                distance = spotDistance;
            }
        }
        Seek(chosenSpot);
    }

    public void CleverHide()
    {
        if (target == null)
        {
            return;
        }
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDirection = Vector3.zero;
        GameObject chosenGameObject = World.Instance.GetHidingSpots()[0];

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;
        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5;
            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);

            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                chosenDirection = hideDirection;
                chosenGameObject = World.Instance.GetHidingSpots()[i];
                distance = spotDistance;
            }
        }
        Collider hideCol = chosenGameObject.GetComponent<Collider>();
        Ray back = new Ray(chosenSpot, -chosenDirection.normalized);
        RaycastHit info;
        float rayDistance = 100.0f;
        hideCol.Raycast(back, out info, rayDistance);
        Seek(info.point + chosenDirection.normalized * 5); 
        //Seek(chosenSpot);
    }
    public bool canSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo,15.0f))
        {
            Debug.DrawLine(transform.position, transform.position + rayToTarget, Color.green);
            //Debug.Log(raycastInfo.transform.gameObject.name);
            return raycastInfo.transform.gameObject.tag == "Player";
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.CompareTag("Agent1"))
        {
            TMP_Text a1 = GameObject.Find("A1Status").GetComponent<TMP_Text>();
            if (!canSeeTarget() && aggroDur <=0)
            {
                a1.text = "Lost Player";
                Wander();
            }
            else
            {
                if (canSeeTarget())
                    aggroDur = aggroMaxDur;
                if (aggroDur > 0)
                {
                        aggroDur--;
                }
                a1.text = "Found Player";
                Pursue();
            }
        }
        else if (this.gameObject.CompareTag("Agent2"))
        {
            TMP_Text a2 = GameObject.Find("A2Status").GetComponent<TMP_Text>();
            if (!canSeeTarget() && aggroDur <= 0)
            {
                a2.text = "Lost Player";
                Wander();
            }
            else
            {
                if (canSeeTarget())
                    aggroDur = aggroMaxDur;
                if (aggroDur > 0)
                {
                    aggroDur--;
                }
                a2.text = "Found Player";
                Hide();
            }
        }
        else if (this.gameObject.CompareTag("Agent3"))
        {
            TMP_Text a3 = GameObject.Find("A3Status").GetComponent<TMP_Text>();
            if (!canSeeTarget() && aggroDur <= 0)
            {
                a3.text = "Lost Player";
                Wander();
            }
            else
            {
                if (canSeeTarget())
                    aggroDur = aggroMaxDur;
                if (aggroDur > 0)
                {
                    aggroDur--;
                }
                a3.text = "Found Player";
                Evade();
            }
        }
    }
}
