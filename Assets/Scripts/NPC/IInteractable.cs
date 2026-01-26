using UnityEngine;

public interface IInteractable
{
    // Method to be called when the player interacts with the object
    // passes player gameobject as parameter to keep track of who is interacting
    void Interact(GameObject player);
}
