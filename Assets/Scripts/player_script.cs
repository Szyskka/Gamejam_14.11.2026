using UnityEngine;
using UnityEngine.InputSystem;

public class player_script : MonoBehaviour
{
    [Header("Movement")]
    public int moveSpeed;
    [Header("Health")]
    public int health;
    private Vector2 directions;
    private Rigidbody2D rb;
    InputAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        directions = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position+directions*moveSpeed*Time.fixedDeltaTime);
    }
}
