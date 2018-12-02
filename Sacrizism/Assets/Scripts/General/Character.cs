﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public HPBar hpBar;

    public int maxHP = 3;
    private int currentHP;

    public UnityEvent onTakeDamage;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void SetMaxHP(int amount)
    {
        maxHP = amount;
        currentHP = maxHP;

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    public void TakeDamage(int amount)
    {
        onTakeDamage.Invoke();

        currentHP -= amount;        

        if(currentHP <= 0)
        {
            Die();
        }

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    public void Heal(int amount)
    {
        currentHP += amount;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        hpBar.SetWidthPercentage((float)currentHP / maxHP);
    }

    protected void Die()
    {
        if(gameObject.CompareTag(Tags.PlayerTag))
        {
            Destroy(gameObject);
            GameManager.instance.OnDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
