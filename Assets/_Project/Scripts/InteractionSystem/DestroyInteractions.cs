using UnityEngine;

public class DestroyInteractions : MonoBehaviour
{
    [Tooltip("Assign the specific interactable components you want to remove.")]
    public BaseInteractable[] interactables;

    // Call this method to destroy all assigned BaseInteractable components
    public void DestroyAllInteractions()
    {
        if (interactables == null) return;

        foreach (BaseInteractable interactable in interactables)
        {
            if (interactable != null)
            {
                interactable.OnSelectExit(null);
                // Destroys only the BaseInteractable script component, leaving the GameObject and its colliders intact
                Destroy(interactable);
            }
        }

        // Clear the array to free up memory and prevent null reference errors if called again
        interactables = new BaseInteractable[0];
    }
}