using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOnCollide : MonoBehaviour{
    float glowChange = -1;
    float glow = 0;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start(){
        sprite = GetComponent<SpriteRenderer> ();
    }

    // Update is called once per frame
    void Update(){
        glow = Mathf.Clamp01 (glow + glowChange * Time.deltaTime);
        Color c = sprite.color;
        c.a = glow;
        sprite.color = c;
    }

	private void OnTriggerStay2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            glowChange = 1;
        }
	}
	private void OnTriggerExit2D (Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			glowChange = -1;
		}
	}
}
