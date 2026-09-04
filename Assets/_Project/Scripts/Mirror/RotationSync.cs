using UnityEngine;

public class RotationSync : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("The local axis to rotate around (e.g., 0, 1, 0 for Y axis).")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Multiplier for the incoming angle (use -1 to reverse rotation direction).")]
    public float angleMultiplier = 1f;

    [Header("Constraints")]
    public bool enableClamp = true;
    [Tooltip("The minimum allowed angle relative to the starting position.")]
    public float minAngle = -45f;
    [Tooltip("The maximum allowed angle relative to the starting position.")]
    public float maxAngle = 45f;

    private Quaternion _initialRotation;

    private void Awake()
    {
        // Store the starting rotation so we don't snap to 0,0,0
        _initialRotation = transform.localRotation;
    }

    // This method is called directly by the WheelInteractable's UnityEvent
    public void SetRotation(float angle)
    {
        // Calculate the intended angle
        float finalAngle = angle * angleMultiplier;

        // Apply clamping if enabled
        if (enableClamp)
        {
            finalAngle = Mathf.Clamp(finalAngle, minAngle, maxAngle);
        }

        // Combine the starting rotation with the new clamped angle
        transform.localRotation = _initialRotation * Quaternion.AngleAxis(finalAngle, rotationAxis);
    }
}