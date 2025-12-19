using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    private IInteractable currentInteractable;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void RegisterInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;
        Debug.Log(currentInteractable + " registered as current interactable.");
    }

    public void UnregisterInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable)
            currentInteractable = null;
    }
}
