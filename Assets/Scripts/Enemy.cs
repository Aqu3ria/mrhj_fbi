using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    private bool isKnockback = false;
    private Vector2 knockbackDirection;
    private float knockbackDistance = 0.5f;
    private float knockbackDuration = 0.1f;
    private float knockbackEndTime;

    public void TakeDamage(int damage, Vector2 direction)
    {
        Debug.Log("TakeDamage called with damage: " + damage + ", knockbackDirection: " + direction);
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
    }
}
