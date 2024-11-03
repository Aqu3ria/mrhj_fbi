using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 0.4f;
    [SerializeField] float followRange = 5f;
    Rigidbody2D rb;
    Transform targetLadder;

    bool isClimbing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) > 0.1f)
        {
            targetLadder = FindNearestLadder();
            if (targetLadder != null)
            {
                FollowObject(targetLadder.position.x);
                if (Mathf.Abs(targetLadder.position.x - transform.position.x) < 0.1f)
                {
                    isClimbing = true;
                    ClimbLadder();
                }
            }
        }
        else
        {
            isClimbing = false;
            FollowObject(player.position.x);
        }
    }

    void ClimbLadder()
    {
        float distanceY = transform.position.y - player.position.y;
        if (distanceY > 0)
            transform.position += speed * Time.deltaTime * Vector3.down;
        else
            transform.position += speed * Time.deltaTime * Vector3.up;
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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            rb.gravityScale = 1;
            gameObject.layer = 9;
        }
    }

    Transform FindNearestLadder()
    {
        GameObject[] ladders = GameObject.FindGameObjectsWithTag("Ladder");
        Transform nearestLadder = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject ladder in ladders)
        {
            float distanceX = Mathf.Abs(transform.position.x - ladder.transform.position.x);
            if (distanceX < minDistance)
            {
                minDistance = distanceX;
                nearestLadder = ladder.transform;
            }
        }
        return nearestLadder;
    }
}
