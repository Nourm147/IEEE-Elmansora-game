using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Settings UI")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        // ≈ŸÂ«— «·„«Ê” Ê Õ—Ì—Â ›Ì «·ﬁ«∆„…
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //  ›⁄Ì· „‰ÌÊ «·»œ«Ì… Êﬁ›· «·≈⁄œ«œ« 
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // «” —Ã«⁄ «·ﬁÌ„ «·„Õ›ÊŸ… „”»ﬁ«
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);

        //  Ÿ»Ìÿ «·‹ Sliders Ê—»ÿÂ« »«· €ÌÌ—« 
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    public void PlayGame()
    {
        // Õ›Ÿ «·≈⁄œ«œ«  »‘ﬂ· ‰Â«∆Ì ﬁ»· «·«‰ ﬁ«·
        PlayerPrefs.Save();

        //  Õ„Ì· „‘Âœ «··⁄»… ( √ﬂœ ≈‰ «·«‰œﬂ” » «⁄Â 1 ›Ì «·‹ Build Settings)
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float val)
    {
        PlayerPrefs.SetFloat("GameVolume", val);
    }

    public void SetSensitivity(float val)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", val);
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
