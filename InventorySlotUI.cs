using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour{
    public Items item;
    public float emptyOpacity = 0;
    Inventory inventory;
    Image image;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start(){
        image = GetComponent<Image> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        inventory = FindAnyObjectByType<Inventory> ();
    }

    // Update is called once per frame
    void Update(){
        Color color = Color.white;
        if (image != null) {
            color = image.color;
            color.a = GetCorrectOpacity ();
            image.color = color;
        }
        if (spriteRenderer != null) {
			color = spriteRenderer.color;
			color.a = GetCorrectOpacity();
			spriteRenderer.color = color;
		}
    }

    float GetCorrectOpacity () {
        return inventory.CheckItem (item) ? 1 : emptyOpacity;
	}
}
