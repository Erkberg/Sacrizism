using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyGroupType groupType;
    public List<Enemy> enemies;

    private bool isAngered = false;

    public void CreateGroup(EnemyGroupType groupType)
    {
        this.groupType = groupType;


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
}

public enum EnemyGroupType
{
    Weak,
    Average,
    Strong
}