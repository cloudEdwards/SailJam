using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public GameObject boat;
    public GameObject waterTilePrefab;
    private Vector2 gridSize = new Vector2(3, 3);
    private float tileSize = 100f;
    private List<GameObject> waterTiles = new List<GameObject>();
    private Vector3 lastBoatTileIndex;

    void Start()
    {
        InitializeGrid();
        lastBoatTileIndex = GetBoatTileIndex();
    }

    void Update()
    {
        CheckBoatPosition();
    }

    void InitializeGrid()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0);
                GameObject tile = Instantiate(waterTilePrefab, position, Quaternion.identity);
                waterTiles.Add(tile);
            }
        }
    }

    void CheckBoatPosition()
    {
        Vector3 currentBoatTileIndex = GetBoatTileIndex();
        if (currentBoatTileIndex != lastBoatTileIndex)
        {
            RearrangeTiles(currentBoatTileIndex);
            lastBoatTileIndex = currentBoatTileIndex;
        }
    }

    Vector3 GetBoatTileIndex()
    {
        Vector3 boatPosition = boat.transform.position;
        return new Vector3(Mathf.Floor(boatPosition.x / tileSize), Mathf.Floor(boatPosition.y / tileSize), 0);
    }

    void RearrangeTiles(Vector3 currentBoatTileIndex)
    {
        Vector3 boatPosition = boat.transform.position;
        List<Vector3> newPositions = new List<Vector3>();

        foreach (GameObject tile in waterTiles)
        {
            Vector3 tilePosition = tile.transform.position;
            Vector3 tileIndex = new Vector3(Mathf.Floor(tilePosition.x / tileSize), Mathf.Floor(tilePosition.y / tileSize), 0);

            if (tileIndex.x > currentBoatTileIndex.x + 1)
                tilePosition.x -= gridSize.x * tileSize;
            else if (tileIndex.x < currentBoatTileIndex.x - 1)
                tilePosition.x += gridSize.x * tileSize;

            if (tileIndex.y > currentBoatTileIndex.y + 1)
                tilePosition.y -= gridSize.y * tileSize;
            else if (tileIndex.y < currentBoatTileIndex.y - 1)
                tilePosition.y += gridSize.y * tileSize;

            newPositions.Add(tilePosition);
        }

        for (int i = 0; i < waterTiles.Count; i++)
        {
            waterTiles[i].transform.position = newPositions[i];
        }
    }
}
