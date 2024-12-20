using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set;}

    public event EventHandler OnFlowerChange;
    public event EventHandler<OnFlowerGrabEventArgs> OnFlowerGrab;
    public class OnFlowerGrabEventArgs : EventArgs
    {
        public GameObject flowerGrabbed;
    }
    public int baseAttackDamage = 5;
    public float baseAttackRange = 1f;
    public float attackCooldown = 0.3f;
    [SerializeField] Transform flowerHolder;

    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private FlowerWeapon grabbedFlower;
    private FlowerWeaponSO grabbedFlowerSO;
    private int currentDurability;
    [SerializeField] private float lastAttackTime;

    private void Awake()
    {
        Instance = this;
    }

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
            attackCooldown = grabbedFlowerSO.cooldown;
            grabbedFlower.transform.SetParent(flowerHolder);
            grabbedFlower.transform.localPosition = Vector3.zero;
            grabbedFlower.transform.localScale = new Vector3(1, 1, 1);
            Destroy(grabbedFlower.GetComponent<CapsuleCollider2D>()); // that way flower is no longer affected by gravity

            OnFlowerChange?.Invoke(this, EventArgs.Empty);
            OnFlowerGrab?.Invoke(this, new OnFlowerGrabEventArgs { flowerGrabbed = grabbedFlower.gameObject });
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

    void ReleaseWeapon()
    {
        // release object
        grabbedFlower.transform.SetParent(null);
        // TODO: implement delete animation
        Destroy(grabbedFlower.gameObject);
        attackCooldown = 0.3f;
        grabbedFlower = null;
        grabbedFlowerSO = null;

        OnFlowerChange?.Invoke(this, EventArgs.Empty);
    }

    void AttackEnemy()
    {
        if (grabbedFlowerSO != null && grabbedFlowerSO.flowerName == "Pear")
        {
            RestorePlayerHP(100);
            currentDurability--;
            ReleaseWeapon();
            return;
        }
        lastAttackTime = Time.time;
        RaycastHit2D[] hitInfo = Physics2D.RaycastAll(rayPoint.position, transform.right * Mathf.Sign(transform.localScale.x), baseAttackRange);
        
        Enemy enemy1 = null;
        foreach (RaycastHit2D hit in hitInfo)
        {
            if (hit.collider != null && (hit.collider.gameObject.tag == "Enemy")) 
            {
                enemy1 = hit.collider.gameObject.GetComponent<Enemy>();
                break;
            }
        }
        int attackDamage = grabbedFlowerSO != null ? grabbedFlowerSO.attackDamage : baseAttackDamage;
        float attackRange = grabbedFlowerSO != null ? grabbedFlowerSO.attackRange : baseAttackRange;

        if (enemy1 != null) {
            if (Vector2.Distance(transform.position, enemy1.transform.position) <= attackRange)
            {
                Vector2 knockbackDirection = (enemy1.transform.position - transform.position).normalized;
                enemy1.TakeDamage(attackDamage, knockbackDirection);
                //Debug.Log($"Attacked with {grabbedFlowerSO?.flowerName ?? "Base Attack"}: Damage={attackDamage}, Range={attackRange}");
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
    void RestorePlayerHP(int amount)
    {
        Transform player = PlayerMove.Instance.transform;
        PlayerPercent playerPercent = player.GetComponent<PlayerPercent>();
        playerPercent.TakeDamage(-amount, Vector2.zero);
    }
    public FlowerWeaponSO GetCurrentFlowerWeaponSO()
    {
        return grabbedFlowerSO;
    }

    public float GetDurabilityNormalized()
    {
        if(grabbedFlowerSO == null)
        {
            return 0f;
        }
        else
        {
            float durab = ((float) currentDurability) / ((float) grabbedFlowerSO.maxDurability);
            return durab;
            
        }
    }
}
