using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1f;
    [SerializeField] Transform enemy;

    [SerializeField]
    private Transform grabPoint;

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private GameObject grabbedObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 눌러 공격
        {   
            if (grabbedObject == null) {
                Debug.Log("pickup");
                GrabWeapon(); 
            }
            
            Debug.Log("Attack");
            AttackEnemy();
            
        }

        if (Input.GetKeyDown(KeyCode.Q) && grabbedObject != null) {
            ReleaseWeapon();
        }

    }

    void GrabWeapon() {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right * Mathf.Sign(transform.localScale.x), rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "Flower") 
        {
            // grab object
            grabbedObject = hitInfo.collider.gameObject;
            grabbedObject.transform.position = grabPoint.position;
            grabbedObject.transform.SetParent(transform);
            
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

    void ReleaseWeapon() {
        // release object
        grabbedObject.transform.SetParent(null);
        // TODO: implement delete animation
        Destroy(grabbedObject);
        grabbedObject = null;

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
