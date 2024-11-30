using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    private int score;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        Enemy.onEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(object sender, System.EventArgs e)
    {
        score += 1;
        Destroy(sender as GameObject);
    }

    public int GetScore()
    {
        return score;
    }
}
