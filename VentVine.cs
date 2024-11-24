using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VentVine : MonoBehaviour{
    public bool active = true;

    private Transform target;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float returnSpeed = 2;

    [SerializeField] private float mouthOpenDistance = 3;
    [SerializeField] private float mouthOpenDeg = 45;
    [SerializeField] private float chompDist = 0.25f;

    [SerializeField] private SpriteRenderer jawUpper;
    [SerializeField] private SpriteRenderer jawLower;

    [SerializeField] private GameObject[] parts = new GameObject[0];
    [SerializeField] private Transform ventSprite;


    //private float mouthAngle = 0;


    // Start is called before the first frame update
    void Start(){
        target = FindObjectOfType<PlayerMovement> ().transform;
    }

    // Update is called once per frame
    void Update(){
        
        // controls decides whether to hunt the player or return to the vent
        if (active) {
            ActiveUpdate (); // hostile enemy logic
        }
        else {
            InactiveUpdate (); // return to player
        }

        //mouthAngle = Mathf.Atan2 (target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
    }

    void ActiveUpdate () {
        EnableDisableThings (true);
        Transform parent = transform.parent;
        Vector2 parentDist = target.position - parent.position;
        Vector2 dist = target.position - transform.position;
        Vector2 dir = dist.normalized;
        // checks for line of sight
        if (Physics2D.Raycast (parent.position, parentDist.normalized, parentDist.magnitude, obstacleMask).collider == null) {
            // chase enemy
            transform.position += (Vector3)(movementSpeed * dir * Time.deltaTime);
        }
        else {
            // return to vent
            //SetMouthOpenness (, 0);
            ReturnToVent ();
        }
        float plDist = dist.magnitude;
        SetMouthOpenness (target.position - transform.position, MouthOpenFunction ((mouthOpenDistance - plDist)/mouthOpenDistance)*mouthOpenDeg);
    }

    void InactiveUpdate () {
        //ReturnToVent ();
        transform.position = Vector2.MoveTowards (transform.position, ventSprite.position, returnSpeed * Time.deltaTime);
        
        if (Vector2.Distance (ventSprite.position, transform.position) < 0.1f) {
            EnableDisableThings (false);
        }
        SetMouthOpenness (transform.position - ventSprite.position, 0);
    }

    void ReturnToVent () {
        Transform parent = transform.parent;
        transform.position = Vector2.MoveTowards(transform.position, parent.position, returnSpeed * Time.deltaTime);
        
    }

    void SetMouthOpenness (Vector2 facingDir, float openness) {
        float mouthAngle = Mathf.Rad2Deg * Mathf.Atan2 (facingDir.y, facingDir.x);
        if (jawLower != null && jawUpper != null) {
            jawLower.transform.rotation = Quaternion.Euler(0, 0, -openness + mouthAngle);
            jawUpper.transform.rotation = Quaternion.Euler (0, 0, openness +mouthAngle);
        }
    }

    float MouthOpenFunction (float x) {
        x = x * (mouthOpenDistance + chompDist)/mouthOpenDistance;
        x = Mathf.Clamp01 (x);
        float v1 = x;
        float v2 = 1-x*x*x*x;
        return Mathf.Lerp(v1,v2, x);
    }

    void EnableDisableThings (bool state) {
        if (jawUpper != null&& jawLower != null) {
            jawUpper.gameObject.SetActive (state);
            jawLower.gameObject.SetActive (state);
        }

        for (int i = 0; i < parts.Length; i++) {
            parts[i].SetActive (state);
        }
    }
}
