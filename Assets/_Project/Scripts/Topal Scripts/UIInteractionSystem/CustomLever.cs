using UnityEngine;
using UnityEngine.Events;

public class CustomLever : UIInteractableBase
{
    [Header("Lever Settings")]
    public Transform pivotObject;

    [Header("Lever Angles")]
    public Vector3 offRotation = Vector3.zero;
    public Vector3 onRotation = new Vector3(45, 0, 0); 
    public float rotationSpeed = 8f;

    [Header("Lever Event")]
    public UnityEvent<bool> OnToggled;

    private bool isOn = false;
    private Quaternion targetRotation;
    private Transform targetToRotate;

    protected override void Awake()
    {
        base.Awake();

        targetToRotate = pivotObject != null ? pivotObject : transform;
        targetRotation = targetToRotate.localRotation;
    }

    protected override void InteractStart()
    {
        isOn = !isOn;
        targetRotation = Quaternion.Euler(isOn ? onRotation : offRotation);
        OnToggled.Invoke(isOn);
    }

    void Update()
    {
        if (targetToRotate != null)
        {
            targetToRotate.localRotation = Quaternion.Lerp(targetToRotate.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
