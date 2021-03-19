using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;
    public Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;

    public float keepChasingTimer = 5f;
    private float _chaseCounter;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate, waitBetweenShots = 2f, timeToShot = 1f;
    private float _fireCount, _shotWaitCounter, _shootTimeCounter;

    public Animator anim;

    private bool wasShot;
    void Start()
    {
        startPoint = transform.position;

        _shotWaitCounter = waitBetweenShots;
        _shootTimeCounter = timeToShot;
    }

    
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing)
        {
            if(Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;

                _shotWaitCounter = waitBetweenShots;
                _shootTimeCounter = timeToShot;
            }

            if(_chaseCounter > 0)
            {
                _chaseCounter -= Time.deltaTime;
                if (_chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }

            if (agent.remainingDistance < .25f)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
            
        }
        else
        {
            if(Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }

            if(Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                if (!wasShot)
                {
                    chasing = false;

                    _chaseCounter = keepChasingTimer;
                }
            }
            else
            {
                wasShot = false;
            }

            if(_shotWaitCounter > 0)
            {
                _shotWaitCounter -= Time.deltaTime;

                if(_shotWaitCounter <= 0)
                {
                    _shootTimeCounter = timeToShot;
                }
                anim.SetBool("isRunning", true);
            }
            else
            {
                if (PlayerController.instance.gameObject.activeInHierarchy)
                {
                    _shootTimeCounter -= Time.deltaTime;

                    if (_shootTimeCounter > 0)
                    {
                        _fireCount -= Time.deltaTime;

                        if (_fireCount <= 0)
                        {
                            _fireCount = fireRate;

                            firePoint.LookAt(PlayerController.instance.checkGroundPoint.position + new Vector3(0, 1.2f, 0));

                            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, transform.up);

                            if (Mathf.Abs(angle) < 30)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);
                                anim.SetTrigger("isShooting");
                            }
                            else
                            {
                                _shotWaitCounter = waitBetweenShots;
                            }

                        }

                        agent.destination = transform.position;
                    }
                    else
                    {
                        _shotWaitCounter = waitBetweenShots;
                    }
                }

                anim.SetBool("isRunning", false);
            }


        }
        
    }

    public void GetShot()
    {
        wasShot = true;

        chasing = true;

    }
}
