using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WheelInteractable : BaseInteractable
{
 [Header("Input Settings")]
    [Tooltip("Input action for turning (supports Axis or Vector2, e.g. Left/Right arrows or A/D keys).")]
    public InputActionProperty turnAction;

    [Tooltip("Degrees per second the wheel turns at full input.")]
    public float turnSensitivity = 90f;

    [Header("Constraints")]
    public bool enableClamp = true;
    [Tooltip("The minimum allowed total angle.")]
    public float minAngle = -45f;
    [Tooltip("The maximum allowed total angle.")]
    public float maxAngle = 45f;

    [Header("Audio Settings")]
    [Tooltip("«”Õ» «·‹ AudioSource Â‰«")]
    public AudioSource wheelAudioSource;

    [Header("Rotation Events")]
    [Tooltip("Fired whenever the rotation angle changes. Hook this up to RotationSync.SetRotation in the Inspector.")]
    public UnityEvent<float> onAngleChanged;

    private float _currentAngle = 0f;

    private void OnEnable()
    {
        turnAction.action?.Enable();
    }

    private void OnDisable()
    {
        StopAudio();
    }

    private void Update()
    {
        if (!IsSelected || turnAction.action == null)
        {
            StopAudio();
            return;
        }

        float inputVal = 0f;

        // Handles both Vector2 (WASD/Sticks) and float axis inputs (Left/Right Arrows)
        if (turnAction.action.activeValueType == typeof(Vector2))
        {
            inputVal = turnAction.action.ReadValue<Vector2>().x;
        }
        else if (turnAction.action.activeValueType == typeof(float))
        {
            inputVal = turnAction.action.ReadValue<float>();
        }

        if (Mathf.Abs(inputVal) > 0.01f)
        {
            float previousAngle = _currentAngle;

            _currentAngle += inputVal * turnSensitivity * Time.deltaTime;

            if (enableClamp)
            {
                _currentAngle = Mathf.Clamp(_currentAngle, minAngle, maxAngle);
            }

            if (Mathf.Abs(_currentAngle - previousAngle) > 0.001f)
            {
                onAngleChanged?.Invoke(_currentAngle);
                PlayAudio(); 
            }
            else
            {
                StopAudio();
            }
        }
        else
        {
            StopAudio();
        }
    }

    private void PlayAudio()
    {
        if (wheelAudioSource != null && !wheelAudioSource.isPlaying)
        {
            wheelAudioSource.Play();
        }
    }

    private void StopAudio()
    {
        if (wheelAudioSource != null && wheelAudioSource.isPlaying)
        {
            wheelAudioSource.Stop();
        }
    }
}