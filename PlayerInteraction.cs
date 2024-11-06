using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour{
    GameInput gameInput;
    [SerializeField] float interactionRadius = 5;
    // Start is called before the first frame update
    void Start()
    {
        gameInput = GetComponent<GameInput> ();
    }

    // Update is called once per frame
    void Update()
    {
        Interactable? interactable = Interactable.GetInteractable(transform.position, interactionRadius);
        if (interactable != null) {
            if (gameInput.GetInteraction ()) {
                interactable.TriggerInteraction ();
            }
            interactable.WhenSelected ();
        }
    }
}
