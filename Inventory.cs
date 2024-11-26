using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Inventory : MonoBehaviour{
    public Items items = Items.None;
    [SerializeField] RectTransform inventoryUI;
    [SerializeField] GameObject[] togglable = new GameObject[0];
    

    [SerializeField] GameInput input;


    float openingPhase = 1;
    bool open = true;

	// Start is called before the first frame update
	void Start(){
        
        
    }

    // Update is called once per frame
    void Update(){
        if (input != null) {
            if (input.GetToggleInventory ()) {
                open = !open;
                inventoryUI.gameObject.SetActive (open);
                for (int i = 0; i < togglable.Length; i++) {
                    togglable[i].SetActive (open);
                }
                //for (int i = 0;)
            }
        }
	}

    public void GiveItem (Items items) {
        //if (item == Items.None) return;
        this.items = this.items | items;
    }

    public void RemoveItem (Items items) {
        this.items = this.items & ~items;
    }

    public bool CheckItem (Items items) {
        return (this.items & items) == items;
    }    
}

[Flags]
public enum Items {
    None = 0,
    Broom = 1,
    KeyCard = 1<<1,
    WeedKiller = 1<<2,
    Shirt = 1<<3
}
