using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public MoneyManager moneyManager; // Assuming this exists; remove if unused
    public TextMeshProUGUI levelText;
    public Slider moneySlider;
    public CustomerManager customerManager;

    private int level = 1;
    private int nextLevelTarget = 500;
    private const int maxLevel = 3;
    private const string MONEY_KEY = "Money"; // Fixed key

    void Start()
    {
        //ResetProgress();
        level = PlayerPrefs.GetInt("Level", 1);
        if (level > maxLevel) level = maxLevel;
        nextLevelTarget = level * 500;
        UpdateUI();
        customerManager.UpdateRoomAvailability();
    }

    void Update()
    {
        int currentMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
        if (currentMoney >= nextLevelTarget && level < maxLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    void LevelUp()
    {
        int currentMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
        int remainingMoney = currentMoney - nextLevelTarget;
        PlayerPrefs.SetInt(MONEY_KEY, remainingMoney);
        level++;
        nextLevelTarget = level * 500;
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
        customerManager.UpdateRoomAvailability();
        Debug.Log($"Leveled up to Level {level}! Next target: {nextLevelTarget}");
    }

    void UpdateUI()
    {
        int currentMoney = PlayerPrefs.GetInt(MONEY_KEY, 0);
        levelText.text = level.ToString();
        moneySlider.maxValue = nextLevelTarget;
        moneySlider.value = currentMoney;
    }
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.Save();
        level = 1;
        nextLevelTarget = level * 500;
        UpdateUI();
        customerManager.UpdateRoomAvailability();
    }

}