using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private health healthComponent;
    // 2d collision method; look for obstacle tags
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Obstacle"))
            if (healthComponent!= null){
                healthComponent.DamagePlayer(1);
            }
    }
    // Start is called before the first frame update
    void Start()
    {
     healthComponent=GetComponent<health>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
