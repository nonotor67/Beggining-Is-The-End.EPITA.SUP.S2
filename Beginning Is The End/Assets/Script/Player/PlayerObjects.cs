using System;
using System.Collections;
using UnityEngine;

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
    private bool beingCarried = false;

    private void Start()
    {
        player = GetComponent<Player>();
        playerMov = GetComponent<PlayerMovement>();
        playerUI = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        RaycastHit hit;

        myTime = myTime + Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out hit,
                pickupRange))
        {
            if (hit.collider.gameObject.CompareTag("HealthRegen"))
            {
                playerUI.ShowUse();
                
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
                pickableObject = hit.collider.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    pickableObject.GetComponent<Rigidbody>().isKinematic = true;
                    pickableObject.transform.parent = this.gameObject.transform;
                    beingCarried = true;
                }
            }
            else
            {
                playerUI.HideUse();
            }
        }
        else
        {
            playerUI.HideUse();
        }

        if (beingCarried)
        {
            if (Input.GetMouseButtonDown(1))
            {
                pickableObject.GetComponent<Rigidbody>().isKinematic = false;
                pickableObject.transform.parent = null;
                beingCarried = false;
            }
        }
    }
}