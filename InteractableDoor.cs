using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    bool isOpen = false;
    BoxCollider2D collision;
    Animator doorAnimation;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<BoxCollider2D>();
        doorAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerInteract()
    {
        
        if (isOpen)
        {
            isOpen = false;
            Debug.Log("door open: " + isOpen.ToString());
            collision.enabled = true;
            doorAnimation.SetBool("Open", false);
        }
        else 
        {
            isOpen = true;
            Debug.Log("door open: " + isOpen.ToString());
            collision.enabled = false;
            doorAnimation.SetBool("Open", true);
        }

        

        Debug.Log("interacted with " + gameObject.name);
    }
}
