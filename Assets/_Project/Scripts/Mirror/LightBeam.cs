using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightBeam : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 50f;
    public LayerMask interactLayer;
    public LayerMask winLayer;

    private const float RayOffset = 0.01f;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
    }

    // Added bounceCount parameter to track depth
    public void Shoot(Vector3 startPos, Vector3 direction, int bounceCount)
    {

        Physics.SyncTransforms();

        if (bounceCount > BeamManager.Instance.maxBounces)
        {
            return;
        }

        _lineRenderer.enabled = true;
        Vector3 rayStart = startPos + (direction.normalized * RayOffset);
        _lineRenderer.SetPosition(0, startPos);

        // Fire ONE raycast that stops at the very first object it hits
        if (Physics.Raycast(rayStart, direction, out RaycastHit hit, maxDistance))
        {
            _lineRenderer.SetPosition(1, hit.point);

            // Convert the hit object's layer into a bitmask to compare against your LayerMasks
            int hitLayer = 1 << hit.collider.gameObject.layer;

            // Check if the object we hit is part of the interactLayer
            if ((hitLayer & interactLayer.value) != 0)
            {
                if (hit.collider.TryGetComponent(out MirrorNode mirror))
                {
                    mirror.Reflect(hit.point, direction, hit.normal, bounceCount);
                }
            }
            // If not interactable, check if it is part of the winLayer
            else if ((hitLayer & winLayer.value) != 0)
            {
                BeamManager.Instance.FinishPuzzle();
            }
        }
        else
        {
            // The ray hit nothing at all
            _lineRenderer.SetPosition(1, startPos + direction * maxDistance);
        }
    }

    public void TurnOff()
    {
        if (_lineRenderer != null && _lineRenderer.enabled)
            _lineRenderer.enabled = false;
    }
}