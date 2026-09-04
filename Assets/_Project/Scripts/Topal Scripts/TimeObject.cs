using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [Header("Visibility Settings")]
    public bool showInPresent = true; // ���� �� �����ѿ
    public bool showInPast = false;   // ���� �� �������

    private void OnEnable()
    {
        // ��� �������� ����� ������ �� ��� Event
        TimeManager.OnTimeShifted += HandleTimeShift;
    }

    // private void OnDisable()
    // {
    //     // ��� ����� ����� �������� ���� ������� Memory Leak
    //     TimeManager.OnTimeShifted -= HandleTimeShift;
    // }

    private void HandleTimeShift(bool isPresent)
    {
        // ����� ��� ����� ������� ��� �� ���� ��� Object
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
