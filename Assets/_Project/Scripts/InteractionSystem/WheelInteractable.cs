using UnityEngine;
using UnityEngine.Events;

public class WheelInteractable : BaseInteractable
{
    [Header("Wheel Settings")]
    [Tooltip("The local axis the wheel rotates around (e.g., 0,1,0 for Y axis, 0,0,1 for Z)")]
    public Vector3 rotationAxis = Vector3.forward;
    public float rotationSpeed = 2f;

    [Tooltip("Invert the mouse drag direction")]
    public bool invertDirection = false;

    [Header("Events")]
    [Tooltip("Passes the total accumulated rotation angle so you can wire it to other objects.")]
    public UnityEvent<float> OnValueChanged;

    private float _currentAngle = 0f;

    public override void OnSelectEnter(IInteractor interactor)
    {
        base.OnSelectEnter(interactor);
    }

    public override void OnSelectExit(IInteractor interactor)
    {
        base.OnSelectExit(interactor);
    }

    private void Update()
    {
        // IsSelected and InputConfig are inherited from your BaseInteractable
        if (!IsSelected || InputConfig == null) return;

        if (InputConfig.lookDeltaAction != null)
        {
            Vector2 lookDelta = InputConfig.lookDeltaAction.action.ReadValue<Vector2>();

            // We'll use horizontal mouse movement to spin the wheel
            float deltaAmount = lookDelta.x * rotationSpeed;
            if (invertDirection) deltaAmount = -deltaAmount;

            if (Mathf.Abs(deltaAmount) > 0.01f)
            {
                RotateWheel(deltaAmount);
            }
        }
    }

    private void RotateWheel(float amount)
    {
        // Apply rotation physically to the wheel mesh
        transform.Rotate(rotationAxis, amount, Space.Self);

        // Track the angle and fire the UnityEvent for the inspector
        _currentAngle += amount;
        OnValueChanged?.Invoke(_currentAngle);
    }
}