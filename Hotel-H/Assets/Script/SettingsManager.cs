using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel; // 🎛️ Reference to Settings Panel
    public Slider soundSlider; // 🎚️ Sound Slider
    private AudioSource bgMusic; // 🎵 Background Music Source

    void Start()
    {
        // 🔍 Find the background music object by name
        bgMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        // 🎚️ Load saved volume or default to 1
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        soundSlider.value = savedVolume;
        bgMusic.volume = savedVolume;

        settingsPanel.SetActive(false); // ❌ Ensure settings panel starts hidden
        bgMusic.Play(); // ▶️ Start playing music
    }

    // 🎛️ Open Settings Panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // ❌ Close Settings Panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // 🎵 Adjust Music Volume from Slider
    public void AdjustVolume(float volume)
    {
        bgMusic.volume = volume; // Change volume
        PlayerPrefs.SetFloat("Volume", volume); // Save setting
        PlayerPrefs.Save();
    }

    // 🔴 Exit Game
    public void ExitGame()
    {
        Debug.Log("Game Exiting..."); // Show log in Editor
        Application.Quit(); // Quit the game
    }
}
