using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(Animator))]
public class Character : MonoBehaviour
{
    AudioSource pickupAudioSource;
    public AudioClip pickupSFX;

    public int damage = 1;
    public float damageTime = 2.0f;
    float timeSinceLastDamage;

    public GameObject goku;
    //public GameObject melee;
    private Goku gok;
    private Melee mel;

    CharacterController controller;
    Animator animator;

    [Header("PlayerSettings")]
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    public float gravity;
    public GameObject backHammer;
    public GameObject handHammer;
    public GameObject hammerHitbox;
    public bool hammer;

    public bool hasKey;
    bool coroutineRunning = false;
    Vector3 moveDirection;

    enum ControllerType { SimpleMove, Move };
    [SerializeField] ControllerType type;

    [Header("ProjectileSettings")]
    public float projectileForce;
    public Rigidbody projectilePrefab;
    public Transform projectieSpawnPoint;

    public Transform thingToLookFrom;
    public float lookAtDistance;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Cursor.lockState = CursorLockMode.Locked;
            //pickupAudioClip = GetComponent<AudioSource>();

            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            gok = goku.GetComponent<Goku>();
            //mel = melee.GetComponent<Melee>();
            controller.minMoveDistance = 0.0f;

            animator.applyRootMotion = false;

            if (speed <= 0)
            {
                //speed = 6.0f;
                //Debug.LogWarning(name + ": Speed not set. Defaulting to" + speed);
            }
            if (jumpSpeed <= 0)
            {
                jumpSpeed = 6.0f;
                //Debug.LogWarning(name + ": jumpSpeed not set. Defaulting to" + jumpSpeed);
            }
            if (rotationSpeed <= 0)
            {
                rotationSpeed = 10.0f;
                //Debug.LogWarning(name + ": rotationSpeed not set. Defaulting to" + rotationSpeed);
            }
            if (gravity <= 0)
            {
                gravity = 9.81f;
                //Debug.LogWarning(name + ": gravity not set. Defaulting to" + gravity);
            }
            if (projectileForce <= 0)
            {
                projectileForce = 10.0f;
            }
            if (!projectilePrefab)
            {
                Debug.LogWarning(name + ":Mising projectilePrefab");
            }
            if (lookAtDistance <= 0)
            {
                //Debug.LogWarning(name + ":Mising projectilePrefab");
                lookAtDistance = 10.0f;
            }

            moveDirection = Vector3.zero;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            Debug.LogWarning("Always get called");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ControllerType.SimpleMove:

                //transform.Rotate(0, Input.GetAxis("Horizontal")*rotationSpeed, 0);
                controller.SimpleMove(transform.forward * Input.GetAxis("Vertical") * speed);

                break;

            case ControllerType.Move:

                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    moveDirection *= speed;
                    moveDirection = transform.TransformDirection(moveDirection);

