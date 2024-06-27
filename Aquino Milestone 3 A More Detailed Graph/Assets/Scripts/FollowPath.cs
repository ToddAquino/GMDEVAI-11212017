using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPath : MonoBehaviour
{
    Transform goal;
    float speed = 5.0f;
    float accuracy = 1.0f;
    float rotSpeed = 2.0f;
    public GameObject wpManager;
    GameObject[] wps;
    public GameObject currentNode;
    int currentWaypointIndex = 0;
    public Transform Tank;

    Graph graph;
    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WaypointManager>().waypoints;
        graph = wpManager.GetComponent<WaypointManager>().graph;
        //currentNode = wps[0];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(graph.getPathLength() == 0 || currentWaypointIndex == graph.getPathLength())
        {
            return;
        }
        // closest node
        currentNode = graph.getPathPoint(currentWaypointIndex);
        // if object is close enough(based on accuracy) to the current waypoint move to the next waypoint
        if (Vector3.Distance(graph.getPathPoint(currentWaypointIndex).transform.position, Tank.position) < accuracy)
        {
            currentWaypointIndex++;
        }
       
        if(currentWaypointIndex < graph.getPathLength())
        {
            goal = graph.getPathPoint(currentWaypointIndex).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, Tank.position.y,goal.position.z);
            Vector3 direction = lookAtGoal - Tank.position;
            Tank.rotation = Quaternion.Slerp(Tank.rotation,Quaternion.LookRotation(direction),Time.deltaTime * rotSpeed);

            Tank.Translate(0, 0, speed * Time.deltaTime);
        }
    }
    public void SelectG()
    {
        Tank = GameObject.FindWithTag("Green").transform;

        currentNode = wps[13]; 
    }
    public void SelectB()
    {
        Tank = GameObject.FindWithTag("Blue").transform;

        currentNode = wps[12];
   
    }
    public void SelectR()
    {
        Tank = GameObject.FindWithTag("Red").transform;
        currentNode = wps[11];
    }

    public void GoToTwinMountains()
    {
        this.graph.AStar(currentNode, wps[10]);
        this.currentWaypointIndex = 0;
    }

    public void GoToBarracks()
    {

        this.graph.AStar(currentNode, wps[2]);
        this.currentWaypointIndex = 0;

    }

    public void GoToCommandCenter()
    {

        this.graph.AStar(currentNode, wps[14]);
        this.currentWaypointIndex = 0;
    }

    public void GoToOilPumps()
    {

        this.graph.AStar(currentNode, wps[4]);
        this.currentWaypointIndex = 0;
    }
    public void GoToTankers()
    {

        this.graph.AStar(currentNode, wps[5]);
        this.currentWaypointIndex = 0;

    }
    public void GoToRadar()
    {
        graph.AStar(currentNode, wps[12]);
        currentWaypointIndex = 0;
    }
    public void GoToCommandPost()
    {
        this.graph.AStar(currentNode, wps[8]);
        this.currentWaypointIndex = 0;
    }
    public void GoToCenter()
    {
        this.graph.AStar(currentNode, wps[9]);
        this.currentWaypointIndex = 0;
    }
}
