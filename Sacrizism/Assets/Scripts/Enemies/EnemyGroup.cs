using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyGroupType groupType;
    public EnemyGroupSize groupSize;
    public List<Enemy> enemies;

    private bool isAngered = false;

    public void CreateRandomGroup()
    {
        CreateGroup(GetRandomGroupType(), GetRandomGroupSize());
    }

    public void CreateGroup(EnemyGroupType groupType, EnemyGroupSize groupSize)
    {
        this.groupType = groupType;
        this.groupSize = groupSize;


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