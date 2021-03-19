using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string theGun;

    public bool collected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            PlayerController.instance.AddGun(theGun);

            collected = true;

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);
        }
    }
}
