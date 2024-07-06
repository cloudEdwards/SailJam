using UnityEngine;

public class SailBoatMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed of movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    
    public float rotationSpeed = 100f;

    Vector2 movement; // Vector to store the movement direction

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 forceDirection = rb.transform.up;  // The direction of the force
        

    
        rb.AddForce(forceDirection.normalized * moveSpeed, ForceMode2D.Impulse);
        
        // Calculate the amount of rotation
        float rotationAmount = movement.y * rotationSpeed * Time.deltaTime;
        Debug.Log($"{rotationAmount}");

        // Apply the rotation to the Rigidbody2D
        rb.MoveRotation(rb.rotation + rotationAmount);

    }
}