using UnityEngine;

public class SailBoatMovement: MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component

    Vector2 movement; // Vector to store the movement direction

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Movement logic using Rigidbody2D forces
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
