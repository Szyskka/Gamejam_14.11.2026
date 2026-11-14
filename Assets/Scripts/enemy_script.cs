using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour
{
    [Header("Shooting")]
    public float cooldown;
    public float projectileLife;
    public float projectileForce;
    public float projectileRange;
    public GameObject projectilePrefab;
    [Header("Health")]
    public int health;
    [Header("Move")]
    public float moveSpeed;
    public float stopDistance;
    private List<GameObject> projectiles = new List<GameObject>();
    public Dictionary<GameObject, bool> projectilesFired = new Dictionary<GameObject, bool>();
    private float nextFireTime;
    private GameObject player;
    private Rigidbody2D rb;


    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextFireTime && Vector2.Distance(transform.position, player.GetComponent<Rigidbody2D>().position) <= projectileRange)
        {
            Shoot();
            nextFireTime = Time.time + cooldown;
        }
        Move();
    }
    void Shoot()
    {
        bool createP = true;
        if (projectilesFired.Count > 0)
        {
            foreach (GameObject p in projectiles)
            {
                if (projectilesFired[p] == true)
                {
                    createP = true;
                }
                else
                {
                    createP = false;
                    break;
                }
            }
        }
        if (createP == true)
        {
            GameObject p = Instantiate(projectilePrefab, gameObject.transform);
            projectiles.Add(p);
            projectilesFired.Add(p, true);
            p.GetComponent<projectile_script>().Shoot(projectileForce, projectileLife);
        }
        else
        {
            foreach (GameObject p in projectiles)
            {
                if (projectilesFired[p] == false)
                {
                    projectilesFired[p] = true;
                    p.GetComponent<projectile_script>().Shoot(projectileForce, projectileLife);
                    break;
                }
            }
        }
    }

    public void DecreaseHealth()
    {
        health -= 1;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        Vector2 direction = player.GetComponent<Rigidbody2D>().position - rb.position;
        float distance = direction.magnitude;
        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = direction.normalized * moveSpeed;
        }
    }
}
