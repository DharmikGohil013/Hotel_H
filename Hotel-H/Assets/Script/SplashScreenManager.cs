using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro for UI text

public class SplashScreenManager : MonoBehaviour
{
    public TextMeshProUGUI loadingText; // Reference to the Loading text UI

    void Start()
    {
        StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        // Wait 2 seconds for logo display
        yield return new WaitForSeconds(2f);

        // Show "Loading..." text
        loadingText.gameObject.SetActive(true);

        // Wait a random time between 1 to 5 seconds
        float randomWaitTime = Random.Range(1f, 5f);
        yield return new WaitForSeconds(randomWaitTime);

        // Load the main game scene
        SceneManager.LoadScene(1); // Replace with your actual scene name
    }
}
