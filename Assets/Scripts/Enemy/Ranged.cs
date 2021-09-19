using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public bool agro;
    Animator animator;

    public Rigidbody powerBall;
    public Transform projectieSpawnPoint;
    public float projectileForce;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agro == true)
        {
            animator.SetTrigger("Shoot");
        }
    }
    public void Burst()
    {
        Rigidbody temp = Instantiate(powerBall, projectieSpawnPoint.position, projectieSpawnPoint.rotation);
        temp.AddForce(projectieSpawnPoint.forward * projectileForce, ForceMode.Impulse);

        Destroy(temp.gameObject, 2.0f);
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
