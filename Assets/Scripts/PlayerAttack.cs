using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1f;
    [SerializeField] Transform enemy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 눌러 공격
        {
            Debug.Log("Attack");
            AttackEnemy();
        }
    }

    void AttackEnemy()
    {
        Enemy enemy1 = enemy.GetComponent<Enemy>();

        if (Vector2.Distance(transform.position, enemy.position) <= attackRange) {
            Vector2 knockbackDirection = (enemy.position - transform.position).normalized;
            enemy1.TakeDamage(attackDamage, knockbackDirection);
        }

    }

}
