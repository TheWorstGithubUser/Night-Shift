using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost: MonoBehaviour {
    [SerializeField] Transform ventHolder;
    [SerializeField] SpriteRenderer sprite;
    Animator animator;
	[SerializeField] string walkAnimation = "GhostWalk";
	[SerializeField] string idleAnimation = "GhostIdle";
    [SerializeField] string ventP1Animation = "GhostVent S1";
    [SerializeField] float p1Length = 0.4f;
	[SerializeField] string ventP2Animation = "GhostVent S2";
    [SerializeField] float p2Length = 0.5f;

	Enemy ai;
    private Transform targetVent = null;
    [SerializeField] float ventBias = 1.3f;
    private bool shouldVent;

    const float TIMER = 0.4f;
    float timerCurrent = 0;

    string animState = "";
    // Start is called before the first frame update
    void Start () {
        ai = GetComponent<Enemy> ();
        animator = sprite.GetComponent<Animator> ();
        timerCurrent = Random.Range (0, TIMER);
    }

    // Update is called once per frame
    void Update () {
        Transform closestVent;
        (closestVent, targetVent, shouldVent) = SearchVents ();
        timerCurrent += Time.deltaTime;

        if (timerCurrent > TIMER) {
            if (shouldVent) {
                ai.ForceTarget (closestVent.position);
            }
            else {
                ai.EnableAutoTarget ();
            }
            timerCurrent -= TIMER;
        }

        // animation stuff
        if (ai.CurrentMovementVelocity.x != 0) {
            var scale = sprite.transform.localScale;
            scale.x = Mathf.Sign (ai.CurrentMovementVelocity.x) * Mathf.Abs (scale.x);
            sprite.transform.localScale = scale;
        }

        if (animState == ventP1Animation) {
            if (!ai.IsWaiting) {
                SetAnimationState (ventP2Animation);
                transform.position = targetVent.position;
                ai.MakeWait (p2Length);
            }
        }
        else if (animState == ventP2Animation) {
            if (!ai.IsWaiting) {
                SetAnimationState (idleAnimation);
            }
        }
        else {
            if (ai.CurrentMovementVelocity.magnitude < 0.1) {
				SetAnimationState (idleAnimation);
            }
            else {
				SetAnimationState (walkAnimation);
            }
        }
        
    }

    void SetAnimationState (string animation) {
        if (animation == animState) return;
        animator.Play (animation);
        animState = animation;
    }

    void OnTriggerStay2D (Collider2D coll) {
        if (coll.tag == "Vent" && shouldVent && animState != ventP1Animation && animState != ventP2Animation) {
            SetAnimationState (ventP1Animation);
            ai.MakeWait (p1Length);
            transform.position = coll.transform.position;
            //transform.position = targetVent.position;
        }
    }

    (Transform, Transform, bool) SearchVents () {
        Transform gClosest = null, tClosest = null;
        Vector3 enemyTarget;
        if (ai.RemembersPlayer) {
            enemyTarget = ai.player.transform.position;
        }
        else {
            enemyTarget =  ai.GetCurrentWaypoint ();
        }
        float gDist = float.MaxValue, tDist = float.MaxValue;
        for (int i = 0; i < ventHolder.childCount; i++) {
            float gd = Vector2.Distance (ventHolder.GetChild (i).position, transform.position);
            float td = Vector2.Distance (ventHolder.GetChild (i).position, enemyTarget);
			if (gDist > gd) {
                gDist = gd;
                gClosest = ventHolder.GetChild (i);
            }

            if (tDist > td) {
                tClosest = ventHolder.GetChild(i);
                tDist = td;
            }
        }

        (float pathDist, bool completePath, _) = ai.Pather.ShortestPath(transform.position, enemyTarget, 2);
        if (!completePath) { pathDist *= 2; }
        return (gClosest, tClosest, gDist + tDist < ventBias * pathDist&&tClosest != gClosest);
    }
}
