// --- Interfaces.cs ---
using UnityEngine;

public interface IInteractor
{
    Transform Transform { get; }
    Transform AttachTransform { get; } // Where grabbed objects snap to
}

public interface IInteractable
{
    Transform Transform { get; }
    InteractableInputConfig InputConfig { get; }
    bool IsSelected { get; }
}

public interface IHoverInteractable : IInteractable
{
    void OnHoverEnter(IInteractor interactor);
    void OnHoverExit(IInteractor interactor);
}

public interface ISelectInteractable : IInteractable
{
    void OnSelectEnter(IInteractor interactor);
    void OnSelectExit(IInteractor interactor);
}

public interface IActivateInteractable : IInteractable
{
    void OnActivate(IInteractor interactor);
    void OnDeactivate(IInteractor interactor);
}

