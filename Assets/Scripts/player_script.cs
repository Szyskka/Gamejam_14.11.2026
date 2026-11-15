using UnityEngine;
using UnityEngine.InputSystem;

public class player_script : MonoBehaviour
{
    [Header("Movement")]
    public int moveSpeed;
    [Header("Health")]
    public int health;
    [Header("Animation")]
    public Animator animator;
    [Header("Sound")]
    public AudioSource ass;
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

    void FixedUpdate()
    {
        Vector2 moveDir = directions.normalized;
        rb.linearVelocity = moveDir * moveSpeed;
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
}
