using EasyPeasyFirstPersonController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Put True here if this scene is main menu")]
    public bool isMainMenu = false;

    [Header("In-Game Pause Menu")]
    [Tooltip("Keep it empty if not)")]
    public GameObject pauseCanvas;

    [Tooltip("Remove the player movement script (local-only for multiplayer) to disable it while the menu is open.")]
    public MonoBehaviour localPlayerController;

    public static bool IsGamePaused = false;
    private bool isPaused = false;

    [Header("3D Sliders (Main Menu)")]
    public CustomSlider volume3DSlider;
    public CustomSlider sensitivity3DSlider;

    [Header("2D Sliders (In-Game Canvas)")]
    public Slider volume2DSlider;
    public Slider sensitivity2DSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);

        ApplyVolume(savedVolume);
        ApplySensitivity(savedSensitivity);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsGamePaused = false;

        if (isMainMenu)
        {
            if (volume3DSlider != null) volume3DSlider.SetSliderValue(savedVolume);
            if (sensitivity3DSlider != null) sensitivity3DSlider.SetSliderValue(savedSensitivity);
        }
        else
        {
            if (pauseCanvas != null) pauseCanvas.SetActive(false);

            if (volume2DSlider != null)
            {
                volume2DSlider.value = savedVolume;
                volume2DSlider.onValueChanged.AddListener(SetVolume);
            }
            if (sensitivity2DSlider != null)
            {
                sensitivity2DSlider.value = savedSensitivity;
                sensitivity2DSlider.onValueChanged.AddListener(SetSensitivity);
            }
        }
    }

    void Update()
    {
        if (!isMainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }


    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        IsGamePaused = isPaused; 

        if (pauseCanvas != null) pauseCanvas.SetActive(isPaused);

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (localPlayerController != null)
                localPlayerController.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (localPlayerController != null)
                localPlayerController.enabled = true;
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(1); 
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.Save();
        IsGamePaused = false; 
        SceneManager.LoadScene(0); 
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("Game Quit!");
    }


    public void SetVolume(float val)
    {
        ApplyVolume(val);
        PlayerPrefs.SetFloat("GameVolume", val);
    }

    private void ApplyVolume(float val)
    {
        AudioListener.volume = val;
    }

    public void SetSensitivity(float val)
    {
        ApplySensitivity(val);
        PlayerPrefs.SetFloat("MouseSensitivity", val);
    }

    private void ApplySensitivity(float val)
    {
        if (localPlayerController != null)
        {
            var player = localPlayerController as FirstPersonController;
            if (player != null)
            {
              
                player.mouseSensitivity = Mathf.Lerp(0.5f, 5f, val);
            }
        }
    }
}
