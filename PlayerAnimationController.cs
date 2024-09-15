using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour{
    PlayerMovement movement;
    [SerializeField] PlayerCursor playerCursor;

    float facingDir = 1;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer armSprite;
    [SerializeField] private Animator animator;

    const string WALK_ANIM_NAME = "Walk";
    const string STAND_ANIM_NAME = "Standing";

    // Start is called before the first frame update
    void Start(){
        movement = GetComponent<PlayerMovement> ();
    }

    // Update is called once per frame
    void Update(){
        var mdir = movement.GetPlayerDirection ();
        if (mdir.x != 0) {
			facingDir = mdir.x;
		}
        if (mdir != Vector2.zero) {
            animator.Play (WALK_ANIM_NAME);
        }
        else {
            animator.Play (STAND_ANIM_NAME);
        }

        armSprite.transform.rotation = playerCursor.transform.rotation;
        armSprite.flipX = 0 > playerCursor.GetDirection().x;

		spriteRenderer.flipX = playerCursor.GetDirection().x < 0;
    }
}
