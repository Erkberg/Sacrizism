using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const int amountOfEnemyGroups = 32;
    private const float minGroupDistance = 4f;

    public Transform enemiesHolder;
    public Transform enemyGroupPrefab;

    private List<Transform> enemyGroups;

    public void CreateEnemies()
    {
        enemyGroups = new List<Transform>();

        for (int i = 0; i < amountOfEnemyGroups; i++)
        {
            Vector3 position = Vector3.zero;
            int counter = 0;

            while(!IsPositionOkay(position))
            {
                position = GameManager.instance.worldManager.GetRandomWorldPosition();

                counter++;
                if(counter > 10)
                {
                    Debug.Log("no more space in hell...");
                    break;
                }
            }

            Transform enemyGroup = Instantiate(enemyGroupPrefab, position, Quaternion.identity, enemiesHolder);
            enemyGroup.GetComponent<EnemyGroup>().CreateRandomGroup();
            enemyGroups.Add(enemyGroup);
        }
    }

    private bool IsPositionOkay(Vector3 position)
    {
        bool positionIsOkay = true;

        // keep area around player spawn clean
        if(Vector3.Distance(Vector3.zero, position) < 16f)
        {
            positionIsOkay = false;
        }
        else if(enemyGroups != null && enemyGroups.Count > 0)
        {
            foreach(Transform group in enemyGroups)
            {
                if(Vector3.Distance(group.position, position) < minGroupDistance)
                {
                    positionIsOkay = false;
                    break;
                }
            }
        }

        return positionIsOkay;
    }
}
