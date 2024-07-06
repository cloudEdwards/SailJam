using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public int currHealth=0;
    public int maxHealth=3;
    public GameObject boat;
   Rect rect = new Rect(0, 0, 300, 100);
   Vector3 offset = new Vector3(0f, 0f, 0.5f); // height above the target position
 
    void OnGUI()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(boat.transform.position + offset);
        rect.x = point.x;
        rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
        GUI.Label(rect, $"Health: {currHealth}"); // display its name, or other string
    }
    // Start is called before the first frame update
    void Start()
    {
   boat = GameObject.FindWithTag("Player");
   currHealth=maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            DamagePlayer(1);
        }
    }
    public void DamagePlayer(int damage){
        currHealth-=damage;
    }
}
