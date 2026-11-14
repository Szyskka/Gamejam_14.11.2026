using UnityEngine;

public class projectile_script : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Rigidbody2D rb;
    private float projectileDie;
    private bool shoot;
    private GameObject parentObj;
    private bool collidedWithShield = false;
    void Awake()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        parentObj = transform.parent.gameObject;
    }

    void FixedUpdate()
    {
        if (Time.time >= projectileDie && shoot == true)
        {
            SetProjectile();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.gameObject.tag)
        {
            case "Enemy":
                if (collidedWithShield == true)
                {
                    collision.gameObject.GetComponent<enemy_script>().health -= 1;
                    SetProjectile();
                }
                break;
            case "Player":
                collision.gameObject.GetComponent<player_script>().health -= 1;
                SetProjectile();
                break;
            case "Shield":
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
        collidedWithShield = false;
        gameObject.SetActive(false);
        parentObj.GetComponent<enemy_script>().projectilesFired[gameObject] = false;
        gameObject.transform.position = parentObj.transform.position;
    }
}
