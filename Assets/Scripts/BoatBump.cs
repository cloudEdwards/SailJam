using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBump : MonoBehaviour
{
    public List<AudioSource> bumpSounds;
    public List<GameObject> onomatopoeiaList;
    public GameObject hitParticles;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) {
            AudioSource bump = bumpSounds[Random.Range(0, bumpSounds.Count)];
            bump.Play();
    
            // Get the point of contact
            Vector2 contactPoint = other.contacts[0].point;
            
            // Instantiate the hit particles at the contact point with no rotation
            Instantiate(hitParticles, contactPoint, Quaternion.identity);
            Instantiate(onomatopoeiaList[Random.Range(0, onomatopoeiaList.Count)], contactPoint, Quaternion.identity);
        }
    }
}
