using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Panel_GameOver : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI Text_GameResult;
    [SerializeField] private TextMeshProUGUI Text_GameWave; 
    [SerializeField] private Button button_Retry;
    private void Awake()
    {
        transform.gameObject.SetActive(false); 
    }

    private void Start()
    {
        button_Retry.onClick.AddListener(() => OnClick_Retry());
    }

    public void Show() {
        //int score = FindObjectOfType<ScoreText>().GetScore(); 
        transform.gameObject.SetActive(true);
        Text_GameResult.text = "Score: " + GameManager.Instance.GetScore(); 
        Text_GameWave.text = "Waves: " + EnemyGenerator.Instance.GetWaveNumber();
    }

    public void OnClick_Retry() 
    {
        // GameManager.Instance.Restart();
        // Hide();
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}