using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_config : MonoBehaviour
{
    [SerializeField] private int attack = 10;
    [SerializeField] private int percentage = 0;
    [SerializeField] private float attackSpeedBase = 1.0f;
    [SerializeField] private bool hasWeapon = false;
    [SerializeField] private bool onAir = false;
    [SerializeField] private bool onRope = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onRope || onAir) {
            attackSpeedBase = 0f;
        } else {
            attackSpeedBase = 1.0f;
        }
    }
}
