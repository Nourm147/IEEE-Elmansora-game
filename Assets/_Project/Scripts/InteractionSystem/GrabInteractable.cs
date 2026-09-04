using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabInteractable : BaseInteractable
{
    private Rigidbody _rb;
    private Transform _originalParent;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnSelectEnter(IInteractor interactor)
    {
        base.OnSelectEnter(interactor);
        _originalParent = transform.parent;

        _rb.isKinematic = true;
        transform.SetParent(interactor.AttachTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public override void OnSelectExit(IInteractor interactor)
    {
        base.OnSelectExit(interactor);
        transform.SetParent(_originalParent);
        _rb.isKinematic = false;
    }
}