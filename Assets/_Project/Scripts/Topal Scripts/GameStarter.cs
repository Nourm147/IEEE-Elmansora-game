using EasyPeasyFirstPersonController;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController playerController;

    void Start()
    {
        // ÇáÊÃßÏ Åä ÇááÚÈÉ ÔÛÇáÉ ãÔ ãÚãæáåÇ Pause
        Time.timeScale = 1f;

        // ÅÎİÇÁ ÇáãÇæÓ æŞİá ÍÑßÊå Ìæå ÇááÚÈÉ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ÇÓÊÑÌÇÚ ÇáÅÚÏÇÏÇÊ Çááí ÇááÇÚÈ ÇÎÊÇÑåÇ ãä ÇáÜ Main Menu
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);

        // ÊØÈíŞ ÇáÕæÊ
        AudioListener.volume = savedVolume;

        // ÊØÈíŞ ÍÓÇÓíÉ ÇáßÇãíÑÇ
        if (playerController != null)
        {
            playerController.mouseSensitivity = savedSensitivity;
        }
    }
}
