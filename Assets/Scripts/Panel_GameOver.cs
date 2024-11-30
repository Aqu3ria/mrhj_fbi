using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Panel_GameOver : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI Text_GameResult; 
    private void Awake()
    {
        transform.gameObject.SetActive(false); 
    }

    public void Show() {
        //int score = FindObjectOfType<ScoreText>().GetScore(); 
        transform.gameObject.SetActive(true);
        Text_GameResult.text = "Score: " + GameManager.Instance.GetScore(); 
    }

    public void OnClick_Retry() 
    {
        SceneManager.LoadScene("GameScene");
    }

}