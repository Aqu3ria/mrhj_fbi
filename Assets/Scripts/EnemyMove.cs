using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 0.75f;
    // [SerializeField] float followRange = 5f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] int attackDamage = 10;

    [SerializeField] float rayDistance = 15;


    // for debugging 
    [SerializeField] GameObject currentLadder;

    Transform player;

    float attackCooldown = 1;
    float lastAttackTime = 0;
    Rigidbody2D rb;
    Transform targetLadder;

    bool isClimbing = false;
    bool lookingToClimb = false;

    bool isMoving = false;
    public float positionTolerance = 0.05f;  // The tolerance range for Y position variation
    public float stuckTime = 4f;  // Time in seconds to check for "stuck" condition
    private float lastYPosition;
    private bool isStuck = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerMove.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) > 0.1f)
        {
            if(!isClimbing)
            {
                // lookingToClimb = true;
                targetLadder = FindNearestLadder();
            }

            if (targetLadder != null)
            {   
                FollowObject(targetLadder.position.x);
                if(isClimbing) ClimbLadder();

                // if (Mathf.Abs(targetLadder.position.x - transform.position.x) < 0.1f)   
                // {
                //     isClimbing = true;
                //     ClimbLadder();
                // }
            }
        }
        else
        {   
            if (!isMoving)
                FollowObject(player.position.x);
        }
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {

            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void ClimbLadder()
    {   
        StartCheckingForStuck();

        float distanceY = transform.position.y - player.position.y;
        if (distanceY > 0 && !isMoving)
            transform.position += speed * Time.deltaTime * Vector3.down * 2f;
        else if (distanceY < 0 && !isMoving)
            transform.position += speed * Time.deltaTime * Vector3.up * 2f;
        if (isStuck) 
            StartCoroutine(EscapeLadder(0.3f));
    }

     // Start checking when you call this method
    public void StartCheckingForStuck()
    {
        StartCoroutine(CheckIfStuck());
    }

    private IEnumerator CheckIfStuck()
    {
        lastYPosition = transform.position.y;  // Initialize the last Y position
        float timeElapsed = 0f;  // Time counter to track how long the Y position has been constant

        while (timeElapsed < stuckTime)
        {
            // Check if the Y position is within the tolerance range
            if (Mathf.Abs(transform.position.y - lastYPosition) <= positionTolerance)
            {
                timeElapsed += Time.deltaTime;  // Increase the time since Y position has been constant
            }
            else
            {
                timeElapsed = 0f;  // Reset if the Y position changes significantly
            }

            lastYPosition = transform.position.y;  // Update the last known Y position
            yield return null;  // Wait until the next frame
        }

        // If timeElapsed reaches stuckTime, set isStuck to true
        isStuck = true;
        Debug.Log("Enemy is stuck on the ladder.");
    }

    private IEnumerator EscapeLadder(float duration)
    {   
        isStuck = false;

        Debug.Log("Escaping Ladder");
        isMoving = true;  // Set flag to prevent other movement
        float timePassed = 0f;

        float direction = Mathf.Sign(transform.position.x - player.position.x);

        while (timePassed < duration)
        {
            transform.position += speed / 40 * Time.deltaTime * Vector3.left * direction;
            timePassed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;  // Reset flag after movement is complete
    }

    void FollowObject(float x)
    {
        float distanceX = transform.position.x - x;
        if (distanceX > 0)
            transform.position += speed * Time.deltaTime * Vector3.left;
        else if (distanceX < 0)
            transform.position += speed * Time.deltaTime * Vector3.right;

        
        if (!isClimbing)
        {
            Vector3 currentScale = transform.localScale;
            if ((distanceX > 0 && currentScale.x > 0) || (distanceX < 0 && currentScale.x < 0))
            {
                currentScale.x *= -1;
            }
            transform.localScale = currentScale;
        }
    }
        
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            gameObject.layer = 8;
            rb.gravityScale = 0;
        //     if(lookingToClimb)
        //     {
        //         isClimbing = true;
        //     }
            currentLadder = other.gameObject;
            isClimbing = true;
         }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
    
        if (other.gameObject.CompareTag("Ladder"))
        {   
            isClimbing = false;
            currentLadder = null;
            rb.gravityScale = 1;
            gameObject.layer = 9;
        }
    }

    void AttackPlayer()
    {
        PlayerPercent playerPercent = player.GetComponent<PlayerPercent>();
        if (playerPercent != null)
        {
            Vector2 knockbackDirection = (player.position - transform.position).normalized; // 넉백 방향 설정
            playerPercent.TakeDamage(attackDamage, knockbackDirection); 
        }
    }

    Transform FindNearestLadder()
    {   
        RaycastHit2D[] hitInfo = Physics2D.RaycastAll(new Vector3(transform.position.x - rayDistance/2, transform.position.y-0.5f, transform.position.z), transform.right, rayDistance);
        List<GameObject> ladders = new List<GameObject>(); 
        Debug.DrawRay(new Vector3(transform.position.x - rayDistance/2, transform.position.y-0.5f, transform.position.z), transform.right * rayDistance);

        foreach (RaycastHit2D hit in hitInfo)
        {   
            Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider != null && (hit.collider.gameObject.tag == "Ladder" || hit.collider.gameObject.tag == "LadderTopCollider")) 
            {   
                GameObject ladder;
                if (hit.collider.gameObject.tag == "LadderTopCollider") ladder = hit.collider.gameObject.transform.parent.gameObject;
                else ladder = hit.collider.gameObject;

                // get rid of case where the ladder is above the player
                if (ladder.transform.position.y > player.position.y) continue;
                
                ladders.Add(ladder);
            }
        }

        Transform nearestLadder = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject ladder in ladders)
        {
            float distance = (player.position - ladder.transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestLadder = ladder.transform;
            }
        }
        return nearestLadder;
    }
}
