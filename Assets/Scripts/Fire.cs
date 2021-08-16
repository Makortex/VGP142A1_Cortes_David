using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("ProjectileSettings")]
    public float projectileForce;
    public Rigidbody projectilePrefab;
    public Transform projectieSpawnPoint;

    private void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    fire();
        //}
    }
    public void fire()
    {
        //Debug.Log("pew pew");

        if (projectieSpawnPoint && projectilePrefab)
        {
            Rigidbody temp = Instantiate(projectilePrefab, projectieSpawnPoint.position, projectieSpawnPoint.rotation);

            temp.AddForce(projectieSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider.gameObject);
        //Destroy(gameObject);
    }
}
