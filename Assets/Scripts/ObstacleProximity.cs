using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleProximity : MonoBehaviour
{
    public float detectRadius=4f;
    public int maxObstaclesToTrack = 5; // Maximum number of closest obstacles to track
    public bool crashed = true;
    public bool hazard=true;
    public enum GameState
    {
      Normal,
      Gauntlet
    }
    private GameState currentState=GameState.Normal;


    private List<ObstacleInfo> nearObstacles=new List<ObstacleInfo>();
    // Start is called before the first frame update
     private float updateInterval = 0.5f; // Update every half second
    private float lastUpdateTime = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
        void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            lastUpdateTime = Time.time;
            
            switch (currentState)
            {
                case GameState.Normal:
                    CheckGauntlet();
                    TrackNearObstacles();
                    break;
                case GameState.Gauntlet:
                    GauntletState();
                    break;
            }

            DisplayObstacleInfo();
        }
    }
    void CheckGauntlet()
    {
      if(nearObstacles.Count>0)
      {
        currentState=GameState.Gauntlet;
      }
    }

     void TrackNearObstacles()
    {
        // Detect all colliders within the detection radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectRadius);

          var sortedObstacles = hitColliders
            .Where(collider => collider.gameObject.tag != "Player" && collider.gameObject.tag != "Force" && !collider.GetComponent<AreaEffector2D>())
            .OrderBy(collider => Vector2.Distance(transform.position, collider.transform.position))
            .Take(maxObstaclesToTrack).ToList();

        if(sortedObstacles.Count==0){
            nearObstacles.Clear();
            return;
        }
        // nearObstacles.Clear();

        // Sort colliders by distance
        // System.Array.Sort(hitColliders, (a, b) => 
        //     Vector2.Distance(transform.position, a.transform.position).CompareTo(
        //     Vector2.Distance(transform.position, b.transform.position)));

        // Add the closest obstacles to our list
        foreach (var obstacle in sortedObstacles)
        {
            float distance = Vector2.Distance(transform.position, obstacle.transform.position);
            nearObstacles.Add(new ObstacleInfo(obstacle.gameObject.name, distance));
        }
    }
     void DisplayObstacleInfo()
    {
        string info = "Nearest Obstacles:\n";
        foreach (var obstacle in nearObstacles)
        {
            info += $"{obstacle.name}: {obstacle.distance:F2}m\n";
        }
        // Debug.Log(info);
    }
    private struct ObstacleInfo
    {
        public string name;
        public float distance;

        public ObstacleInfo(string name, float distance)
        {
            this.name = name;
            this.distance = distance;
        }
    }
     void GauntletState()
    {
        TrackNearObstacles();
        if (nearObstacles.Count == 0)
        {
            crashed = false;
            hazard = false;
            currentState = GameState.Normal;
            Debug.Log("Celebrate!!!");
        }
        else
        {
            crashed = true;
            hazard = true;
            Debug.Log($"In Gauntlet {nearObstacles.Count} nearby");
        }
    }
}
