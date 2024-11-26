using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int baseAttackDamage = 5;
    public float baseAttackRange = 1f;
    public float attackCooldown = 0.3f;
    [SerializeField] Transform enemy;
    [SerializeField] Transform flowerHolder;

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private FlowerWeapon grabbedFlower;
    private FlowerWeaponSO grabbedFlowerSO;
    private int currentDurability;
    private float lastAttackTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanAttack()) // 스페이스바를 눌러 공격
        {
            Debug.Log("Attack");
            AttackEnemy();

        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (grabbedFlower == null) GrabWeapon();
            else ReleaseWeapon();
        }

    }

    bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCooldown;
    }

    void GrabWeapon()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right * Mathf.Sign(transform.localScale.x), rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Flower")
        {
            // grab object
            grabbedFlower = hitInfo.collider.gameObject.GetComponent<FlowerWeapon>();
            grabbedFlowerSO = grabbedFlower.GetFlowerWeaponSO();
            currentDurability = grabbedFlowerSO.maxDurability;
            grabbedFlower.transform.SetParent(flowerHolder);
            grabbedFlower.transform.localPosition = Vector3.zero;
            grabbedFlower.transform.localScale = new Vector3(1, 1, 1);
            Destroy(grabbedFlower.GetComponent<CapsuleCollider2D>()); // that way flower is no longer affected by gravity
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

    void ReleaseWeapon()
    {
        // release object
        grabbedFlower.transform.SetParent(null);
        // TODO: implement delete animation
        Destroy(grabbedFlower.gameObject);
        grabbedFlower = null;
        grabbedFlowerSO = null;

    }

    void AttackEnemy()
    {
        lastAttackTime = Time.time;
        Enemy enemy1 = enemy.GetComponent<Enemy>();
        int attackDamage = grabbedFlowerSO != null ? grabbedFlowerSO.attackDamage : baseAttackDamage;
        float attackRange = grabbedFlowerSO != null ? grabbedFlowerSO.attackRange : baseAttackRange;

        if (Vector2.Distance(transform.position, enemy.position) <= attackRange)
        {
            Vector2 knockbackDirection = (enemy.position - transform.position).normalized;
            enemy1.TakeDamage(attackDamage, knockbackDirection);
            Debug.Log($"Attacked with {grabbedFlowerSO?.flowerName ?? "Base Attack"}: Damage={attackDamage}, Range={attackRange}");
        }
        
        if (grabbedFlowerSO != null) 
        {
            currentDurability--;
            Debug.Log($"Remaining durability: {currentDurability}");

            if (currentDurability <= 0)
            {
                ReleaseWeapon();
            }
        }
    }

}
