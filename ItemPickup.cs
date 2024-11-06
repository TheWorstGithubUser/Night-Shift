using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{
    // Start is called before the first frame update
    Inventory inventory;
    public Items item = Items.None;
    [SerializeField] bool blockPickupIfHeld = false;
    
    void Start(){
        inventory = FindAnyObjectByType<Inventory> ();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnPlayerInteract () {
        if (blockPickupIfHeld && inventory.CheckItem(item)) return;
        inventory.GiveItem (item);
        Destroy (gameObject);
    }
}
