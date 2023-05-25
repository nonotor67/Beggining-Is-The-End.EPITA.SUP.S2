using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerObjects : MonoBehaviour
{
    [SerializeField] private float pickupRange = 5;
    private Player player;
    private PlayerMovement playerMov;
    private PlayerUI playerUI;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip pickupSound;

    public float fireDelta = 0.5F;
    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private GameObject pickableObject;
    public bool beingCarried = false;

    private GameObject Button;

    Vector3 _rotate = new Vector3(0,1,0);

    private void Start()
    {
        player = GetComponent<Player>();
        playerMov = GetComponent<PlayerMovement>();
        playerUI = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        RaycastHit hit;

        myTime += Time.deltaTime;
        
        if (beingCarried)
        {
            playerUI.HideLeftMouse();
            playerUI.ShowRightMouse();

            if (Input.GetMouseButton(1))
            {
                pickableObject.GetComponent<Rigidbody>().isKinematic = false;
                pickableObject.transform.parent = null;
                SceneManager.MoveGameObjectToScene(pickableObject, SceneManager.GetActiveScene());
                beingCarried = false;
            }
        }
        else
        {
            playerUI.HideRightMouse();
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit,
                pickupRange))
        {
            if (hit.collider.gameObject.CompareTag("HealthRegen"))
            {
                playerUI.ShowUse();
                playerUI.HideLeftMouse();

                if (Input.GetButton("Use"))
                {
                    player.health += 5;
                    Destroy(hit.collider.gameObject);
                    _audioSource.PlayOneShot(pickupSound);
                }
            }

            else if (hit.collider.gameObject.CompareTag("HealthBoost"))
            {
                playerUI.ShowUse();
                playerUI.HideLeftMouse();

                if (Input.GetButton("Use"))
                {
                    player.maxEnergy += 10;
                    player.health += 10;
                    Destroy(hit.collider.gameObject);
                    _audioSource.PlayOneShot(pickupSound);
                }
            }

            else if (hit.collider.gameObject.CompareTag("SpeedBoost"))
            {
                playerUI.ShowUse();
                playerUI.HideLeftMouse();

                if (Input.GetButton("Use"))
                {
                    playerMov.speed += 2;
                    playerMov.sprintSpeed += 2;
                    Destroy(hit.collider.gameObject);
                    _audioSource.PlayOneShot(pickupSound);
                }
            }

            else if (hit.collider.gameObject.CompareTag("JumpBoost"))
            {
                playerUI.ShowUse();
                playerUI.HideLeftMouse();

                if (Input.GetButton("Use"))
                {
                    playerMov.jumpHeight += 2;
                    Destroy(hit.collider.gameObject);
                    _audioSource.PlayOneShot(pickupSound);
                }
            }

            else if (hit.collider.gameObject.CompareTag("Door") && myTime > nextFire)
            {
                playerUI.ShowUse();
                playerUI.HideLeftMouse();

                if (Input.GetButton("Use"))
                {
                    nextFire = myTime + fireDelta;
                    hit.collider.gameObject.GetComponent<DoorController>().ToggleDoor();
                    nextFire = nextFire - myTime;
                    myTime = 0.0F;
                }
            }

            else if (hit.collider.gameObject.CompareTag("pickableObject"))
            {
                playerUI.HideUse();
                playerUI.ShowLeftMouse();
                pickableObject = hit.collider.gameObject;

                if (Input.GetMouseButton(0))
                {
                    pickableObject.GetComponent<Rigidbody>().isKinematic = true;
                    pickableObject.transform.parent = this.gameObject.transform;
                    beingCarried = true;
                }
                
                if(Input.GetKey ("f"))
                {
                    pickableObject.transform.Rotate(_rotate);
                }
            }
            else
            {
                playerUI.HideUse();
                playerUI.HideRightMouse();
                playerUI.HideLeftMouse();
            }
        }
        else
        {
            playerUI.HideUse();
            playerUI.HideRightMouse();
            playerUI.HideLeftMouse();
        }
    }
}