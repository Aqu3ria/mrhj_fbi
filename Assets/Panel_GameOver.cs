using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Panel_GameOver : MonoBehaviour {
    public Text Text_GameResult; 
    private void Awake()
    {
        transform.gameObject.SetActive(false); 
    }

    public void Show() {
        //int score = FindObjectOfType<ScoreText>().GetScore(); 
        transform.gameObject.SetActive(true);
        //Text_GameResult.text = "GameSet\nScore : " + score.ToString(); 
    }

    public void OnClick_Retry() 
    {
        SceneManager.LoadScene("GameScene");
    }

}