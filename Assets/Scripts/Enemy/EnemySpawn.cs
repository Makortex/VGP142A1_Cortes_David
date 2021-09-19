using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] Enemies;
    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        int RNG = UnityEngine.Random.Range(0, Enemies.Length + 1);
        Debug.Log("RNG = " + RNG);
        if (RNG != 0)
        {
            Instantiate(Enemies[UnityEngine.Random.Range(0, Enemies.Length)], spawn.position, spawn.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
