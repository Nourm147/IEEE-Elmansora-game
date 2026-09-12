using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource uiAudioSource; 

    [Header("Audio Clips")]
    public AudioClip hoverClip;
    public AudioClip clickClip; 

    public void PlayHoverSound()
    {
        if (hoverClip != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(hoverClip);
        }
    }

    public void PlayClickSound()
    {
        if (clickClip != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(clickClip);
        }
    }
}
