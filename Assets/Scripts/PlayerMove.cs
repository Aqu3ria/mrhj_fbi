using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance { get; private set; }

    [SerializeField]float maxSpeed = 0.5f;
    float accel;
    float deccel;
    float stopThresholdValue = 0.1f;

    [SerializeField] int framesToAccel = 20;
    [SerializeField] int framesToDeccel = 10; 

    [SerializeField]float velocityX = 0f;
    Rigidbody2D rb;
    [SerializeField] Panel_GameOver panelGameOver;
    

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

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
        if (transform.position.y < -10) 
            GameOver();
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

    void Ladder(float verticalInput) {
        if (gameObject.layer == 8) {
            rb.velocity = Vector2.zero;
            transform.Translate(0, verticalInput * maxSpeed * Time.deltaTime, 0);
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
        velocityX = Mathf.Clamp(calculatedVelocity, -maxSpeed, maxSpeed);
        
        // set velocity to 0 if at close to 0 value
        if (Math.Abs(velocityX) < stopThresholdValue) velocityX = 0;

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

    void GameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.Show(); 
        }
        else
        {
            Debug.LogError("Panel_GameOver가 설정되지 않았습니다!");
        }

        Time.timeScale = 0f;
    }
}
