using UnityEngine;
using UnityEngine.InputSystem;

public class InteractManager : MonoBehaviour
{
    private IInteractable currentInteractable;

    // function called by input system when interact action is performed
    // calls the interact function on the currently registered interactable
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null)
        {
            currentInteractable.Interact(this.gameObject);
        }
    }

    // registers an interactable as the current one
    // called when player is in range of an interactable
    public void RegisterInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;
        Debug.Log(currentInteractable + " registered as current interactable.");
    }

    // unregisters an interactable as the current one
    // called when player leaves range of interactable
    public void UnregisterInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable)
            currentInteractable = null;
    }
}
