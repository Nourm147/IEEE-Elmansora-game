using UnityEngine;

public class RotationSync : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The local axis to rotate around (e.g., 0, 1, 0 for Y axis).")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Multiplier for the incoming angle (use -1 to reverse rotation direction).")]
    public float angleMultiplier = 1f;

    private Quaternion _initialRotation;

    private void Awake()
    {
        // Store the starting rotation so we don't snap to 0,0,0
        _initialRotation = transform.localRotation;
    }

    // Called directly by WheelInteractable's UnityEvent
    public void SetRotation(float angle)
    {
        float finalAngle = angle * angleMultiplier;
        transform.localRotation = _initialRotation * Quaternion.AngleAxis(finalAngle, rotationAxis);
    }
}