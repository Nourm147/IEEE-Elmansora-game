using UnityEngine;
using UnityEngine.Events;

public class CustomButton : UIInteractableBase
{
    [Header("Button Event")]
    public UnityEvent OnClick;

    protected override void InteractStart()
    {
        OnClick.Invoke(); 
    }
}
