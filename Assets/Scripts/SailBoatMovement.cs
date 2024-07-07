using UnityEngine;

public class SailBoatMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed of movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    
    public float rotationSpeed = 100f;
    public float speedMultiplier; // based on degrees (points of sail)
    public AreaEffector2D waterWind; // base scene area effector; speed of 1

    public ParticleSystem[] wakeEffect;
    private Vector2 windDirection;
    private float relativeWindAngle;

    Vector2 movement; // Vector to store the movement direction

    void Start()
    {

    }

    void Update()
    {
        UpdateWindDirection();
        CalculateRelativeWindAngle();


        var degreesCompare = relativeWindAngle;

        speedMultiplier = degreesCompare switch
        {
            var d when (d > -30 && d < 30)=> 2f,
            var d when (d >= 30 && d < 90) || (d > -90 && d <= -30)=> 2.5f,
            var d when (d >= 90 && d < 150) || (d > -150 && d <= -90)=> 1.5f,
            var d when (d>=150)||(d<=-150)=> 0.3f,
            _ => 0.1f
        };
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        foreach(var effect in wakeEffect){

        if (effect != null)
            {
                var main = effect.main;
                main.startSize = new ParticleSystem.MinMaxCurve(Mathf.Clamp(2*speedMultiplier * 1f, 0.1f, 1f));
                // Optionally, adjust other properties
                var emission = effect.emission;
                emission.rateOverTime = speedMultiplier * 20f; // Adjust multiplier as needed

                main.startLifetime = new ParticleSystem.MinMaxCurve(Mathf.Clamp(speedMultiplier * 0.5f, 0.5f, 2f));
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 forceDirection = rb.transform.up;  // The direction of the force
    
        rb.AddForce(forceDirection.normalized * (moveSpeed*speedMultiplier), ForceMode2D.Impulse);
        
        // Calculate the amount of rotation
        float movementAmount = Mathf.Abs(movement.x) > .1f ? movement.x * -1 : 0f;
        movementAmount = Mathf.Abs(movement.y) > .1f ? movement.y : movementAmount;
        
        float rotationAmount = movementAmount * rotationSpeed * Time.deltaTime;
        
        // Apply the rotation to the Rigidbody2D
        rb.MoveRotation(rb.rotation + rotationAmount);

    }
    void CalculateRelativeWindAngle()
    {
        Vector2 boatDirection=rb.transform.up;
        relativeWindAngle=Vector2.SignedAngle(boatDirection,windDirection);
    }
    void UpdateWindDirection()
    {
        if(waterWind!=null)
        {
            float windAngle = waterWind.transform.eulerAngles.z * Mathf.Deg2Rad;
            windDirection = new Vector2(Mathf.Cos(windAngle), Mathf.Sin(windAngle));
        }
    }
}