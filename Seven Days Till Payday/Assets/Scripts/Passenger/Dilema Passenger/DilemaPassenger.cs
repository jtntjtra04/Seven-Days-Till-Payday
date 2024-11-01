using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilemaPassenger : MonoBehaviour
{
    private string passenger_status;

    public DilemaPassengerDialogue dilema_dialogue_data;

    private void Start()
    {
        if(gameObject.tag == "RichPassenger")
        {
            passenger_status = "rich";
        }
        else if (gameObject.tag == "StudentPassenger")
        {
            passenger_status = "student";
        }
    }
    public string GetPassengerStatus()
    {
        return passenger_status;
    }
    public DilemaPassengerDialogue GetDilemaPassengerDialogue()
    {
        return dilema_dialogue_data;
    }
}
