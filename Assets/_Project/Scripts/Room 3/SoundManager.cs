using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Interaction SFX")]
    public AudioClip leverMoveClip;
    public AudioClip puzzleSolvedClip;
    public AudioClip grabObjectClip;
    public AudioClip placeObjectClip;
    public AudioClip groundRiseClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayLeverMove()
    {
        PlaySFX(leverMoveClip);
    }

    public void PlayPuzzleSolved()
    {
        PlaySFX(puzzleSolvedClip);
    }

    public void PlayGrabObject()
    {
        PlaySFX(grabObjectClip);
    }

    public void PlayPlaceObject()
    {
        PlaySFX(placeObjectClip);
    }

    public void PlayGroundRise()
    {
        PlaySFX(groundRiseClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}