using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public int level = 0;
	
	// Update is called once per frame
	protected void Update ()
    {
		
	}
}

public enum EnemyType
{
    Runner,
    Shooter,
    Healer,
    Boss
}