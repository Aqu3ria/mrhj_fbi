using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DurabilityUI : MonoBehaviour
{
    [SerializeField] private GameObject flowerHolder;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Image fillImage; 

    private FlowerWeaponSO currentFlowerWeapon;

    private void Start()
    {
        PlayerAttack.Instance.OnFlowerChange += PlayerAttack_OnFlowerChange;
        UpdateVisuals();
    }

    private void PlayerAttack_OnFlowerChange(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        currentFlowerWeapon = PlayerAttack.Instance.GetCurrentFlowerWeaponSO();

        if(currentFlowerWeapon == null)
        {
            flowerHolder.SetActive(false);
            percentText.text = "Grab a Flower";
        }
        else
        {
            flowerHolder.SetActive(true);
            Image flowerImage = flowerHolder.GetComponent<Image>();
            flowerImage.sprite = currentFlowerWeapon.flowerSprite;
        }
    }

    private void Update()
    {
        if(currentFlowerWeapon != null)
        {
            float currentDurability = PlayerAttack.Instance.GetDurabilityNormalized();
            fillImage.fillAmount = currentDurability; 
            percentText.text = Mathf.Abs(currentDurability  * 100f) + "%";
        }
        // else
        // {
        //     percentText.text = "Grab a Flower";
        // }
    }
}
