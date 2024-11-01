using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]float maxSpeed = 0.5f;
    float accel;
    float deccel;

    [SerializeField] int framesToAccel = 20;
    [SerializeField] int framesToDeccel = 10; 

    [SerializeField]float velocityX = 0f;
    Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        accel = maxSpeed/framesToAccel;
        deccel = maxSpeed/framesToDeccel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Move(h, v);

        Animate(h , v);
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
            transform.Translate(0, v * maxSpeed * Time.deltaTime, 0);
        }
    }

    void Move(float horizontalInput, float verticalInput) {
        float calculatedVelocity = velocityX;

        //holding an input
        if (horizontalInput != 0) {
            // going right, accelerating to the right
            if (horizontalInput > 0 && velocityX >= 0) calculatedVelocity += accel;
            // going right, deccelerating to the left
            if (horizontalInput < 0 && velocityX > 0) calculatedVelocity -= deccel;
            //going left, accelerating to the left
            if (horizontalInput < 0 && velocityX <= 0) calculatedVelocity -= accel;
            //going left, deccelerating to the right
            if (horizontalInput > 0 && velocityX < 0) calculatedVelocity += deccel;
        }
        // not holding an input
        else {
            calculatedVelocity -= deccel * Math.Sign(calculatedVelocity);
        }

        // set new velocity
        if (calculatedVelocity >= 0) velocityX = Math.Min(calculatedVelocity, maxSpeed);
        else velocityX = Math.Max(calculatedVelocity, -maxSpeed);
        

        Vector3 velocity = Vector3.right * velocityX * Time.deltaTime;
        transform.position += velocity;
    }

    void Animate(float horizontalInput, float verticalInput) {
        Vector3 currentScale = transform.localScale;
        if((horizontalInput < 0 && currentScale.x > 0) || (horizontalInput > 0 && currentScale.x < 0)){
            currentScale.x *= -1;
        }
        transform.localScale = currentScale;
    }

    
}
