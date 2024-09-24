using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorSwitch : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    [SerializeField] GameObject door;
    bool isOpen = false;
    BoxCollider2D collision;

    // Start is called before the first frame update
    void Start()
    {
        collision = door.GetComponent<BoxCollider2D>();
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
        }
        else
        {
            isOpen = true;
            Debug.Log("door open: " + isOpen.ToString());
            collision.enabled = false;
        }



        Debug.Log("interacted with " + gameObject.name);
    }
}
