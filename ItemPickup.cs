using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{
    // Start is called before the first frame update
    Inventory inventory;
    public Items item = Items.None;
    void Start(){
        inventory = FindAnyObjectByType<Inventory> ();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnPlayerInteract () {
        inventory.GiveItem (item);
        Destroy (gameObject);
    }
}
