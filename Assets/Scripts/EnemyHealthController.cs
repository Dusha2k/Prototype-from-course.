using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int health;

    public EnemyController theEC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDamage(int damageAmount)
    {
        health -= damageAmount;

        if(theEC != null)
        {
            theEC.GetShot();
        }
        if(health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(2);
        }
    }
}
