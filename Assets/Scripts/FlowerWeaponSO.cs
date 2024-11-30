using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FlowerWeaponSO : ScriptableObject
{
    public string flowerName;
    public Sprite flowerSprite;
    public int attackDamage;
    public float attackRange;
    public int maxDurability;
    public float cooldown;
}
