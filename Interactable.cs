using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour{
    static List<Interactable> interactables = new List<Interactable>();

    
    [SerializeField] SpriteRenderer popup;
    Vector3 popupPos;
    Vector3 popupHidePos = Vector3.zero;
    Vector3 popupScale;
    float popupPhase = 0;

    public bool active = true;
    public Items requiredItems = Items.None;
    public bool removeRequiredItems = false;

    Inventory inventory;


    bool hasCorrectItems = true;

    const float selectionTime = 0.25f;
    // Start is called before the first frame update
    void Start(){
        inventory = FindAnyObjectByType<Inventory> ();
        interactables.Add (this);
        if (popup != null) {
            popupScale = popup.transform.localScale;
            popupPos = popup.transform.localPosition;
            
        }
		//SendMessage ("OnPlayerInteract", null, SendMessageOptions.DontRequireReceiver);
	}

    // Update is called once per frame
    void Update(){
        if (popup != null) {
			popupPhase = Mathf.Clamp (popupPhase, 0, 1);
            popup.transform.localScale = (Mathf.Exp(popupPhase)-1)/Mathf.Exp(1) * popupScale;
            popup.transform.localPosition = Vector3.Lerp(popupHidePos, popupPos, Mathf.SmoothStep(0, 1, popupPhase));
			popupPhase -= Time.deltaTime / selectionTime;
            var col = popup.color;
            if (inventory != null)
                hasCorrectItems = inventory.CheckItem (requiredItems);
            col.a = hasCorrectItems ? 1 : 0.5f;
            popup.color = col;
        }
        
    }

    public void WhenSelected () {
        popupPhase += 2 * Time.deltaTime / selectionTime;
    }

    public void TriggerInteraction () {
        if (!active || !hasCorrectItems) return;
        if (removeRequiredItems && inventory != null) inventory.RemoveItem (requiredItems);
		gameObject.SendMessage ("OnPlayerInteract", null, SendMessageOptions.DontRequireReceiver);
	}

	private void OnDestroy () {
        interactables.Remove (this);
	}

    public static Interactable? GetInteractable (Vector2 searchPos, float radius) {
        float currentDist = radius * 2;
        Interactable? res = null;

        for (int i = 0; i < interactables.Count; i++) {
            if (!interactables[i].active) continue;
            float dist = Vector2.Distance (searchPos, interactables[i].transform.position);

            if (dist < radius && dist < currentDist) {
                currentDist = dist;
                res = interactables[i];
            }
        }

        return res;
    }
}

public enum InteractableLock {
    Unlocked,
    VisibleLock,
    HideLock,
}
