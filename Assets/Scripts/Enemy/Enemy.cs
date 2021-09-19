using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    Animator animator;
    [SerializeField] TMP_Text healthText;
    public GameObject[] drops;
    public GameObject target;
    public Transform PUPSpawn;


    public int health = 10;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        if (target == null)
        {
            target = GameObject.FindWithTag("Player"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        healthText.text = health.ToString();
        if (health <= 0)
        {
            animator.SetTrigger("Die");
        }
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

    void DropPUp()
    {
        int RNG = UnityEngine.Random.Range(0, drops.Length + 1);
        Debug.Log("RNG = " + RNG);
        if (RNG != 0)
        {
            Instantiate(drops[UnityEngine.Random.Range(0, drops.Length)], PUPSpawn.position, PUPSpawn.rotation);
        }
        Destroy(gameObject);
    }
}
