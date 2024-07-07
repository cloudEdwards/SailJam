using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class health : MonoBehaviour
{
    public int currHealth=0;
    public int maxHealth=5;
    public GameObject boat;
    public string hearts;
    public GameObject gameOverUI;
    public Rect rect = new Rect(0, 0, 300, 100);
    public Vector3 offset = new Vector3(0f, 0f, 0.5f); // height above the target position
    public GUIStyle healthStyle=new GUIStyle();
    private ObstacleProximity obstacleProximity;
    public TextMeshProUGUI numCleared;
    public TextMeshProUGUI status;
    public Button startButton;
    private bool started=false;
 
    void OnGUI()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(boat.transform.position + offset);
        rect.x = point.x;
        rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
        hearts=string.Join("", Enumerable.Repeat("<3",currHealth));
        GUI.Label(rect, $"Health: {hearts}", healthStyle);
    }
    // Start is called before the first frame update
    void Start()
    {
    status.text="Welcome!";
    boat = GameObject.FindWithTag("Player");
    gameOverUI.SetActive(true);
    if(started==false)
    {

    startButton.onClick.AddListener(StartGame);
    }
    Time.timeScale=0;
    currHealth=maxHealth;
    healthStyle.fontSize=20;  
    obstacleProximity=GetComponent<ObstacleProximity>();
    }

    void StartGame()
    {
        Debug.Log("click");
        gameOverUI.SetActive(false);
        Time.timeScale=1;
        started=true;
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
    public void HealPlayer(int health)
    {
        if (currHealth<maxHealth)
        {
        currHealth+=health;
        }
    }
    void GameOver()
    {
        status.text="Game Over!";
        gameOverUI.SetActive(true);
        Time.timeScale=0; // pause game
        if(obstacleProximity!=null)
        {   int cleared=obstacleProximity.gauntletsCleared;
            numCleared.text=$"Narrows Cleared: {cleared}";
            Debug.Log($"Gauntlets Cleared: {obstacleProximity.gauntletsCleared}");
        }
    }
    public void Restart()
    {
        if(started==true){

        // Debug.Log("Restart");
        Time.timeScale=1; //resume normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload current scene
        Start();
        }
        else
        return;
    }
}
