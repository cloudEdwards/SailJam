using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WindZoneBehavior : MonoBehaviour
{

    public float windDirection;
    

    public float randomMinTime;
    public float randomMaxTime;


    // Start is called before the first frame update
    void Start()
    { 
        ScheduleNextCall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandonWindDirection()
    {
        Debug.Log("Random Wind Called At: " + Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, windDirectionRandomizer(transform.rotation.z)));
        ScheduleNextCall() ;
    }

    float windDirectionRandomizer(float currentValue)
    {
        float newValue = Random.Range(0f, 359f);

        while (System.Math.Abs(newValue - currentValue) < 50)
        {
            newValue = Random.Range(0f, 359f);
        }
        Debug.Log("New Wind Vale: " + newValue);
        return newValue;
    }

    void ScheduleNextCall()
    {

        // ParticleSystem particlesWind = Instantiate(particleEffect, new Vector3(this.transform.position.x, this.transform.position.y, -10), Quaternion.Euler(new Vector3(0,0,windDirection)));
        // particlesWind.transform.SetParent(this.transform);
        // var main =particlesWind.main;
        // main.simulationSpeed=windSpeed;
        // particlesWind.Play();
        // instantiatedObjects.Add(particlesWind);
        // // Calculate a random interval between 10 and 20 seconds
        // float waitTime = Random.Range(randomMinTime, randomMaxTime);

        // // Schedule the next call
        // Invoke(nameof(RandonWindDirection), waitTime);
    }
    
}
