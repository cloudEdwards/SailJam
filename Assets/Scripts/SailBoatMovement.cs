using UnityEngine;

public class SailBoatMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed of movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    
    public float rotationSpeed = 100f;
    public float degrees=0;
    public float speedMultiplier;
    public AreaEffector2D waterWind;

    Vector2 movement; // Vector to store the movement direction

    void Start()
    {
        waterWind=GameObject.FindGameObjectWithTag("Force").GetComponent<AreaEffector2D>();
    }

    void Update()
    {
        if(degrees==360|degrees==-360){
            degrees=0;
        }
        speedMultiplier = degrees switch
        {
            var d when (d > -30 && d < 30)||(d>330)||(d<-330) => 0.1f,
            var d when (d >= 30 && d < 60) || (d > -60 && d <= -30)||(d>=270)||(d<=-270) => 0.75f,
            var d when (d >= 60 && d < 120) || (d > -120 && d <= -60)|| (d>=210)||(d<=-210)=> 1.5f,
            var d when (d >= 120 && d < 150) || (d > -150 && d <= -120) ||(d>=150)||(d<=-150)=> 1f,
            _ => 0.1f
        };
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 forceDirection = rb.transform.up;  // The direction of the force
        

    
        rb.AddForce(forceDirection.normalized * (moveSpeed*speedMultiplier), ForceMode2D.Impulse);
        
        // Calculate the amount of rotation
        float rotationAmount = movement.y * rotationSpeed * Time.deltaTime;
        degrees+=rotationAmount;
        Debug.Log($"{degrees}, {speedMultiplier}");
        

        // Apply the rotation to the Rigidbody2D
        rb.MoveRotation(rb.rotation + rotationAmount);

    }
}