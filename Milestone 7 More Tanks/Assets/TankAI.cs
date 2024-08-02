using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    Animator animator;
    public GameObject player;
    public GameObject bullet;
    public GameObject turret;

    public GameObject GetPlayer()
    {
        return player;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            animator.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
            animator.SetFloat("health", this.GetComponent<Health>().currentHealth);
        }
    }

    void Fire()
    {
        if (player != null)
        {
            GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
            b.GetComponent<Bullet>().type = this.tag;
            b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
        }
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }
    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }
}
