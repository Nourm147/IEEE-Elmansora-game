using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public String interactionTag;
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerStayEvent;
    public UnityEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactionTag))
            OnTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(interactionTag))
            OnTriggerExitEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(interactionTag))
            OnTriggerStayEvent?.Invoke();
    }
}
