using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlowerGenerator : MonoBehaviour
{
    [SerializeField] public int spawnInterval = 5;
    [SerializeField] public int flowerNumber = 2;
    [SerializeField] private GameObject flower;
    private BoxCollider2D area;
    private List<GameObject> BigFlowerList = new List<GameObject>();
    [SerializeField] private List<FlowerWeaponSO> flowerWeaponList;

    void Start()
    {
        BigFlowerList.Clear();
        StartCoroutine("Spawn", spawnInterval);
    }

    IEnumerator Spawn(float delayTime)
    {
        if (BigFlowerList.Count < flowerNumber)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = spawnPos.y - 0.8f;
            float dx = Random.Range(-0.5f, 0.5f);

            if (dx <= 0f) dx += Random.Range(-1f, -0.5f);
            else dx += Random.Range(0.5f, 1f);

            spawnPos.x += dx;

            GameObject instance = Instantiate(flower, spawnPos, Quaternion.identity);
            AssignFlowerWeaponType(instance.GetComponent<FlowerWeapon>());
            BigFlowerList.Add(instance);
        }

        yield return new WaitForSeconds(delayTime);

        StartCoroutine("Spawn", spawnInterval);
    }

    private void AssignFlowerWeaponType(FlowerWeapon flowerWeapon)
    {
        int index = Random.Range(0, flowerWeaponList.Count - 1);
        FlowerWeaponSO weaponInstance = Instantiate(flowerWeaponList[index]);
        flowerWeapon.SetFlowerWeaponSO(weaponInstance);
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = area.transform.position;
        Vector2 size = area.size;
        float posX = basePosition.x + Random.Range(-size.x * 2, size.x * 2);

        float posY = basePosition.y + 0.5f;

        Vector2 spawnPos = new Vector2(posX, posY);

        return spawnPos;
    }

}
