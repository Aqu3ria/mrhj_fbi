using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event EventHandler onEnemyDeath;
    public int health = 10;
    private bool isKnockback = false;
    private Vector2 knockbackDirection;
    private float knockbackDistance = 0.5f;
    private float knockbackDuration = 0.1f;
    private float knockbackEndTime;

    public bool alive;

    private void Start()
    {
        alive = true;
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        health += damage;
        
        knockbackDirection = direction.normalized;
        isKnockback = true;
        knockbackEndTime = Time.time + knockbackDuration;
    }

    private void Update()
    {
        if (isKnockback)
        {
            if (Time.time < knockbackEndTime)
            {
                transform.Translate(knockbackDirection * (knockbackDistance / knockbackDuration) * health / 10 * Time.deltaTime);
            }
            else
            {
                isKnockback = false;
            }
        }

        if(transform.position.y < -3.23)
        {
           if(alive) InvokeDeath(); 
        }
    }

    private void InvokeDeath()
    {
        alive = false;
        onEnemyDeath?.Invoke(this, EventArgs.Empty);
    }
}
