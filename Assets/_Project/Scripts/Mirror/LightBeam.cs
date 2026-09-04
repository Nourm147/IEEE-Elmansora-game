using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightBeam : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 50f;
    public LayerMask interactLayer;

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
        // Safety check to prevent infinite reflection loops
        if (bounceCount > BeamManager.Instance.maxBounces)
        {
            return;
        }

        _lineRenderer.enabled = true;
        Vector3 rayStart = startPos + (direction.normalized * RayOffset);

        _lineRenderer.SetPosition(0, startPos);

        if (Physics.Raycast(rayStart, direction, out RaycastHit hit, maxDistance, interactLayer))
        {
            _lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.TryGetComponent(out MirrorNode mirror))
            {
                // Trigger the next bounce immediately
                mirror.Reflect(hit.point, direction, hit.normal, bounceCount);
            }
        }
        else
        {
            _lineRenderer.SetPosition(1, startPos + direction * maxDistance);
        }
    }

    public void TurnOff()
    {
        if (_lineRenderer != null && _lineRenderer.enabled)
            _lineRenderer.enabled = false;
    }
}