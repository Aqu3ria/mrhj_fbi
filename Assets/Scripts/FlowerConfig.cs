using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerConfig : MonoBehaviour
{

    float time = 0;

    void Start() {

        resetAnim();
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 6f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, time/3);
        }
        else
        {
            time = 0;
            this.gameObject.SetActive(false);
        }
        time += Time.deltaTime;

    }

    public void resetAnim()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        this.gameObject.SetActive(true);
        time = 0;
    }
}
