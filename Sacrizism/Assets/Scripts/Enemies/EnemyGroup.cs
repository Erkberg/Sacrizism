using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public EnemyGroupType enemyGroupType;


}

public enum EnemyGroupType
{
    Weak,
    Average,
    Strong
}