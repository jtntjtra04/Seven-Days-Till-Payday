using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityPassenger : MonoBehaviour
{
    private string passenger_class;

    public PriorityPassengerDialogue priority_dialogue_data;

    private void Start()
    {
        if(gameObject.tag == "YoungPassenger")
        {
            passenger_class = "young";
        }
        else if(gameObject.tag == "OldPassenger")
        {
            passenger_class = "old";
        }
    }
    public string GetPassengerClass()
    {
        return passenger_class;
    }
    public PriorityPassengerDialogue GetPriorityPassengerDialogue()
    {
        return priority_dialogue_data;
    }
}
