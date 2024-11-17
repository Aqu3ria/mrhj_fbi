using TMPro;
using UnityEngine;

public class PlayerPercent : MonoBehaviour
{
    [SerializeField] private int currentPercent;
    [SerializeField] public TextMeshProUGUI percentText;

    float knockbackDistance = 0.5f;   
    [SerializeField] float knockbackDuration = 0.1f; 

    private bool isKnockback = false;  
    private Vector2 knockbackDirection;
    private float knockbackEndTime;

    private void Start()
    {
        currentPercent = 0;
        UpdatePercentUI();
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        currentPercent += damage;
        currentPercent = Mathf.Max(currentPercent, 0);
        UpdatePercentUI();

        knockbackDirection = direction.normalized;
        isKnockback = true;
        knockbackEndTime = Time.time + knockbackDuration;   
        //Debug.Log("TakeDamage called with damage: " + damage + ", knockbackDirection: " + knockbackDirection);

    }

    private void Update()
    {
        if (isKnockback)
        {
            if (Time.time < knockbackEndTime)
            {
                knockbackDistance = currentPercent / 30 + 0.5f;
                transform.Translate(knockbackDirection * (knockbackDistance / knockbackDuration) * Time.deltaTime);
            }
            else
            {
                isKnockback = false;
            }
        }
    }

    private void UpdatePercentUI()
    {
        percentText.text = $"{currentPercent}%";
    }
}