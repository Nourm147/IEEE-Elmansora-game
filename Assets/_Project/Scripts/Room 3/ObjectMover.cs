using System.Collections;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform targetPoint;
    public float moveSpeed = 3f;

    private Coroutine _moveCoroutine;

    public void MoveToTarget()
    {
        if (targetPoint == null)
        {
            Debug.LogWarning($"Target Point is not assigned on {gameObject.name}!");
            return;
        }

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _moveCoroutine = StartCoroutine(AnimateMovement());
    }

    private IEnumerator AnimateMovement()
    {
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPoint.position, 
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPoint.position;
        _moveCoroutine = null;
    }
}