using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // استدعاء النظام الجديد

public class TriggerZoneInteract : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";

    [Header("Event")]
    public UnityEvent onInteract;

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = true;
            Debug.Log("Player entered the zone!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = false;
            Debug.Log("Player left the zone!");
        }
    }

    private void Update()
    {
       
        if (isPlayerInside && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E key pressed inside zone!");
            onInteract?.Invoke();
        }
    }
}