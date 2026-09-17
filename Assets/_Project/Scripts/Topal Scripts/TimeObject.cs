using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [Header("Visibility Settings")]
    public bool showInPresent = true; 
    public bool showInPast = false;   

    private void Start()
    {
        TimeManager.OnTimeShifted += HandleTimeShift;
        
        // Ensure the initial state matches the current time when the scene starts
        if (TimeManager.Instance != null)
        {
            HandleTimeShift(TimeManager.Instance.isPresent);
        }
    }

    private void OnDestroy()
    {
        TimeManager.OnTimeShifted -= HandleTimeShift;
    }

    private void HandleTimeShift(bool isPresent)
    {
        gameObject.SetActive(isPresent ? showInPresent : showInPast);
    }
}
