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

    public void CreateWorld()
    {
        CreateGround();
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