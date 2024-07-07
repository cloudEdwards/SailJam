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
    private List<Vector3> occupiedPositions = new List<Vector3>();

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
            for (int i = 0; i < islandDensity; i++)
            {
                Vector3 islandPosition = Vector3.zero;
                bool positionValid = false;

                // Find a valid position that doesn't overlap with the boat or other islands
                for (int attempt = 0; attempt < 10; attempt++)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-tileSize / 2, tileSize / 2), Random.Range(-tileSize / 2, tileSize / 2), 0);
                    islandPosition = tileCenter + randomOffset;

                    if (!IsPositionOccupied(islandPosition))
                    {
                        positionValid = true;
                        break;
                    }
                }

                if (!positionValid)
                {
                    // Skip this island if a valid position couldn't be found
                    continue;
                }

                GameObject selectedPrefab = ChooseIslandPrefab();
                Quaternion islandRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                GameObject island = Instantiate(selectedPrefab, islandPosition, islandRotation);

                // Randomize island scale
                var scaleX = island.transform.localScale.x;
                float scale = Random.Range(0.8f * scaleX, 1.2f * scaleX);
                island.transform.localScale = new Vector3(scale, scale, scale);

                tileObjects.Add(island);
                occupiedPositions.Add(islandPosition);
            }

            spawnedObjects[tileIndex] = tileObjects;
        }
    }

    bool IsPositionOccupied(Vector3 position)
    {
        float minDistance = 5f; // Minimum distance between objects
        if (Vector3.Distance(position, boat.transform.position) < minDistance)
        {
            return true;
        }

        foreach (Vector3 occupiedPosition in occupiedPositions)
        {
            if (Vector3.Distance(position, occupiedPosition) < minDistance)
            {
                return true;
            }
        }

        return false;
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
