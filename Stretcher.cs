using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretcher : MonoBehaviour {
    [SerializeField] Transform tr1, tr2;

    [SerializeField] Vector2 scale = Vector2.one;
    [SerializeField] float origin = 0.5f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    
    void LateUpdate() {
        if (tr1 != null && tr2 != null) {
            // keeps the object between the two transform
            transform.position = Vector3.Lerp (tr1.position, tr2.position, origin);
            
            // streaches the object to connect the two transforms
            transform.localScale = new Vector2 (scale.x, scale.y * Vector2.Distance (tr1.position, tr2.position));
            
            // rotates the object so it always points toward the two transforms
            transform.rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (tr2.position.y - tr1.position.y, tr2.position.x - tr1.position.x) * Mathf.Rad2Deg + 90);
        }
    }
}
