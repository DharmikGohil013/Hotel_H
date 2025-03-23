using UnityEngine;
using TMPro; // Import TextMeshPro for UI text
using UnityEngine.UI; // For Slider UI

public class LevelProgress : MonoBehaviour
{
    public MoneyManager moneyManager; // Reference to MoneyManager
    public TextMeshProUGUI levelText; // UI text for level display
    public Slider moneySlider; // Reference to UI slider

    private int level = 1; // Default level
    private int nextLevelTarget = 500; // Initial target for next level

    void Start()
    {
        // Load saved level
        level = PlayerPrefs.GetInt("Level", 1);
        nextLevelTarget = level * 500;

        UpdateUI();
    }

    void Update()
    {
        int currentMoney = PlayerPrefs.GetInt("", 0); // Get money from PlayerPrefs

        if (currentMoney >= nextLevelTarget)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        int remainingMoney = PlayerPrefs.GetInt("", 0) - nextLevelTarget; // Keep leftover money
        PlayerPrefs.SetInt("", remainingMoney); // Update remaining money in PlayerPrefs
        level++; // Increase level
        nextLevelTarget = level * 500; // Update next level target

        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save(); // Save changes
    }

    void UpdateUI()
    {
        int currentMoney = PlayerPrefs.GetInt("", 0);
        levelText.text = level.ToString(); // Update level text
        moneySlider.maxValue = nextLevelTarget; // Update slider max value
        moneySlider.value = currentMoney; // Update slider progress
    }
}
