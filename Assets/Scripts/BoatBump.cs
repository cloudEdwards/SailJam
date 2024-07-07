using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBump : MonoBehaviour
{
    public AudioSource bump;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) {
            bump.Play();
        }
    }
}
