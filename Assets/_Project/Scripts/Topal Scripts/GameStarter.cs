using EasyPeasyFirstPersonController;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController playerController;

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);

        AudioListener.volume = savedVolume;

        if (playerController != null)
        {
            playerController.mouseSensitivity = savedSensitivity;
        }
    }
}
