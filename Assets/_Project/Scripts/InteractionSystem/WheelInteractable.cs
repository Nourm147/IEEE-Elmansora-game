using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class WheelInteractable : BaseInteractable
{
    [Header("Wheel Settings")]
    [Tooltip("The local axis the wheel rotates around (e.g., 0,0,1 for Z).")]
    public Vector3 rotationAxis = Vector3.forward;
    
    [Tooltip("How fast the virtual hand moves with your mouse. Adjust based on mouse sensitivity.")]
    public float trackingSpeed = 0.01f;

    [Header("Constraints")]
    public bool enableClamp = false;
    [Tooltip("The minimum allowed angle relative to the starting position.")]
    public float minAngle = -90f;
    [Tooltip("The maximum allowed angle relative to the starting position.")]
    public float maxAngle = 90f;

    [Header("Events")]
    public UnityEvent<float> OnValueChanged;

    private float _currentAngle = 0f;
    private float _startAngle = 0f;
    private Quaternion _baseRotation; 

    // Interaction State
    private Transform _interactorTransform;
    private Vector3 _worldRotationAxis;
    private Vector3 _grabVector;
    private Vector3 _virtualCursorPos;

    private void Awake()
    {
        // Store the original rotation of the wheel when the game starts
        _baseRotation = transform.localRotation;
        rotationAxis.Normalize();
    }

    public override void OnSelectEnter(IInteractor interactor)
    {
        base.OnSelectEnter(interactor);
        _interactorTransform = interactor.Transform;
        _startAngle = _currentAngle;

        // 1. Raycast to find the exact point on the wheel the player is looking at
        Ray ray = new Ray(_interactorTransform.position, _interactorTransform.forward);
        Vector3 hitPoint = transform.position; // Fallback to center
        
        // Raycast strictly against this specific object's collider
        if (TryGetComponent(out Collider col) && col.Raycast(ray, out RaycastHit hit, 10f))
        {
            hitPoint = hit.point;
        }

        // 2. Get the wheel's rotation plane in world space
        _worldRotationAxis = transform.TransformDirection(rotationAxis).normalized;
        Plane wheelPlane = new Plane(_worldRotationAxis, transform.position);

        // 3. Find the vector from the center of the wheel to where you grabbed it
        Vector3 projectedHit = wheelPlane.ClosestPointOnPlane(hitPoint);
        _grabVector = (projectedHit - transform.position).normalized;

        // 4. Initialize our "virtual hand" at the grab point
        _virtualCursorPos = projectedHit;
    }

    public override void OnSelectExit(IInteractor interactor)
    {
        base.OnSelectExit(interactor);
        _interactorTransform = null;
    }

    private void Update()
    {
        if (!IsSelected || InputConfig == null || _interactorTransform == null) return;
        if (InputConfig.lookDeltaAction == null) return;

        Vector2 lookDelta = InputConfig.lookDeltaAction.action.ReadValue<Vector2>();
        
        if (lookDelta.sqrMagnitude > 0.001f)
        {
            // 1. Move the virtual cursor in 3D space matching the camera's Up/Right directions
            _virtualCursorPos += (_interactorTransform.right * lookDelta.x + _interactorTransform.up * lookDelta.y) * trackingSpeed;

            // 2. Snap the virtual cursor flat onto the wheel's rotation plane
            Plane wheelPlane = new Plane(_worldRotationAxis, transform.position);
            Vector3 projectedCursor = wheelPlane.ClosestPointOnPlane(_virtualCursorPos);

            // 3. Get the new vector from the wheel's center to the virtual cursor
            Vector3 currentVector = (projectedCursor - transform.position).normalized;

            // 4. Calculate the angle between where we initially grabbed it, and where the cursor is now
            float angleDelta = Vector3.SignedAngle(_grabVector, currentVector, _worldRotationAxis);

            // 5. Apply the change and clamp it
            float targetAngle = _startAngle + angleDelta;
            
            if (enableClamp)
            {
                targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
            }

            // 6. Only update if the angle actually changed
            if (!Mathf.Approximately(_currentAngle, target(targetAngle)))
            {
                _currentAngle = targetAngle;
                
                // Set rotation absolutely from the base rotation to prevent mathematical drift over time
                transform.localRotation = _baseRotation * Quaternion.AngleAxis(_currentAngle, rotationAxis);
                
                OnValueChanged?.Invoke(_currentAngle);
            }
        }
    }
    
    // Helper to fix a small typo in the float check logic
    private float target(float val) => val; 
}