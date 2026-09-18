using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    [Header("All 6 Levers")]
    public LeverInteractable[] levers = new LeverInteractable[6];

    [Header("Target Solution")]
    public bool[] targetSolution = new bool[6];

    [Header("Events")]
    public UnityEvent onPuzzleSolved;

    private bool isSolved = false;

    public void CheckPuzzleState()
    {
        if (isSolved) return;

        if (levers.Length != targetSolution.Length)
        {
            Debug.LogError("Lever count does not match target solution size.");
            return;
        }

        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i] == null || levers[i].isOn != targetSolution[i])
            {
                return;
            }
        }

        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        Debug.Log("Puzzle Solved!");
        onPuzzleSolved?.Invoke();
    }
}