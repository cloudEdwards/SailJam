using UnityEngine;

public class SailBoatMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed of movement
    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    
    public float rotationSpeed = 100f;
    public float degrees=0; // track degrees to compare boat angle to wind
    public float speedMultiplier; // based on degrees (points of sail)
    public AreaEffector2D waterWind; // base scene area effector; speed of 1

    public ParticleSystem[] wakeEffect;
    private Vector2 windDirection;
    private float boatAngle;
    private float relativeWindAngle;

    Vector2 movement; // Vector to store the movement direction

    void Start()
    {
        waterWind=GameObject.FindGameObjectWithTag("Force").GetComponent<AreaEffector2D>();
    }

    void Update()
    {
        UpdateWindDirection();
        if(degrees==360|degrees==-360){
            degrees=0;
        }

        var waterWindDirection = waterWind.forceAngle;
        var degreesCompare = degrees - waterWindDirection;

        // Debug.Log(degreesCompare);

        speedMultiplier = degreesCompare switch
        {
            var d when (d > -30 && d < 30)||(d>330)||(d<-330) => 0.3f,
            var d when (d >= 30 && d < 60) || (d > -60 && d <= -30)||(d>=270)||(d<=-270) => 1f,
            var d when (d >= 60 && d < 120) || (d > -120 && d <= -60)|| (d>=210)||(d<=-210)=> 2f,
            var d when (d >= 120 && d < 150) || (d > -150 && d <= -120) ||(d>=150)||(d<=-150)=> 1.5f,
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
        float rotationAmount = movement.y * rotationSpeed * Time.deltaTime;
        degrees+=rotationAmount;
        // Debug.Log($"{degrees}, {speedMultiplier}");
        
        // Apply the rotation to the Rigidbody2D
        rb.MoveRotation(rb.rotation + rotationAmount);

    }
    void CalculateRelativeWindAngle()
    {
        Vector2 boatDirection=rb.transform.up;
        relativeWindAngle=Vector2.SignedAngle(boatDirection,windDirection);
        Debug.Log(relativeWindAngle);
    }
    void UpdateWindDirection()
    {
        if(waterWind!=null)
        {
            float windAngle = waterWind.transform.eulerAngles.z * Mathf.Deg2Rad;
            windDirection = new Vector2(Mathf.Cos(windAngle), Mathf.Sin(windAngle));
            // Debug.Log(windDirection);
        }
    }
}