using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBump : MonoBehaviour
{
    public List<AudioSource> bumpSounds;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) {
            AudioSource bump = bumpSounds[Random.Range(0, bumpSounds.Count)];
            bump.Play();
        }
    }
}
