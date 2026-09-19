using System.Collections;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [Header("Visibility Settings")]
    public bool showInPresent = true;
    public bool showInPast = false;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        TimeManager.OnTimeShifted += HandleTimeShift;
    }

    private void Start()
    {
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
        bool shouldBeActive = isPresent ? showInPresent : showInPast;

        if (gameObject.activeSelf != shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);

            // If we are activating it, force the physics engine to catch up
            if (shouldBeActive && _collider != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(RefreshColliderNextFrame());
            }
        }
    }

    private IEnumerator RefreshColliderNextFrame()
    {
        for (int i = 0; i < 5; i++)
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _collider.enabled = true;
        }

    }
}