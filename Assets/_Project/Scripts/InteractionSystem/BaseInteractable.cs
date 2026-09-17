// --- BaseInteractable.cs ---
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseInteractable : MonoBehaviour, IHoverInteractable, ISelectInteractable, IActivateInteractable
{
    public InteractionTimeSettings timeSettings = new InteractionTimeSettings();
    public InteractableInputConfig inputConfig;
    public InteractableInputConfig InputConfig => inputConfig;
    public Transform Transform => transform;
    public bool IsSelected { get; private set; }

    public UnityEvent onHoverEnter, onHoverExit, onSelectEnter, onSelectExit, onActivate, onDeactivate;

    public virtual void OnHoverEnter(IInteractor interactor) { onHoverEnter?.Invoke(); }
    public virtual void OnHoverExit(IInteractor interactor) { onHoverExit?.Invoke(); }

    public virtual void OnSelectEnter(IInteractor interactor)
    {
        IsSelected = true;
        onSelectEnter?.Invoke();
    }

    public virtual void OnSelectExit(IInteractor interactor)
    {
        IsSelected = false;
        onSelectExit?.Invoke();
    }

    public virtual void OnActivate(IInteractor interactor) { onActivate?.Invoke(); }
    public virtual void OnDeactivate(IInteractor interactor) { onDeactivate?.Invoke(); }

    protected virtual void Start()
    {
        TimeManager.OnTimeShifted += HandleTimeShift;
        // Initial state
        if (TimeManager.Instance != null)
        {
            HandleTimeShift(TimeManager.Instance.isPresent);
        }
    }

    protected virtual void OnDestroy()
    {
        TimeManager.OnTimeShifted -= HandleTimeShift;
    }

    private void HandleTimeShift(bool isPresent)
    {
        this.enabled = isPresent ? timeSettings.activeInPresent : timeSettings.activeInPast;
    }
}

[System.Serializable]
public class InteractionTimeSettings
{
    public bool activeInPresent = true;
    public bool activeInPast = true;
}