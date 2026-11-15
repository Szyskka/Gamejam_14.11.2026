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
    public float turnSpeed;
    public float stopDistance;
    [Header("Animation")]
    public Animator animator;
    [Header("Sound")]
    public AudioSource ass;
    private List<GameObject> projectiles = new List<GameObject>();
    public Dictionary<GameObject, bool> projectilesFired = new Dictionary<GameObject, bool>();
    private float nextFireTime;
    private GameObject player;
    private Rigidbody2D rb;
    private GameObject wave;


    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        wave = GameObject.Find("WaveManager");
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
            GameObject p = Instantiate(projectilePrefab,transform.Find("TurretPivot/Turret/ProjectilePivot").position,Quaternion.identity);
            projectiles.Add(p);
            projectilesFired.Add(p, true);
            p.GetComponent<projectile_script>().parentObj = gameObject;
            sound_script.Instance.PlayShootSound();
            p.GetComponent<projectile_script>().Shoot(projectileForce, projectileLife);
        }
        else
        {
            foreach (GameObject p in projectiles)
            {
                if (projectilesFired[p] == false)
                {
                    projectilesFired[p] = true;
                    sound_script.Instance.PlayShootSound();
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
            if (ass.isPlaying)
            {
                ass.Stop();
            }
            animator.SetBool("dead", true);
        }
    }
    public void DestroySelf()
    {
        sound_script.Instance.PlayOnEnemyDeath();
        Destroy(gameObject);
    }
    public void PlayerIsDead()
    {
        if (ass.isPlaying)
        {
            ass.Stop();
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
        if (rb.linearVelocity.x!=0 || rb.linearVelocityY!=0)
        {
            animator.SetBool("driving", true);
            if (!ass.isPlaying)
            {
                ass.Play();
            }
        }
        else
        {
            animator.SetBool("driving", false);
            if (ass.isPlaying)
            {
                ass.Stop();
            }
        }
    }
    void Update()
    {
        Vector2 moveDirection = rb.linearVelocity;

        // Only rotate when the object is actually moving
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            // Smoothly rotate toward the target direction
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
        }
    }
}
