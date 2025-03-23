using UnityEngine;
using TMPro; // Import TextMeshPro for UI text

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // UI text reference
    private int money = 0; // Money counter

    void Start()
    {
        // Load saved money
        money = PlayerPrefs.GetInt("", 0);
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        PlayerPrefs.SetInt("", money); // Save money permanently
        PlayerPrefs.Save(); // Ensure it's saved
        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        moneyText.text = money.ToString();
    }
}
