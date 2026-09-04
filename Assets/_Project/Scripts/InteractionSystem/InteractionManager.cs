using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    // Track what is holding what
    private Dictionary<IInteractable, IInteractor> _interactableToInteractor = new Dictionary<IInteractable, IInteractor>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HoverEnter(IInteractor interactor, IHoverInteractable interactable) => interactable.OnHoverEnter(interactor);
    public void HoverExit(IInteractor interactor, IHoverInteractable interactable) => interactable.OnHoverExit(interactor);

    public void SelectEnter(IInteractor interactor, ISelectInteractable interactable)
    {
        // Exclusivity: If someone else is holding this, force them to drop it
        if (_interactableToInteractor.TryGetValue(interactable, out IInteractor currentHolder))
        {
            if (currentHolder != interactor)
                SelectExit(currentHolder, interactable);
        }

        _interactableToInteractor[interactable] = interactor;
        interactable.OnSelectEnter(interactor);
    }

    public void SelectExit(IInteractor interactor, ISelectInteractable interactable)
    {
        if (_interactableToInteractor.ContainsKey(interactable))
        {
            _interactableToInteractor.Remove(interactable);
            interactable.OnSelectExit(interactor);
        }
    }

    public void Activate(IInteractor interactor, IActivateInteractable interactable) => interactable.OnActivate(interactor);
    public void Deactivate(IInteractor interactor, IActivateInteractable interactable) => interactable.OnDeactivate(interactor);
}