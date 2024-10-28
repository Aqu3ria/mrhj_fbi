using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]float speed = 0.5f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        transform.position += Vector3.right * h * speed * Time.deltaTime;

        Vector3 currentScale = transform.localScale;
        if((h < 0 && currentScale.x > 0) || (h > 0 && currentScale.x < 0)){
            currentScale.x *= -1;
        }
        transform.localScale = currentScale;

        Ladder(v);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 6) {
            gameObject.layer = 8;
            rb.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == 6) {
            LadderOut();
        }
    }

    void LadderOut() {
        rb.gravityScale = 1;
        gameObject.layer = 9;
    }

    void Ladder(float v) {
        if (gameObject.layer == 8) {
            transform.Translate(0, v * speed * Time.deltaTime, 0);
        }
    }

    
}
