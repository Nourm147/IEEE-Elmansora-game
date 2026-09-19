using UnityEngine;


[RequireComponent(typeof(Collider), typeof(AudioSource))]
public class UIInteractableBase : MonoBehaviour
{
    [Header("Visual Feedback")]
    public MeshRenderer[] targetRenderers;
    public Color hoverColor = new Color(1f, 0.8f, 0.5f);
    public Color interactColor = Color.red;

    private Color[] originalColors;

    [Header("Audio Feedback")]
    public AudioClip hoverSound;
    public AudioClip interactSound;
    protected AudioSource audioSource;

    protected bool isHovered = false;
    protected bool isInteracting = false;

    protected virtual void Awake()
    {
        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            targetRenderers = GetComponentsInChildren<MeshRenderer>();
        }

        if (targetRenderers != null && targetRenderers.Length > 0)
        {
            originalColors = new Color[targetRenderers.Length];
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    originalColors[i] = targetRenderers[i].material.color;
            }
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnMouseEnter()
    {
        isHovered = true;
        if (!isInteracting && targetRenderers != null)
        {
            foreach (MeshRenderer MR in targetRenderers)
            {
                if (MR != null) MR.material.color = hoverColor;
            }
        }
        PlaySound(hoverSound);
    }

    void OnMouseExit()
    {
        isHovered = false;
        if (!isInteracting && targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material.color = originalColors[i];
            }
        }
    }

    void OnMouseDown()
    {
        isInteracting = true;
        if (targetRenderers != null)
        {
            foreach (MeshRenderer MR in targetRenderers)
            {
                if (MR != null) MR.material.color = interactColor;
            }
        }
        PlaySound(interactSound);
        InteractStart();
    }

    void OnMouseUp()
    {
        isInteracting = false;
        if (targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                    targetRenderers[i].material.color = isHovered ? hoverColor : originalColors[i];
            }
        }
        InteractEnd();
    }

    protected virtual void InteractStart() { }
    protected virtual void InteractEnd() { }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null) audioSource.PlayOneShot(clip);
    }
}
