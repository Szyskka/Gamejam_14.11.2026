using UnityEngine;

public class turret_script : MonoBehaviour
{
    private Rigidbody2D playerRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = playerRB.position - (Vector2)transform.position;
        transform.up = direction; 
    }
}
