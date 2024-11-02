using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlowerGenerator : MonoBehaviour
{   
    [SerializeField]public int spawnInterval = 5;
    private BoxCollider2D area;  
    private List<GameObject> BigFlowerList = new List<GameObject>();		

    void Start()
    {
        StartCoroutine("Spawn", spawnInterval);
        BigFlowerList.Clear();
    }

    IEnumerator Spawn(float delayTime)

    {
        GameObject BigFlower = GameObject.Find("Flower");
        GameObject [] SpawningPlatforms = GameObject.FindGameObjectsWithTag("FlowerSpawnable_Platform");
        GameObject SpawningPlatform = SpawningPlatforms[Random.Range(0, SpawningPlatforms.Length)];
        print(SpawningPlatform);

        area = SpawningPlatform.GetComponent<BoxCollider2D>();
        print(area);

        Vector3 spawnPos = GetRandomPosition();
        print(spawnPos);
        GameObject instance = Instantiate(BigFlower, spawnPos, Quaternion.identity);
        BigFlowerList.Add(instance);
        yield return new WaitForSeconds(delayTime);

        StartCoroutine("Spawn", spawnInterval);
    }

    

    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = area.transform.position; 
        Vector2 size = area.size;                  
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);

        float posY = basePosition.y + 0.5f;

        Vector2 spawnPos = new Vector2(posX, posY);

        return spawnPos;
    }

}
