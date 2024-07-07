using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindZoneBehavior : MonoBehaviour
{

    public float windDirection;
    public ParticleSystem particleEffect;
    public GameObject sailShip;
    public AreaEffector2D areaEffector2D;
    public float windSpeed;
    // List to keep track of instantiated objects
    private List<ParticleSystem> instantiatedObjects = new List<ParticleSystem>();


    // Start is called before the first frame update
    void Start()
    { 
        areaEffector2D = GetComponent<AreaEffector2D>();
        areaEffector2D.forceAngle = windDirection;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerParticleEffect();
            ChangeEfectorZone();

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the object that exited is the one you are interested in
        if (other.CompareTag("Player"))
        {
            DestroyAllInstantiatedObjects();
        }
    }

    void TriggerParticleEffect()
    {
        ParticleSystem particlesWind = Instantiate(particleEffect, new Vector3(this.transform.position.x, this.transform.position.y, -10), Quaternion.Euler(new Vector3(0,0,windDirection)));
        particlesWind.playbackSpeed = windSpeed;
        particlesWind.Play();
        instantiatedObjects.Add(particlesWind);

    }
    
    void ChangeEfectorZone()
    {
        Debug.Log("ChangeEfectorZone");
        areaEffector2D.forceAngle = windDirection;
    }

    void DestroyAllInstantiatedObjects()
    {
        foreach (ParticleSystem obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
    }
}
