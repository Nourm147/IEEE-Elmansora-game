using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static event Action<bool> OnTimeShifted;

    [Header("Time Settings")]
    public bool isPresent = true;
    public float cooldownDuration = 3f;
    private AudioSource As;

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
        UpdateUI();
        if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);
        foreach (Material mat in baseMaterials)
        {
            mat.SetFloat("_Age_Factor", 0.75f);
        }
        As = GetComponent<AudioSource>();
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
        instructionText.text = "Shifting...";
        As.Play();
        float timer = 0f;
        Color c = fadeImage.color;
        while (timer < fadeSpeed)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, timer / fadeSpeed);
            fadeImage.color = c;
            yield return null;
        }

        isPresent = !isPresent;
        OnTimeShifted?.Invoke(isPresent);

        if (isPresent)
        {
            onPresentShift.Invoke();
            //  Shader.SetGlobalFloat("Age_Factor", 0.5f);
            foreach (Material mat in baseMaterials)
            {
                mat.SetFloat("_Age_Factor", 0.75f);
            }
        }
        else
        {
            onPastShift.Invoke();
            //Shader.SetGlobalFloat("Age_Factor", 0f);
            foreach (Material mat in baseMaterials)
            {
                mat.SetFloat("_Age_Factor", 0f);
            }
        }
        yearText.text = isPresent ? "Year: 2026" : "Year: 1926";

        yield return new WaitForSeconds(0.2f);

        timer = 0f;
        while (timer < fadeSpeed)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, timer / fadeSpeed);
            fadeImage.color = c;
            yield return null;
        }
        As.Stop();

        float cooldownTimer = cooldownDuration;
        while (cooldownTimer > 0)
        {
            instructionText.text = $"Energy Recharging... {cooldownTimer:F1}s";
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isShifting = false;
        instructionText.text = "Press [T] to Shift Time";
    }

    private void UpdateUI()
    {
        yearText.text = isPresent ? "Year: 2026" : "Year: 1926";
        instructionText.text = "Press [T] to Shift Time";
    }
   
}
