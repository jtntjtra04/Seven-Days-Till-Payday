using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityPassenger : MonoBehaviour
{
    private string passenger_class;
    private int passenger_type;

    public PriorityPassengerDialogue priority_dialogue_data;

    private void OnDestroy()
    {
        PriorityPassengerUI priorityPassengerUI = FindAnyObjectByType<PriorityPassengerUI>();

        if(priorityPassengerUI != null)
        {
            priorityPassengerUI.ClearCurrentMinigame();
        }
    }

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
        GetPassengerPosition();
    }
    public string GetPassengerClass()
    {
        return passenger_class;
    }
    private void GetPassengerPosition()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Metro")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("MetroInterior")))
        {
            passenger_type = 0;
            Debug.Log("Passenger type = 0");
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Commuter")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("CommuterInterior")))
        {
            passenger_type = 1;
            Debug.Log("Passenger type = 1");
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Highspeed")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("HighspeedInterior")))
        {
            passenger_type = 2;
            Debug.Log("Passenger type = 2");
        }
    }
    public int GetPassengerType()
    {
        return passenger_type;
    }
    public PriorityPassengerDialogue GetPriorityPassengerDialogue()
    {
        return priority_dialogue_data;
    }
}
