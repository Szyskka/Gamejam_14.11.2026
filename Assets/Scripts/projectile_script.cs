using UnityEngine;

public class projectile_script : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Rigidbody2D rb;
    private float projectileDie;
    private bool shoot;
    public GameObject parentObj;
    private Collider2D col;
    private bool collidedWithShield = false;
    void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(GameObject creator)
    {
        parentObj = creator;
    }

    void FixedUpdate()
    {
        if (Time.time >= projectileDie && shoot == true)
        {
            SetProjectile();
        }
        if (parentObj == null)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.gameObject.tag)
        {
            case "Enemy":
                if (collidedWithShield == true)
                {
                    collision.gameObject.GetComponent<enemy_script>().DecreaseHealth();
                    SetProjectile();
                }
                break;
            case "Player":
                collision.gameObject.GetComponent<player_script>().health -= 1;
                SetProjectile();
                break;
            case "Shield":
                col.excludeLayers = LayerMask.GetMask("Projectile");
                collidedWithShield = true;
                break;
        }
    }
    public void Shoot(float pForce, float pLife)
    {
        gameObject.SetActive(true);
        shoot = true;
        Vector2 direction = (playerRB.position - rb.position).normalized;
        rb.AddForce(direction * pForce, ForceMode2D.Impulse);
        projectileDie = Time.time + pLife;
    }
    void SetProjectile()
    {
        col.excludeLayers = LayerMask.GetMask("Enemy", "Projectile");
        collidedWithShield = false;
        gameObject.SetActive(false);
        parentObj.GetComponent<enemy_script>().projectilesFired[gameObject] = false;
        gameObject.transform.position = parentObj.transform.Find("TurretPivot/Turret/ProjectilePivot").position;
    }
}
