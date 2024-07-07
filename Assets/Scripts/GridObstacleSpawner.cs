using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IslandPrefabInfo
{
    public GameObject prefab;
    public int rarityWeight = 1; // Higher values mean more common
}

public class GridObstacleSpawner : MonoBehaviour
{
    public GameObject boat;
    public float islandDensity = 10f;
    public List<IslandPrefabInfo> islandPrefabs;
    private float tileSize = 100f;

    // Dictionary to keep track of spawned objects and their grid positions
    private Dictionary<Vector2Int, List<GameObject>> spawnedObjects = new Dictionary<Vector2Int, List<GameObject>>();

    private Vector2Int lastBoatTileIndex;

    void Start()
    {
        lastBoatTileIndex = GetBoatTileIndex(boat.transform.position);
        PrepopulateTiles();
    }

    void Update()
    {
        Vector2Int currentBoatTileIndex = GetBoatTileIndex(boat.transform.position);
        if (currentBoatTileIndex != lastBoatTileIndex)
        {
            PrepopulateTiles();
            lastBoatTileIndex = currentBoatTileIndex;
        }
    }

    Vector2Int GetBoatTileIndex(Vector3 position)
    {
        int xIndex = Mathf.FloorToInt(position.x / tileSize);
        int yIndex = Mathf.FloorToInt(position.y / tileSize);
        return new Vector2Int(xIndex, yIndex);
    }

    void PrepopulateTiles()
    {
        Vector2Int currentTileIndex = GetBoatTileIndex(boat.transform.position);

        // Prepopulate tiles within prepopulateDistance
        for (int x = currentTileIndex.x - 1; x <= currentTileIndex.x + 1; x++)
        {
            for (int y = currentTileIndex.y - 1; y <= currentTileIndex.y + 1; y++)
            {
                Vector2Int tileIndex = new Vector2Int(x, y);
                PopulateTile(tileIndex);
            }
        }
    }

    void PopulateTile(Vector2Int tileIndex)
    {
        // Check if the tile has already been populated
        if (!spawnedObjects.ContainsKey(tileIndex))
        {
            List<GameObject> tileObjects = new List<GameObject>();

            // Populate the tile with objects (random positions)
            Vector3 tileCenter = new Vector3(tileIndex.x * tileSize, tileIndex.y * tileSize, 0);
            for (int i = 0; i < islandDensity; i++) // Example: spawn 10 islands per tile
            {
                GameObject selectedPrefab = ChooseIslandPrefab();
                Vector3 randomOffset = new Vector3(Random.Range(-tileSize / 2, tileSize / 2), Random.Range(-tileSize / 2, tileSize / 2), 0);
                Vector3 islandPosition = tileCenter + randomOffset;
                Quaternion islandRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                GameObject island = Instantiate(selectedPrefab, islandPosition, islandRotation);
                
                // Randomize island scale
                float scale = Random.Range(0.8f, 1.2f);
                island.transform.localScale = new Vector3(scale, scale, scale);

                tileObjects.Add(island);
            }

            spawnedObjects[tileIndex] = tileObjects;
        }
    }

    GameObject ChooseIslandPrefab()
    {
        // Calculate total weight
        int totalWeight = 0;
        foreach (var info in islandPrefabs)
        {
            totalWeight += info.rarityWeight;
        }

        // Generate a random number within total weight
        int randomNum = Random.Range(0, totalWeight);

        // Choose the prefab based on rarity weights
        int cumulativeWeight = 0;
        foreach (var info in islandPrefabs)
        {
            cumulativeWeight += info.rarityWeight;
            if (randomNum < cumulativeWeight)
            {
                return info.prefab;
            }
        }

        // Fallback (shouldn't normally happen)
        return islandPrefabs[islandPrefabs.Count - 1].prefab;
    }
}
