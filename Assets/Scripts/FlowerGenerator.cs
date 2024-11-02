using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FlowerGenerator : MonoBehaviour
{
    
    [SerializeField]public int spawnInterval = 5;
    int count = 3;
    private BoxCollider2D area;  
    private List<GameObject> BigFlowerList = new List<GameObject>();		

    void Start()
    {
        area = GetComponent<BoxCollider2D>();
        StartCoroutine("Spawn", spawnInterval);
    }


    
    private IEnumerator Spawn(float delayTime)
    {
        GameObject BigFlower = GameObject.Find("Flower");

        for (int i = 0; i < count; i++) 
        {
            Vector3 spawnPos = GetRandomPosition(); 

            yield return new WaitForSeconds(delayTime);

            GameObject instance = Instantiate(BigFlower, spawnPos, Quaternion.identity);
            BigFlowerList.Add(instance);
        }
        area.enabled = false;
        yield return new WaitForSeconds(delayTime);

        for (int i = 0; i < count; i++)
            Destroy(BigFlowerList[i].gameObject);

        BigFlowerList.Clear();
        area.enabled = true;
        StartCoroutine("Spawn", spawnInterval);
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = transform.position; 
        Vector2 size = area.size;                  
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);

        float posY = basePosition.y;

        Vector2 spawnPos = new Vector2(posX, posY);

        return spawnPos;
    }

}
