using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [Header("Visibility Settings")]
    public bool showInPresent = true; // ÌŸÂ— ›Ì «·Õ«÷—ø
    public bool showInPast = false;   // ÌŸÂ— ›Ì «·„«÷Ìø

    private void OnEnable()
    {
        // ·„« «·”ﬂ—Ì»  Ì‘ €·° »Ì‘ —ﬂ ›Ì «·‹ Event
        TimeManager.OnTimeShifted += HandleTimeShift;
    }

    private void OnDisable()
    {
        // ·„« Ì ﬁ›·° »Ì·€Ì «·«‘ —«ﬂ ⁄‘«‰ „«ÌÕ’·‘ Memory Leak
        TimeManager.OnTimeShifted -= HandleTimeShift;
    }

    private void HandleTimeShift(bool isPresent)
    {
        // »‰«¡ ⁄·Ï «·“„‰ «·Õ«·Ì° ‘€· √Ê «ﬁ›· «·‹ Object
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
