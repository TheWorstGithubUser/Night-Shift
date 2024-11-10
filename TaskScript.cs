using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScript : MonoBehaviour{
    bool complete = false;
    public bool Complete => complete;
    [SerializeField] Sprite completedSprite;

    [SerializeField] SpriteRenderer renderer;
    Inventory inventory;
    Interactable interactable;

    public string displayName = "";
    // Start is called before the first frame update
    void Start(){
        inventory = FindObjectOfType<Inventory> ();
        interactable = GetComponent <Interactable>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnPlayerInteract () {
        //if (inventory != null) if (!inventory.CheckItem (requiredItem)) return;

        complete = true;
        if (completedSprite != null && renderer != null) {
            interactable.active = false;
            renderer.sprite = completedSprite;
        }
    }
}
