using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public bool collected;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !collected)
        {
            PlayerController.instance.activeGun.GetAmmo();

            collected = true;

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(3);
        }
    }
}
