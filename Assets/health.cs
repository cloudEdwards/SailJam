using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class health : MonoBehaviour
{
    public int currHealth=0;
    public int maxHealth=3;
    public GameObject boat;
    public string hearts;
    public GameObject gameOverUI;
   Rect rect = new Rect(0, 0, 300, 100);
   Vector3 offset = new Vector3(0f, 0f, 0.5f); // height above the target position
 
    void OnGUI()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(boat.transform.position + offset);
        rect.x = point.x;
        rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
        hearts=string.Join("", Enumerable.Repeat("♥",currHealth));
        GUI.Label(rect, $"Health: {hearts}");
    }
    // Start is called before the first frame update
    void Start()
    {
   boat = GameObject.FindWithTag("Player");
   gameOverUI.SetActive(false);
   currHealth=maxHealth;        
    }

    // Update is called once per frame
    void Update()
    {
        if(currHealth<=0){
            // set GUI.Label to 'sinking' with countdown
            // game over screen
            GameOver();
        }
    }
    public void DamagePlayer(int damage){
        currHealth-=damage;
        // healthMeter.insert("♡");
        // healthMeter.pop();
    }
    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale=0; // pause game
    }
    public void Restart()
    {
        Time.timeScale=1; //resume normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload current scene
        Start();
    }
}
