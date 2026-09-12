using UnityEngine;

public class InteractOutline : MonoBehaviour
{
    public float interactDistance = 5f;
    uint defaultLayer = 1;
    uint outlineLayer = 2;
    private Renderer currentRenderer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

                if (hitRenderer != null && hitRenderer != currentRenderer)
                {
                    ResetOutline();
                    currentRenderer = hitRenderer;
                    currentRenderer.renderingLayerMask = outlineLayer;
                }
            }
            else
            {
                ResetOutline();
            }
        }
        else
        {
            ResetOutline();
        }
    }

    void ResetOutline()
    {
        if (currentRenderer != null)
        {
            currentRenderer.renderingLayerMask = defaultLayer;
            currentRenderer = null;
        }
    }
}
