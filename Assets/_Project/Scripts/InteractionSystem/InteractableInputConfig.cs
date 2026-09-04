// --- InteractableInputConfig.cs ---
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Interaction/Input Config")]
public class InteractableInputConfig : ScriptableObject
{
    [Tooltip("Input to pick up / interact (e.g., Left Mouse or E)")]
    public InputActionReference selectAction;

    [Tooltip("Input to fire / use (e.g., Left Mouse)")]
    public InputActionReference activateAction;

    [Tooltip("Input to rotate object while held (e.g., Right Mouse)")]
    public InputActionReference rotateModifierAction;

    [Tooltip("Input for mouse delta (used for rotation)")]
    public InputActionReference lookDeltaAction;

    [Tooltip("Input action for mouse scroll wheel (Action Type: Value, Control Type: Vector2)")]
    public InputActionReference scrollAction;

    private void OnEnable()
    {
        // Ensure actions are enabled
        selectAction?.action?.Enable();
        activateAction?.action?.Enable();
        rotateModifierAction?.action?.Enable();
        lookDeltaAction?.action?.Enable();
        scrollAction?.action?.Enable();
    }
}