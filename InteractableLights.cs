using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLights : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    public GameObject light;
    bool isOn = false;

    void OnPlayerInteract()
    {
        isOn = !isOn;

        if (isOn)
        {
            light.SetActive(false);
        }
        else
        {
            light.SetActive(true);

        }
    }
}
