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

    void Start()
    {
        StartCoroutine("Spawn", spawnInterval);
        BigFlowerList.Clear();
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

            // GameObject [] SpawningPlatforms = GameObject.FindGameObjectsWithTag("FlowerSpawnable_Platform");
            // GameObject SpawningPlatform = SpawningPlatforms[Random.Range(0, SpawningPlatforms.Length)];
            // Debug.Log(SpawningPlatform);

            // area = SpawningPlatform.GetComponent<BoxCollider2D>();
            // Debug.Log(area);

            // Vector3 spawnPos = GetRandomPosition();
            GameObject instance = Instantiate(flower, spawnPos, Quaternion.identity);
            BigFlowerList.Add(instance);
        }

        yield return new WaitForSeconds(delayTime);

        StartCoroutine("Spawn", spawnInterval);
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
