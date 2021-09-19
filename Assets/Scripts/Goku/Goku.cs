using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TMPro;

public class Goku : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] TMP_Text healthText;
    public GameObject drop;

    public float health = 100;
    public GameObject kameHameHa;
    public Rigidbody powerBall;
    public Transform projectieSpawnPoint;
    public float projectileForce;
    public float speed;
    public Transform target;
    public bool fight;
    bool coroutineRunning = false;
    public float time;

    public bool range;
    public bool melee;

    public bool[] attackM = new bool[] { false, false }; //melee attacks [0]punch [1]kick
    public bool[] attackR = new bool[] { false, false }; //range attacks [0]kame [1]blasts

    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(target);

        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        if (fight == true)
        {
            animator.SetBool("Fight", true);
            agent.SetDestination(target.transform.position);
            if (health <= 0)
            {
                animator.SetTrigger("Die");
            }
            if (range == true)
            {
                time += Time.deltaTime;
                if (time >= 11)
                {
                    time = 0;
                    if (!coroutineRunning)
                    {
                        StartCoroutine("RangeCR");
                    }
                    else
                    {
                        StopCoroutine("RangeCR");
                        StartCoroutine("RangeCR");
                    }
                }
            }
            if (melee == true)
            {
                time += Time.deltaTime;
                if (time >= 2)
                {
                    time = 0;
                    if (!coroutineRunning)
                    {
                        StartCoroutine("MeleeCR");
                    }
                    else
                    {
                        StopCoroutine("MeleeCR");
                        StartCoroutine("MeleeCR");
                    }
                }                
            }
        }
        if (attackM[0] == true)
        {
            animator.SetTrigger("Punch");
        }
        if (attackM[1] == true)
        {
            animator.SetTrigger("Kick");
        }
        if (attackR[0] == true)
        {
            animator.SetTrigger("KameHameHa");
        }
        if (attackR[1] == true)
        {
            animator.SetTrigger("Burst");
        }
        animator.SetFloat("Speed", transform.InverseTransformDirection(agent.velocity).z);
        healthText.text = health.ToString();

    }
    public void Kame()
    {
        GameObject temp = Instantiate(kameHameHa, projectieSpawnPoint.position, projectieSpawnPoint.rotation);
        Destroy(temp.gameObject, 2.0f);

    }
    public void Burst()
    {
        Rigidbody temp = Instantiate(powerBall, projectieSpawnPoint.position, projectieSpawnPoint.rotation);
        temp.AddForce(projectieSpawnPoint.forward * projectileForce, ForceMode.Impulse);

        Destroy(temp.gameObject, 2.0f);

    }
    public void Stop()
    {
        agent.speed = 0f;
    }
    public void Normalize()
    {
        agent.speed = 3.5f;
        //agent.angularSpeed = 120f;
    }
    public void Die()
    {
        Instantiate(drop);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hammer")
        {
            health -= 10;
            animator.SetTrigger("Hit");
        }
        if (collision.gameObject.tag == "PlayerPunch")
        {
            health -= 5;
            animator.SetTrigger("Hit");

        }
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= 2;
            animator.SetTrigger("Hit");

        }
    }
    IEnumerator MeleeCR()
    {
        int RNG = UnityEngine.Random.Range(0, 2);
        Debug.Log(RNG);
        attackM[RNG] = true;
        yield return new WaitForSeconds(0.1f);
        attackM[RNG] = false;

    }
    IEnumerator RangeCR()
    {
        agent.speed = 0f;
        int RNG = UnityEngine.Random.Range(0, 2);
        Debug.Log(RNG);
        attackR[RNG] = true;
        yield return new WaitForSeconds(0.01f);
        attackR[RNG] = false;
        //agent.speed = 3.5f;
    }
}
