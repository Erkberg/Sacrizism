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

    private const int grassAmount = 256;
    private const int treesAmount = 32;
    private const int rocksAmount = 32;

    public Transform groundHolder;
    public Transform groundTilePrefab;
    public Transform lavaHolder;
    public Transform lavaPrefab;
    public Transform grassHolder;
    public Transform grassPrefab;
    public Transform rockHolder;
    public Transform rockPrefab;
    public Transform treeHolder;
    public Transform treePrefab;

    private List<Transform> allRocks = new List<Transform>();
    private List<Transform> allTrees = new List<Transform>();

    public void CreateWorld()
    {
        CreateLava();
        CreateGround();
        CreateGrass();
        CreateRocks();
        CreateTrees();
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

    public void CreateGrass()
    {
        for (int i = 0; i <= grassAmount; i++)
        {
            Instantiate(grassPrefab, GetRandomWorldPosition(), Quaternion.identity, grassHolder);
        }
    }

    public void CreateRocks()
    {
        for (int i = 0; i <= rocksAmount; i++)
        {
            allRocks.Add(Instantiate(rockPrefab, GetRandomWorldPosition(), Quaternion.identity, rockHolder));
        }
    }

    public void CreateTrees()
    {
        for (int i = 0; i <= treesAmount; i++)
        {
            allTrees.Add(Instantiate(treePrefab, GetRandomWorldPosition(), Quaternion.identity, treeHolder));
        }
    }

    public void DestroyAllRocksAndTrees()
    {
        for(int i = 0; i < allRocks.Count; i++)
        {
            Destroy(allRocks[i].gameObject);
        }

        for (int i = 0; i < allRocks.Count; i++)
        {
            Destroy(allTrees[i].gameObject);
        }
    }

    public Vector3 GetRandomWorldPosition()
    {
        float radius = WorldSize * TileSize;
        return new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);
    }
}