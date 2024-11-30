using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private EnemyGenerator enemyGenerator;

    private void Update()
    {
        waveNumberText.text = "" + enemyGenerator.wave;    
    }
}
