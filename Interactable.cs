using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour{
    public static List<Interactable> interactables = new List<Interactable>();
    // Start is called before the first frame update
    void Start()
    {
        interactables.Add (this);
		//SendMessage ("OnPlayerInteract", null, SendMessageOptions.DontRequireReceiver);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy () {
        interactables.Remove (this);
	}

    public static Interactable? GetInteractable (Vector2 searchPos, float radius) {
        float currentDist = radius * 2;
        Interactable? res = null;

        for (int i = 0; i < interactables.Count; i++) {
            float dist = Vector2.Distance (searchPos, interactables[i].transform.position);

            if (dist < radius && dist < currentDist) {
                currentDist = dist;
                res = interactables[i];
            }
        }

        return res;
    }
}
