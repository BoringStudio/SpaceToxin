using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour {
    public Interactable InteractableObject = null;

    void Start()
    {
        if (InteractableObject == null)
        {
            InteractableObject = GetComponent<Interactable>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (InteractableObject == null) return;

        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        if (player.CurrentInteractable == null)
        {
            player.CurrentInteractable = InteractableObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InteractableObject == null) return;

        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        Player player = collision.GetComponent<Player>();

        if (player.CurrentInteractable == InteractableObject)
        {
            player.CurrentInteractable = null;
        }
    }
}
