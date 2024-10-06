using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorSwitch : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    [SerializeField] GameObject door;
    bool isOpen = false;
    BoxCollider2D collision;
    Animator doorAnimation;

    // Start is called before the first frame update
    void Start()
    {
        collision = door.GetComponent<BoxCollider2D>();
        doorAnimation = door.GetComponent<Animator>();
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
            //door.SetActive(true);
            doorAnimation.SetBool("Open", false);
        }
        else
        {
            isOpen = true;
            Debug.Log("door open: " + isOpen.ToString());
            collision.enabled = false;
            //door.SetActive(false);
            doorAnimation.SetBool("Open", true);
        }



        Debug.Log("interacted with " + gameObject.name);
    }
}
