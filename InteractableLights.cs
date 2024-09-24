using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLights : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    bool isOn = false;
    //bool isReleased = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (gameInput.GetInteractionRelease())
        //{
        //    isReleased = true;
        //    Debug.Log(isReleased.ToString());
        //}
    }

    void OnPlayerInteract()
    {

        if (isOn)
        {
            
                isOn = false;
                Debug.Log("light on: " + isOn.ToString());
                //isReleased = false;
                gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255);

            
        }
        else
        {
                isOn = true;
                Debug.Log("light on: " + isOn.ToString());
                //isReleased = false;
                gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 0);
            
        }

        

        //Debug.Log("interacted with " + gameObject.name);
    }
}
