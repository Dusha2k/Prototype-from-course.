using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public float rangeToTarget, timeBetweenShots = .5f;
    private float shotCounter;

    public Transform gun, firePoint;
    public float rotateSpeed = 45f;
    void Start()
    {
        shotCounter = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToTarget)
        {
            gun.LookAt(PlayerController.instance.checkGroundPoint.position + new Vector3(0, 1.2f, 0));

            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
            }
        }
        else
        {
            shotCounter = timeBetweenShots;
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.Euler(0, gun.rotation.eulerAngles.y + 10f, 0), rotateSpeed * Time.deltaTime);
        }
    }
}
