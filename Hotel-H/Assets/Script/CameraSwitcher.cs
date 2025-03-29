using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array to store cameras
    private int currentCameraIndex = 0;

    public Button nextButton; // UI Button

    void Start()
    {
        // Enable only the first camera
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }

        nextButton.onClick.AddListener(SwitchCamera); // Assign button event
    }

    void SwitchCamera()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false); // Disable current camera
        currentCameraIndex++;

        if (currentCameraIndex < cameras.Length)
        {
            cameras[currentCameraIndex].gameObject.SetActive(true); // Enable next camera
        }
        else
        {
            SceneManager.LoadScene(2); // Load scene 3 when last camera is reached
        }
    }
}
