using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CustomSlider : UIInteractableBase
{
    [Header("Slider Limits")]
    public Transform minPoint;
    public Transform maxPoint;

    [Header("Output")]
    public UnityEvent<float> OnValueChanged;

    [Header("Sliding Audio")]
    public AudioClip slidingSound;

    private Camera mainCam;
    private float currentValue;

    protected override void Awake()
    {
        base.Awake(); 
        mainCam = Camera.main;
    }

    void OnMouseDrag()
    {
        if (minPoint == null || maxPoint == null) return;

        Plane dragPlane = new Plane(mainCam.transform.forward * -1, transform.position);
        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(camRay, out float distance))
        {
            Vector3 hitPoint = camRay.GetPoint(distance);
            Vector3 lineDir = (maxPoint.position - minPoint.position).normalized;
            Vector3 toHit = hitPoint - minPoint.position;

            float dot = Vector3.Dot(toHit, lineDir);
            float sliderLength = Vector3.Distance(minPoint.position, maxPoint.position);

            dot = Mathf.Clamp(dot, 0f, sliderLength);
            Vector3 newPos = minPoint.position + lineDir * dot;

            if (Vector3.Distance(transform.position, newPos) > 0.001f)
            {
                transform.position = newPos;

                if (slidingSound != null && !audioSource.isPlaying)
                {
                    audioSource.clip = slidingSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying && audioSource.clip == slidingSound)
                {
                    audioSource.Pause(); 
                }
            }

            currentValue = dot / sliderLength;
            OnValueChanged.Invoke(currentValue);
        }
    }

    protected override void InteractEnd()
    {
        if (audioSource.isPlaying && audioSource.clip == slidingSound)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }
    public void SetSliderValue(float normalizedValue)
    {
        if (minPoint == null || maxPoint == null) return;

        currentValue = Mathf.Clamp01(normalizedValue);
        Vector3 lineDir = (maxPoint.position - minPoint.position).normalized;
        float sliderLength = Vector3.Distance(minPoint.position, maxPoint.position);

        transform.position = minPoint.position + lineDir * (currentValue * sliderLength);
    }
    void OnDrawGizmos()
    {
        if (minPoint != null && maxPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(minPoint.position, maxPoint.position);
            Gizmos.DrawWireSphere(minPoint.position, 0.05f);
            Gizmos.DrawWireSphere(maxPoint.position, 0.05f);
        }
    }
}
