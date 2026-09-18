using System.Collections;

using UnityEngine;

public class LeverInteractable : BaseInteractable
{
    [Header("Lever State")]
    public bool isOn = false;

    [Header("Rotation Settings")]
    public Vector3 offRotation = new Vector3(0, 0, -45);
    public Vector3 onRotation = new Vector3(0, 0, 45);
    public float rotationSpeed = 8f;

    [Header("Linked Lever")]
    public LeverInteractable linkedLever;

    [Header("Manager")]
    public PuzzleManager puzzleManager;

    private Coroutine _rotateCoroutine;

    protected override void Start()
    {
        base.Start();
        transform.localRotation = Quaternion.Euler(isOn ? onRotation : offRotation);
    }

    
    public override void OnSelectEnter(IInteractor interactor)
    {
        
        ToggleLever(true);     
        base.OnSelectEnter(interactor);
      Debug.Log("enterd");
    }

    public void ToggleLever(bool triggerLinked)
    {
        isOn = !isOn;

        if (_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
        }
        _rotateCoroutine = StartCoroutine(AnimateRotation(isOn ? onRotation : offRotation));

        if (triggerLinked && linkedLever != null)
        {
            linkedLever.ToggleLever(false);
        }

        if (puzzleManager != null)
        {
            puzzleManager.CheckPuzzleState();
        }
    }

    private IEnumerator AnimateRotation(Vector3 targetAngles)
    {
        Quaternion targetRot = Quaternion.Euler(targetAngles);
        while (Quaternion.Angle(transform.localRotation, targetRot) > 0.05f)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, 
                targetRot, 
                rotationSpeed * 50f * Time.deltaTime
            );
            yield return null;
        }
        transform.localRotation = targetRot;
    }
}