using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;
    public int damage;
    public bool damageEnemy, damagePlayer;

    public Rigidbody theRb;

    public GameObject destroyEffect;
    void Start()
    {
        
    }

    
    void Update()
    {
        theRb.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().EnemyDamage(damage);
        }

        if (other.gameObject.tag == "Headshot" && damageEnemy)
        {
            other.transform.parent.gameObject.GetComponent<EnemyHealthController>().EnemyDamage(damage * 2);
            Debug.Log("Headshot");
        }

        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.DamageToPlayer(damage);
        }
        Destroy(gameObject);
        Instantiate(destroyEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
