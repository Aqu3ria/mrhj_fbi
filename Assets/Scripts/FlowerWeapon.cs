using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerWeapon : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private FlowerWeaponSO flowerWeaponSO;
    private float time = 0;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        //resetAnim();
    }

    public void SetFlowerWeaponSO(FlowerWeaponSO inputFlowerWeaponSO)
    {
        flowerWeaponSO = inputFlowerWeaponSO;
        sprite.sprite = flowerWeaponSO.flowerSprite;
    }

    public FlowerWeaponSO GetFlowerWeaponSO()
    {
        return flowerWeaponSO;
    }

    // Update is called once per frame
    void Update()
    {
        //FlowerBloom();
    }

    public void resetAnim()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        this.gameObject.SetActive(true);
        time = 0;
    }

    void FlowerBloom()
    {
        if (time < 6f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, time / 3);
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            this.gameObject.SetActive(false);
        }

    }
}
