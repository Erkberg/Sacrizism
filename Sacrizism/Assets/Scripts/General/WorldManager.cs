using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldManager : MonoBehaviour
{
    private const int WorldSize = 3;
    private const float TileSize = 10.24f;

    public Transform groundHolder;
    public Transform groundTilePrefab;
    public Transform lavaHolder;
    public Transform lavaPrefab;

    public void CreateWorld()
    {
        CreateLava();
        CreateGround();
    }

    public void CreateLava()
    {
        for (int i = -WorldSize - 1; i <= WorldSize + 1; i++)
        {
            Instantiate(lavaPrefab, new Vector3(i * TileSize, (-WorldSize - 1) * TileSize, 0f), Quaternion.identity, lavaHolder);
            Instantiate(lavaPrefab, new Vector3(i * TileSize, (WorldSize + 1) * TileSize, 0f), Quaternion.identity, lavaHolder);
        }

        for (int i = -WorldSize; i <= WorldSize; i++)
        {
            Instantiate(lavaPrefab, new Vector3((-WorldSize - 1) * TileSize, i * TileSize, 0f), Quaternion.identity, lavaHolder);
            Instantiate(lavaPrefab, new Vector3((WorldSize + 1) * TileSize, i * TileSize, 0f), Quaternion.identity, lavaHolder);
        }
    }

    public void CreateGround()
    {
        for (int i = -WorldSize; i <= WorldSize; i++)
        {
            for (int j = -WorldSize; j <= WorldSize; j++)
            {
#if UNITY_EDITOR
                Transform clone = PrefabUtility.InstantiatePrefab(groundTilePrefab as Transform) as Transform;
                clone.transform.position = new Vector3(i * TileSize, j * TileSize, 0f);
                clone.name = "GroundTile_" + i + "_" + j;
                clone.transform.parent = groundHolder;
#else
                Instantiate(groundTilePrefab, new Vector3(i * TileSize, j * TileSize, 0f), Quaternion.identity, groundHolder);
#endif
            }
        }
    }

    public Vector3 GetRandomWorldPosition()
    {
        float radius = WorldSize * TileSize;
        return new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);
    }
}