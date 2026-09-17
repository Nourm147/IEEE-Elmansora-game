using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SocketInteractor : MonoBehaviour, IInteractor
{
    public Transform Transform => transform;
    public Transform AttachTransform => attachTransform;

    [Header("Settings")]
    public Transform attachTransform;
    public Material hoverMaterial; // Material used for drawing the ghost mesh
    
    [Tooltip("If true, the socket will immediately yank the interactable. If false, it waits for the player to drop it.")]
    public bool autoGrabOnHover = false;
    
    public LayerMask interactableLayer;

    [Header("Events")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    public UnityEvent onSelectEnter;
    public UnityEvent onSelectExit;

    private BaseInteractable _hoveredInteractable;
    private BaseInteractable _selectedInteractable;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        if (attachTransform == null)
        {
            attachTransform = transform;
        }
    }

    private void Update()
    {
        // Force drop if the object became disabled (e.g. due to time shift)
        if (_selectedInteractable != null && !_selectedInteractable.isActiveAndEnabled)
        {
            InteractionManager.Instance.SelectExit(this, _selectedInteractable);
            _selectedInteractable = null;
            onSelectExit?.Invoke();
        }

        CheckForStolenObject();

        if (_selectedInteractable == null)
        {
            HandleHoverPreview();
            HandleAutoGrab();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only accept if we don't already have a hovered or selected object
        if (_selectedInteractable != null || _hoveredInteractable != null) return;

        // Check layer mask
        if (((1 << other.gameObject.layer) & interactableLayer) == 0) return;

        BaseInteractable interactable = other.GetComponentInParent<BaseInteractable>();
        if (interactable != null && interactable.isActiveAndEnabled && interactable != _selectedInteractable)
        {
            _hoveredInteractable = interactable;
            InteractionManager.Instance.HoverEnter(this, _hoveredInteractable);
            onHoverEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_hoveredInteractable == null) return;

        BaseInteractable interactable = other.GetComponentInParent<BaseInteractable>();
        if (interactable == _hoveredInteractable)
        {
            InteractionManager.Instance.HoverExit(this, _hoveredInteractable);
            _hoveredInteractable = null;
            onHoverExit?.Invoke();
        }
    }

    private void HandleHoverPreview()
    {
        if (_hoveredInteractable == null || hoverMaterial == null) return;

        // Create matrices that ONLY represent position and rotation (scale = 1)
        // This prevents non-uniform scale deformation when calculating the offset.
        Matrix4x4 currentRootTR = Matrix4x4.TRS(_hoveredInteractable.transform.position, _hoveredInteractable.transform.rotation, Vector3.one);
        Matrix4x4 targetRootTR = Matrix4x4.TRS(attachTransform.position, attachTransform.rotation, Vector3.one);
        
        // This offset matrix will rigidly move and rotate the meshes from their current root to the attach transform
        Matrix4x4 offsetMatrix = targetRootTR * currentRootTR.inverse;

        // Draw ghost meshes for preview
        MeshFilter[] meshFilters = _hoveredInteractable.GetComponentsInChildren<MeshFilter>();
        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh == null) continue;

            // Apply the rigid offset to the mesh's current world matrix
            Matrix4x4 finalMatrix = offsetMatrix * mf.transform.localToWorldMatrix;
            
            Graphics.DrawMesh(mf.sharedMesh, finalMatrix, hoverMaterial, gameObject.layer);
        }
    }

    private void HandleAutoGrab()
    {
        if (_hoveredInteractable == null) return;

        // Wait for the object to be released by the player unless autoGrabOnHover is true
        bool isReleased = !_hoveredInteractable.IsSelected;

        if (autoGrabOnHover || isReleased)
        {
            _selectedInteractable = _hoveredInteractable;
            
            // Clear hover state
            InteractionManager.Instance.HoverExit(this, _hoveredInteractable);
            _hoveredInteractable = null;
            onHoverExit?.Invoke();

            // Select it (snap to socket)
            InteractionManager.Instance.SelectEnter(this, _selectedInteractable);
            onSelectEnter?.Invoke();
        }
    }

    private void CheckForStolenObject()
    {
        // If we think we have an object, but it has been grabbed by another interactor (e.g. the player),
        // its parent will have changed. We clear our reference.
        if (_selectedInteractable != null && _selectedInteractable.transform.parent != attachTransform)
        {
            // It was stolen!
            _selectedInteractable = null;
            onSelectExit?.Invoke();
        }
    }
}
