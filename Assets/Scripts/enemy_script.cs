using System.Collections.Generic;
using UnityEngine;

public class enemy_script : MonoBehaviour
{
    [Header("Shooting")]
    public float cooldown;
    public float projectileLife;
    public float projectileForce;
    public GameObject projectilePrefab;
    [Header("Health")]
    public int health;
    private List<GameObject> projectiles = new List<GameObject>();
    public Dictionary<GameObject, bool> projectilesFired = new Dictionary<GameObject, bool>();
    private float nextFireTime;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + cooldown;
        }
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
}
