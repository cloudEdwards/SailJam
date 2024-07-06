using System.Collections.Generic;
using UnityEngine;

public class GridObstacleSpawner : MonoBehaviour
{
    public GameObject boat;
    public GameObject objectPrefab;
    private float tileSize = 100f;

    // Dictionary to keep track of spawned objects and their grid positions
    private Dictionary<Vector2Int, List<GameObject>> spawnedObjects = new Dictionary<Vector2Int, List<GameObject>>();

    private Vector2Int lastBoatTileIndex;

    void Start()
    {
        lastBoatTileIndex = GetBoatTileIndex(boat.transform.position);
        PopulateTile(lastBoatTileIndex);
    }

    void Update()
    {
        Vector2Int currentBoatTileIndex = GetBoatTileIndex(boat.transform.position);
        if (currentBoatTileIndex != lastBoatTileIndex)
        {
            PopulateTile(currentBoatTileIndex);
            lastBoatTileIndex = currentBoatTileIndex;
        }
    }

    Vector2Int GetBoatTileIndex(Vector3 position)
    {
        int xIndex = Mathf.FloorToInt(position.x / tileSize);
        int yIndex = Mathf.FloorToInt(position.y / tileSize);
        return new Vector2Int(xIndex, yIndex);
    }

    void PopulateTile(Vector2Int tileIndex)
    {
        // Check if the tile has already been populated
        if (!spawnedObjects.ContainsKey(tileIndex))
        {
            List<GameObject> tileObjects = new List<GameObject>();

            // Populate the tile with objects (customize this as needed)
            Vector3 tileCenter = new Vector3(tileIndex.x * tileSize, tileIndex.y * tileSize, 0);
            for (int i = 0; i < 5; i++) // Example: spawn 5 objects per tile
            {
                Vector3 offset = new Vector3(Random.Range(-tileSize / 2, tileSize / 2), Random.Range(-tileSize / 2, tileSize / 2), 0);
                Vector3 spawnPosition = tileCenter + offset;
                GameObject obj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                tileObjects.Add(obj);
            }

            spawnedObjects[tileIndex] = tileObjects;
        }
    }
}
