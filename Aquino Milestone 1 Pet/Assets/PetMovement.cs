using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    public Transform goal;
    public float speed = 2;
    public float rotSpeed = 4;
    public Transform playerForward;
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Spawn").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if(Input.GetKey(KeyCode.E))
        {
            goal = GameObject.FindGameObjectWithTag("Goal").transform;
        }
        else
        {
            goal = GameObject.FindGameObjectWithTag("Spawn").transform;
        }
        Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
        Vector3 direction = lookAtGoal - transform.position;

        //Move and face towards target
        if (Vector3.Distance(lookAtGoal, transform.position) > .5)
        { 
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);     
        transform.position = Vector3.Lerp(this.transform.position, goal.position, speed * Time.deltaTime);
        }
        //When done moving face forward based on the player's direction
        else
        {
            
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, playerForward.rotation, Time.deltaTime * rotSpeed);
        }
    }
}
    
