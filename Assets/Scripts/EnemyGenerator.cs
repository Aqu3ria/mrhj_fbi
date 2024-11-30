using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
    // public static EnemyGenerator Instance { get; private set;}
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> enemyPositions;
    public int wave = 1;
    [SerializeField] private BoxCollider2D area;
    private List<GameObject> enemyList = new List<GameObject>();

    public int lastEnemyCount = 0;
    public int waveDuration = 0;
    public int enemyCount = 0;

    void Awake()
    {
        StartCoroutine(Wave());
        enemyList.Clear();

        // Instance = this;
    }

    IEnumerator Wave()
    {

        while (true)
        {

            if (wave == 1)
            {
                enemyCount = Random.Range(4, 7);
                lastEnemyCount = enemyCount;
            }
            else
            {
                enemyCount = Random.Range(lastEnemyCount + 2, lastEnemyCount + 6);
                lastEnemyCount = enemyCount;
            }

            waveDuration = enemyCount * 10;

            for (int i = 0; i < enemyCount; i++)
            {
                yield return new WaitForSeconds(Random.Range(1, 4));
                StartCoroutine(Spawn());
            }


            wave++;

            yield return new WaitForSeconds(waveDuration);
        }
    }

    IEnumerator Spawn()
    {
        Vector3 spawnPos = enemyPositions[Random.Range(0, enemyPositions.Count)].position;
        GameObject instance = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemyList.Add(instance);
        yield return null;
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 basePosition = area.transform.position;
        Vector2 size = area.size;
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + 3f;
        return new Vector2(posX, posY);
    }
}

