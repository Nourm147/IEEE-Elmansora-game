using UnityEngine;
using UnityEngine.InputSystem;

public class RayInteractor : MonoBehaviour, IInteractor
{
    public Transform Transform => transform;
    public Transform AttachTransform => attachTransform;

    [Header("Settings")]
    public Transform attachTransform; // Where grabbed objects go
    public float interactRange = 3f;

    [Header("Manipulation Settings")]
    public float rotationSpeed = 0.2f;
    public float scrollSpeed = 0.001f; // Adjust based on Input System raw values
    public float minGrabDistance = 0.5f;
    public float maxGrabDistance = 5f;

    public LayerMask interactLayer;

    private BaseInteractable _hoveredInteractable;
    private BaseInteractable _selectedInteractable;

    // Tracks how far the attach point currently is from the camera
    private float _currentGrabDistance;

    // Public property your FPS Look script can read to pause camera rotation
    public bool IsRotatingObject { get; private set; }

    void Update()
    {
        HandleRaycast();
        HandleInput();
    }

    private void HandleRaycast()
    {
        // Don't change hover targets if we are currently holding something
        if (_selectedInteractable != null) return;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            var interactable = hit.collider.GetComponentInParent<BaseInteractable>();
            if (interactable != _hoveredInteractable)
            {
                if (_hoveredInteractable != null)
                    InteractionManager.Instance.HoverExit(this, _hoveredInteractable);

                _hoveredInteractable = interactable;
                if (_hoveredInteractable != null)
                    InteractionManager.Instance.HoverEnter(this, _hoveredInteractable);
            }
        }
        else if (_hoveredInteractable != null)
        {
            InteractionManager.Instance.HoverExit(this, _hoveredInteractable);
            _hoveredInteractable = null;
        }
    }

    private void HandleInput()
    {
        BaseInteractable target = _selectedInteractable != null ? _selectedInteractable : _hoveredInteractable;
        if (target == null || target.InputConfig == null) return;

        var config = target.InputConfig;

        // Select / Drop
        if (config.selectAction != null && config.selectAction.action.WasPressedThisFrame())
        {
            if (_selectedInteractable == null)
            {
                _selectedInteractable = target;

                // Set initial grab distance to the object's current distance so it doesn't violently snap
                _currentGrabDistance = Vector3.Distance(transform.position, _selectedInteractable.transform.position);
                _currentGrabDistance = Mathf.Clamp(_currentGrabDistance, minGrabDistance, maxGrabDistance);
                UpdateAttachTransformPosition();

                InteractionManager.Instance.SelectEnter(this, _selectedInteractable);
            }
            else
            {
                InteractionManager.Instance.SelectExit(this, _selectedInteractable);
                _selectedInteractable = null;
                IsRotatingObject = false;
            }
        }

        if (_selectedInteractable == null) return;

        // Activate (Fire/Use)
        if (config.activateAction != null)
        {
            if (config.activateAction.action.WasPressedThisFrame())
                InteractionManager.Instance.Activate(this, _selectedInteractable);
            if (config.activateAction.action.WasReleasedThisFrame())
                InteractionManager.Instance.Deactivate(this, _selectedInteractable);
        }

        // Rotate object logic (Right Mouse Button)
        if (config.rotateModifierAction != null && config.lookDeltaAction != null)
        {
            IsRotatingObject = config.rotateModifierAction.action.IsPressed();

            if (IsRotatingObject)
            {
                Vector2 delta = config.lookDeltaAction.action.ReadValue<Vector2>();
                // Rotate around the camera's up and right axes
                _selectedInteractable.transform.RotateAround(_selectedInteractable.transform.position, transform.up, -delta.x * rotationSpeed);
                _selectedInteractable.transform.RotateAround(_selectedInteractable.transform.position, transform.right, delta.y * rotationSpeed);
            }
        }

        // Scroll distance logic (Mouse Scroll Wheel)
        if (config.scrollAction != null)
        {
            float scrollDelta = config.scrollAction.action.ReadValue<Vector2>().y;

            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                // Modify the distance
                _currentGrabDistance += scrollDelta * scrollSpeed;
                _currentGrabDistance = Mathf.Clamp(_currentGrabDistance, minGrabDistance, maxGrabDistance);

                UpdateAttachTransformPosition();
            }
        }
    }

    private void UpdateAttachTransformPosition()
    {
        if (attachTransform != null)
        {
            // Pushes the attach transform forward/backward along the camera's local Z axis
            attachTransform.position = transform.position + (transform.forward * _currentGrabDistance);
        }
    }
}