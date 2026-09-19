using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightIntensityController : MonoBehaviour
{
    private Light puzzleLight;
    private Coroutine activeFadeRoutine;
    [SerializeField] private float targetIntensity;
    [SerializeField] private float duration = 0.7f;

    private void Awake()
    {
        // Automatically grab the Light component attached to this GameObject
        puzzleLight = GetComponent<Light>();
    }

    // Function 1: Increase (or decrease) to a specific target value over time
    public void FadeToTarget()
    {
        if (activeFadeRoutine != null)
        {
            StopCoroutine(activeFadeRoutine);
        }
        activeFadeRoutine = StartCoroutine(AnimateLight(targetIntensity, duration));
    }

    // Function 2: Fade completely to 0 over time
    public void FadeToZero()
    {
        if (activeFadeRoutine != null)
        {
            StopCoroutine(activeFadeRoutine);
        }
        activeFadeRoutine = StartCoroutine(AnimateLight(0f, duration));
    }

    private IEnumerator AnimateLight(float targetValue, float duration)
    {
        // Prevent division by zero if duration is set to 0
        if (duration <= 0f)
        {
            puzzleLight.intensity = targetValue;
            yield break;
        }

        float startIntensity = puzzleLight.intensity;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Smoothly interpolate the intensity
            puzzleLight.intensity = Mathf.Lerp(startIntensity, targetValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the exact target value is set at the end of the transition
        puzzleLight.intensity = targetValue;
    }
}