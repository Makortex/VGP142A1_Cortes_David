using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;

    Animator animator;
    public bool agro;
    public bool melee;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);

        if (agro == true)
        {
            agent.speed = 3.5f;
            animator.SetTrigger("Melee");

        }
        if (agro == false)
        {
            agent.speed = 0f;
        }
        animator.SetFloat("Speed", transform.InverseTransformDirection(agent.velocity).z);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            agro = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            agro = false;
        }
    }
}
