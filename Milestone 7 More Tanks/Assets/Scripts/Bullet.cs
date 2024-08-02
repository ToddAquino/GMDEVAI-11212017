using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject explosion;
	public string type;

	void OnCollisionEnter(Collision col)
    {
		if(col.gameObject.tag == "enemy" || col.gameObject.tag == "Player")
		{
			Debug.Log("HIT!");
			col.gameObject.GetComponent<Health>().GetDamaged(5, this.type);
		}
        GameObject e = Instantiate(explosion, this.transform.position, Quaternion.identity);
    	Destroy(e,1.5f);
    	Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