                    if (Input.GetButtonDown("Jump"))
                    {
                        moveDirection.y = jumpSpeed;
                    }
                }
                moveDirection.y -= gravity * Time.deltaTime;

                controller.Move(moveDirection * Time.deltaTime);

                break;
        }
        //usage raycast
        //~gameobject needs a collider
        RaycastHit hit;

        if (!thingToLookFrom)
        {
            Debug.DrawRay(transform.position, transform.forward * lookAtDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, lookAtDistance))
            {
                //Debug.Log("Raycast hit: " + hit.transform.name);
            }
        }
        else
        {
            Debug.DrawRay(thingToLookFrom.transform.position, thingToLookFrom.transform.forward * lookAtDistance, Color.blue);

            if (Physics.Raycast(thingToLookFrom.transform.position, thingToLookFrom.transform.forward, out hit, lookAtDistance))
            {
                //Debug.Log("Raycast hit: " + hit.transform.name);
            }
        }

        if (Input.GetButtonDown("Fire2") && hammer==false)
        {
            animator.SetTrigger("Fire");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    animator.SetTrigger("Kick");
        //}
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("Dead", true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("Dead", false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && hammer == false) //take out hammer
        {
            hammer = true;

            animator.SetTrigger("CallHammer");
            animator.SetBool("Hammer", true);
        }
        if (Input.GetKeyDown(KeyCode.E) && hammer == true) //store hammer
        {
            hammer = false;

            animator.SetTrigger("StoreHammer");
            animator.SetBool("Hammer", false);

        }
        animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetFloat("Speed", transform.InverseTransformDirection(controller.velocity).z);
    }
 
    void OnControllerColliderHit(ControllerColliderHit hit)//same as stay
    {
        if (hit.gameObject.tag == "Enemy")
        {
            if (Time.time > timeSinceLastDamage + damageTime)
            {
                //GameManager.instance.health--;
                Damaged();
                //moveDirection.y = jumpSpeed;
                //moveDirection.z
            }

        }
        if (hit.gameObject.tag == "Acid")
        {
            Damaged();
            speed = 3.0f;
        }
        if (hit.gameObject.tag == "Water")
        {
            animator.SetBool("Dead", true);
            GameManager.instance.health = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        { 
            Damaged(); 
        }
        if (other.gameObject.tag == "Key")
        {
            if (!pickupAudioSource)
            {
                pickupAudioSource = gameObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = pickupSFX;
                pickupAudioSource.loop = false;
            }
            pickupAudioSource.Play();

            hasKey = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Medkit")
        {
            if (!pickupAudioSource)
            {
                pickupAudioSource = gameObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = pickupSFX;
                pickupAudioSource.loop = false;
            }
            pickupAudioSource.Play();

            GameManager.instance.health = GameManager.instance.maxHealth;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Coin")
        {
            if (!pickupAudioSource)
            {
                pickupAudioSource = gameObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = pickupSFX;
                pickupAudioSource.loop = false;
            }
            pickupAudioSource.Play();

            GameManager.instance.score++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Pumpkin")
        {
            if (!pickupAudioSource)
            {
                pickupAudioSource = gameObject.AddComponent<AudioSource>();
                pickupAudioSource.clip = pickupSFX;
                pickupAudioSource.loop = false;
            }
            pickupAudioSource.Play();

            GameManager.instance.maxHealth++;
            GameManager.instance.health++;
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Range")
        {
            gok.range = true;
            gok.fight = true;
        }
        if (other.gameObject.tag == "Melee")
        {
            gok.melee = true;
        }
        if (other.gameObject.tag == "Portal")
        {
            if(hasKey==true)
            {
                GameManager.instance.Win();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Range")
        {
            gok.range = false;
            gok.fight = false;
        }
        if (other.gameObject.tag == "Melee")
        {
            gok.melee = false;
        }
        //if (other.gameObject.tag == "Agro")
        //{
        //    mel.melee = false;
        //}
    }
    public void fire()
    {
        Debug.Log("pew pew");

        if (projectieSpawnPoint && projectilePrefab)
        {
            Rigidbody temp = Instantiate(projectilePrefab, projectieSpawnPoint.position, projectieSpawnPoint.rotation);

            temp.AddForce(projectieSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
        }
    }
    public void grabHammer()
    {
        handHammer.SetActive(true);
        backHammer.SetActive(false);
        //hammer = true;

    }
    public void storeHammer()
    {
        handHammer.SetActive(false);
        backHammer.SetActive(true);
        //hammer = false;

    }
    public void activeHitbox()
    {
        hammerHitbox.SetActive(true);

    }
    public void desactiveHitbox()
    {
        hammerHitbox.SetActive(false);
    }
    public void Damaged()
    {
        if (!coroutineRunning)
        {
            StartCoroutine("Damage");
        }
        else
        {
            StopCoroutine("Damage");
            StartCoroutine("Damage");
        }
    }
    
    IEnumerator Damage()
    {
        coroutineRunning = true;

        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Hurt");
        GameManager.instance.health--;
        coroutineRunning = false;
    }
}