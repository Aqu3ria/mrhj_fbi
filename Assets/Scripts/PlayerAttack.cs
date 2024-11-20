using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 5;
    public float attackRange = .5f;
    [SerializeField] Transform enemy;
    [SerializeField] Transform flowerHolder;
    private Transform grabPoint;

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private FlowerWeapon grabbedFlower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 눌러 공격
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

    void GrabWeapon()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right * Mathf.Sign(transform.localScale.x), rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Flower")
        {
            // grab object
            grabbedFlower = hitInfo.collider.gameObject.GetComponent<FlowerWeapon>();
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

    }

    void AttackEnemy()
    {
        Enemy enemy1 = enemy.GetComponent<Enemy>();

        if (Vector2.Distance(transform.position, enemy.position) <= attackRange)
        {
            Vector2 knockbackDirection = (enemy.position - transform.position).normalized;
            enemy1.TakeDamage(attackDamage, knockbackDirection);
        }

    }

}
