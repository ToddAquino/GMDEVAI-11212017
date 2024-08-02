using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetDamaged(float damage, string type)
    {
        if(type != this.tag)
        {
            currentHealth -= damage;
        }
    }
}
