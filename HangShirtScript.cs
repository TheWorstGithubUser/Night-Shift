using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangShirtScript : MonoBehaviour
{
    [SerializeField] TaskScript task;

    void OnPlayerInteract()
    {
        task.complete = true;
        task.IsComplete();
    }
}
