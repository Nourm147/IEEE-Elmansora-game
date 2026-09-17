using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    public static event Action<bool> OnTimeShifted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("Time Settings")]
    public bool isPresent = true;
    public float cooldownDuration = 3f;
    private AudioSource _audioSource;

    [Header("UI Elements")]
    public Image fadeImage;
    public float fadeSpeed = 0.5f;
    public TextMeshProUGUI yearText;
    public TextMeshProUGUI instructionText;

    [Header("Shader settings")]
    public Material[] baseMaterials;


    private bool isShifting = false;

    [Header("Events")]
    public UnityEvent onPresentShift;
    public UnityEvent onPastShift;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        UpdateUI();
        
        if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);
        
        if (baseMaterials != null)
        {
            foreach (Material mat in baseMaterials)
            {
                if (mat != null) mat.SetFloat("_Age_Factor", 0.75f);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isShifting)
        {
            StartCoroutine(TimeShiftRoutine());
        }
    }

    private IEnumerator TimeShiftRoutine()
    {
        isShifting = true;
        if (instructionText != null) instructionText.text = "Shifting...";
        if (_audioSource != null) _audioSource.Play();
        
        float fadeTimer = 0f;
        
        if (fadeImage != null)
        {
            Color fadeColor = fadeImage.color;
            while (fadeTimer < fadeSpeed)
            {
                fadeTimer += Time.deltaTime;
                fadeColor.a = Mathf.Lerp(0, 1, fadeTimer / fadeSpeed);
                fadeImage.color = fadeColor;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(fadeSpeed);
        }

        isPresent = !isPresent;
        OnTimeShifted?.Invoke(isPresent);

        if (isPresent)
        {
            onPresentShift?.Invoke();
            if (baseMaterials != null)
            {
                foreach (Material mat in baseMaterials)
                {
                    if (mat != null) mat.SetFloat("_Age_Factor", 0.75f);
                }
            }
        }
        else
        {
            onPastShift?.Invoke();
            if (baseMaterials != null)
            {
                foreach (Material mat in baseMaterials)
                {
                    if (mat != null) mat.SetFloat("_Age_Factor", 0f);
                }
            }
        }
        
        if (yearText != null) yearText.text = isPresent ? "Year: 2026" : "Year: 1926";

        yield return new WaitForSeconds(0.2f);

        fadeTimer = 0f;
        if (fadeImage != null)
        {
            Color fadeColor = fadeImage.color;
            while (fadeTimer < fadeSpeed)
            {
                fadeTimer += Time.deltaTime;
                fadeColor.a = Mathf.Lerp(1, 0, fadeTimer / fadeSpeed);
                fadeImage.color = fadeColor;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(fadeSpeed);
        }
        
        if (_audioSource != null) _audioSource.Stop();

        float cooldownTimer = cooldownDuration;
        while (cooldownTimer > 0)
        {
            if (instructionText != null) instructionText.text = $"Energy Recharging... {cooldownTimer:F1}s";
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isShifting = false;
        if (instructionText != null) instructionText.text = "Press [T] to Shift Time";
    }

    private void UpdateUI()
    {
        if (yearText != null) yearText.text = isPresent ? "Year: 2026" : "Year: 1926";
        if (instructionText != null) instructionText.text = "Press [T] to Shift Time";
    }
   
}
