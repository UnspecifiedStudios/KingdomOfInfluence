using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public void OnTriggerEnter(Collider other)
    {
        // check if player entered trigger
        if (other.CompareTag("Player"))
        {
            // get interact manager from player and register interactable
            other.GetComponent<InteractManager>().RegisterInteractable(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // check if player exited trigger
        if (other.CompareTag("Player"))
        {
            // get interact manager from player and unregister interactable
            other.GetComponent<InteractManager>().UnregisterInteractable(this);
        }
    }

    public void Interact()
    {
        Debug.Log("NPC interacted with!");
        // Add additional interaction logic here
    }
}
