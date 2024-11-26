using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineMainBody : MonoBehaviour{
    [SerializeField]private float vineGrowthTime;
    [SerializeField] private Sprite[] sprites = new Sprite[1];
    [SerializeField] private SpriteRenderer spriteRenderer;
    int numGrownVines;
    public float vineTime = 0;

    private VentVine[] vines;
    
    // Start is called before the first frame update
    void Start(){
        vineTime = vineGrowthTime;
        vines = FindObjectsOfType<VentVine> ();
        //Debug.Log ("debugWorks");
    }

    // Update is called once per frame
    void Update(){
        vineTime -= Time.deltaTime;

        // grows vines
        if (vineTime <= 0) {
            vineTime += vineGrowthTime;

            ChangeRandomVine (true);
        }

        numGrownVines = 0;
        for (int i = 0; i < vines.Length; i++) {
            if (vines[i].active) numGrownVines++;
        }

        int vineIndex = (int)MathF.Floor((sprites.Length)*(0.9f*numGrownVines / (float)vines.Length));
        spriteRenderer.sprite = sprites[vineIndex];
    }

    void OnPlayerInteract () {
        vineTime = vineGrowthTime;// kills all vines
        for (int i = 0; i < vines.Length; i++) {
            vines[i].active = false;
        }
    }

    // changes a random vine from grown to ungrown
    void ChangeRandomVine (bool activeValue) {
        //counts flippable objects
        int numVinesMS = 0;
        for (int i = 0; i < vines.Length; i++) {
            if (vines[i].active == activeValue) continue;
            numVinesMS++;
        }

        if (numVinesMS <= 0) return;
        // gets random objects
        int idx = UnityEngine.Random.Range (0, numVinesMS);
        //Debug.Log ($"chose {idx} out of {numVinesMS}");
        
        for (int i = 0; i < vines.Length; i++) {

            if (vines[i].active == activeValue) {
                idx++; // increases the index for every object before the index
            }
            else {
                if (i >= idx) break;// breaks once i reaches the index
            }
        }

        //Debug.Log ($"ended up with {idx}");
        

        if (idx < vines.Length) vines[idx].active = activeValue;
        //else Debug.Log ($"VentIDX {idx} is out of bounds");
    }
}
