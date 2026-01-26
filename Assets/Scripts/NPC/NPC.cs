using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField, Header("Unique identifier for this NPC (string)")]
    private string npcID;
    [SerializeField, Header("Starting dialogue node for this NPC")]
    private DialogueNode startNode;

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

    public void Interact(GameObject player)
    {
        Debug.Log("NPC interacted with!");
        
        player.GetComponent<DialogueManager>().StartDialogue(npcID, startNode);
    }
}
