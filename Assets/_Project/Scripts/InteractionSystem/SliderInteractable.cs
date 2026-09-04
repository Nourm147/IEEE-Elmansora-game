using UnityEngine;
using UnityEngine.Events;

public class SliderInteractable : BaseInteractable
{
    [Header("Slider Settings")]
    [Tooltip("The local axis the slider moves along (e.g., 1, 0, 0 for X).")]
    public Vector3 slideAxis = Vector3.right;

    [Tooltip("Minimum distance from the starting position.")]
    public float minDistance = 0f;

    [Tooltip("Maximum distance from the starting position.")]
    public float maxDistance = 1f;

    public float slideSpeed = 0.01f;

    [Tooltip("Check this to use vertical mouse movement instead of horizontal.")]
    public bool useMouseYForDrag = true;
    public bool invertDirection = false;

    [Header("Events")]
    [Tooltip("Outputs a normalized value (0.0 to 1.0) based on how far the slider is pulled.")]
    public UnityEvent<float> OnValueChangedNormalized;

    private float _currentDistance = 0f;
    private Vector3 _startLocalPos;

    private void Awake()
    {
        _startLocalPos = transform.localPosition;
        slideAxis.Normalize();
    }

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
        if (!IsSelected || InputConfig == null || InputConfig.lookDeltaAction == null) return;

        Vector2 lookDelta = InputConfig.lookDeltaAction.action.ReadValue<Vector2>();

        // Choose X or Y mouse movement to drive the slider based on inspector setting
        float deltaAmount = useMouseYForDrag ? lookDelta.y : lookDelta.x;
        deltaAmount *= slideSpeed;

        if (invertDirection) deltaAmount = -deltaAmount;

        if (Mathf.Abs(deltaAmount) > 0.001f)
        {
            MoveSlider(deltaAmount);
        }
    }

    private void MoveSlider(float amount)
    {
        // Add the movement and clamp it within bounds
        _currentDistance += amount;
        _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);

        // Apply position physically
        transform.localPosition = _startLocalPos + (slideAxis * _currentDistance);

        // Calculate normalized value (0 to 1) and fire the UnityEvent
        float normalizedValue = 0f;
        if (maxDistance > minDistance)
        {
            normalizedValue = (_currentDistance - minDistance) / (maxDistance - minDistance);
        }

        OnValueChangedNormalized?.Invoke(normalizedValue);
    }
}