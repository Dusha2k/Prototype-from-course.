using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public bool isCollected;
    public int healthAmount = 3;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isCollected)
        {
            PlayerHealthController.instance.PickupHealth(healthAmount);
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(5);
        }
    }
}
