using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{

    [SerializeField]
    protected float turnSpeed = 1f;
    public float pullRadius = 10f;
    public float pullForce = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed, Space.World);
    }

    void FixedUpdate()
    {
        // Find all colliders within the pull radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pullRadius);
        
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has a Rigidbody2D (i.e., it's a physical object)
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate the direction from the player to the gravity well
                Vector2 direction = (Vector2)transform.position - rb.position;
                float distance = direction.magnitude;

                // Normalize the direction to get the direction vector and apply force
                Vector2 pullDirection = direction.normalized;

                // Apply force proportional to the distance from the gravity well
                rb.AddForce(pullDirection * pullForce / Mathf.Max(distance, 1f));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the pull radius in the scene view for easier debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
