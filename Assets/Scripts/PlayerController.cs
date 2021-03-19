using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpForce, runSpeed;
    public CharacterController charCon;
    public Transform camTransform;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool _canJump;
    private bool _canDoubleJump;
    public Transform checkGroundPoint;
    public LayerMask whatIsGround;

    private Vector3 _moveInput;
    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun;

    public Transform adsPoint, gunHolder;
    public Vector3 gunStartPos;
    public float adsSpeed = 2f;

    public AudioSource soundWalk;
    public AudioSource soundRun;

    private float _bounceAmount;
    private bool _bounce;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentGun--;
        SwitchGun();
        gunStartPos = gunHolder.localPosition;
    }

    
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy && !GameManager.instance.ending)
        {
            float yStore = _moveInput.y;
            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            _moveInput = vertMove + horiMove;
            _moveInput.Normalize();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _moveInput *= runSpeed;
            }
            else
            {
                _moveInput *= moveSpeed;
            }
            _moveInput.y = yStore;

            _moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            if (charCon.isGrounded)
            {
                _moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }

            _canJump = Physics.OverlapSphere(checkGroundPoint.position, .25f, whatIsGround).Length > 0;

            if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                _moveInput.y = jumpForce;

                _canDoubleJump = true;

                AudioManager.instance.PlaySFX(8);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                _moveInput.y = jumpForce;

                _canDoubleJump = false;

                AudioManager.instance.PlaySFX(8);
            }

            if(_bounce)
            {
                _bounce = false;
                _moveInput.y = _bounceAmount;
                _canDoubleJump = true;
            }


            charCon.Move(_moveInput * Time.deltaTime);

            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            mouseInput.x = invertX ? -mouseInput.x : mouseInput.x;
            mouseInput.y = invertY ? -mouseInput.y : mouseInput.y;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles.x + -mouseInput.y, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            //Shooting
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTransform.position, hit.point) > 2)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }
                else
                {
                    firePoint.LookAt(camTransform.position + (camTransform.forward * 30f));
                }


                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
            }

            if (Input.GetMouseButton(1))
            {
                gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
            }
            else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }

            anim.SetFloat("Speed", _moveInput.magnitude);
            anim.SetBool("onGround", _canJump);
        }
    }

    public void FireShot()
    {
        if(activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;

        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.transform.position = activeGun.firePoint.transform.position;
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if(unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }
        }

        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

    public void Bounce(float bounceForce)
    {
        _bounceAmount = bounceForce;
        _bounce = true;
    }
}
