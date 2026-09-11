using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [Header("Visibility Settings")]
    public bool showInPresent = true; 
    public bool showInPast = false;   

    private void OnEnable()
    {
        TimeManager.OnTimeShifted += HandleTimeShift;
    }

  
    private void HandleTimeShift(bool isPresent)
    {
        if (isPresent)
        {
            gameObject.SetActive(showInPresent);
        }
        else
        {
            gameObject.SetActive(showInPast);
        }
    }
}
