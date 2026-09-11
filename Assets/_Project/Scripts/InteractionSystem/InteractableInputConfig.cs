// --- InteractableInputConfig.cs ---
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Interaction/Input Config")]
public class InteractableInputConfig : ScriptableObject
{
    [Tooltip("Input to pick up / interact (e.g., Left Mouse or E)")]
    public InputActionProperty selectEnterAction;

    [Tooltip("Input to drop / release interaction (e.g., Q or Left Mouse again)")]
    public InputActionProperty selectCancelAction;

    [Tooltip("Input to fire / use (e.g., Left Mouse)")]
    public InputActionProperty activateAction;

    [Tooltip("Input to rotate object while held (e.g., Right Mouse)")]
    public InputActionProperty rotateModifierAction;

    [Tooltip("Input for mouse delta (used for rotation)")]
    public InputActionProperty lookDeltaAction;

    [Tooltip("Input action for mouse scroll wheel (Action Type: Value, Control Type: Vector2)")]
    public InputActionProperty scrollAction;

    private void OnEnable()
    {
        // Ensure actions are enabled
        selectEnterAction.action?.Enable();
        selectCancelAction.action?.Enable();
        activateAction.action?.Enable();
        rotateModifierAction.action?.Enable();
        lookDeltaAction.action?.Enable();
        scrollAction.action?.Enable();
    }
}