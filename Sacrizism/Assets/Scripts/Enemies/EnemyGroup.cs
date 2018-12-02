using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyGroupType groupType;
    public EnemyGroupSize groupSize;
    public List<Enemy> enemies;

    private bool isAngered = false;

    private const float spawnRadius = 3f;

    public void CreateRandomGroup()
    {
        CreateGroup(GetRandomGroupType(), GetRandomGroupSize());
    }

    public void CreateGroup(EnemyGroupType groupType, EnemyGroupSize groupSize)
    {
        this.groupType = groupType;
        this.groupSize = groupSize;

        int maxLevel = 0;
        if(groupType == EnemyGroupType.Average)
        {
            maxLevel = 1;
        }
        if (groupType == EnemyGroupType.Strong)
        {
            maxLevel = 2;
        }

        int currentSize = Random.Range(2, 4);
        if (groupSize == EnemyGroupSize.Average)
        {
            currentSize = Random.Range(4, 6);
        }
        if (groupSize == EnemyGroupSize.Big)
        {
            currentSize = Random.Range(6, 9);
        }

        enemies = new List<Enemy>();

        for(int i = 0; i < currentSize; i++)
        {
            Enemy enemy = Instantiate(GameManager.instance.enemyManager.GetEnemyPrefabByType(GetRandomEnemyType()),
                                                   GetRandomGroupPosition(), Quaternion.identity, transform).GetComponent<Enemy>();
            enemy.SetLevel(Random.Range(0, maxLevel + 1));
            enemy.enemyGroup = this;
            enemies.Add(enemy);
        }
    }

    public void SetAngered()
    {
        if(!isAngered)
        {
            isAngered = true;

            if (enemies != null && enemies.Count > 0)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.SetAngered();
                }
            }
        }        
    }

    private Vector3 GetRandomGroupPosition()
    {
        return transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), 0f);
    }

    public EnemyGroupType GetRandomGroupType()
    {
        var values = System.Enum.GetValues(typeof(EnemyGroupType));
        return (EnemyGroupType) values.GetValue(Random.Range(0, values.Length));
    }

    public EnemyGroupSize GetRandomGroupSize()
    {
        var values = System.Enum.GetValues(typeof(EnemyGroupSize));
        return (EnemyGroupSize)values.GetValue(Random.Range(0, values.Length));
    }

    public EnemyType GetRandomEnemyType()
    {
        var values = System.Enum.GetValues(typeof(EnemyType));
        return (EnemyType)values.GetValue(Random.Range(0, values.Length - 1));
    }
}

public enum EnemyGroupType
{
    Weak,
    Average,
    Strong
}

public enum EnemyGroupSize
{
    Small,
    Average,
    Big
}