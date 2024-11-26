using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessmanPassenger : MonoBehaviour
{
    private int passenger_type;

    public BusinessmanPassengerDialogue businessman_dialogue_data;

    private void OnDestroy()
    {
        BusinessmanPassengerUI businessmanPassengerUI = FindAnyObjectByType<BusinessmanPassengerUI>();
        if (businessmanPassengerUI != null)
        {
            businessmanPassengerUI.ClearCurrentMinigame();
        }
    }
    private void Start()
    {
        GetPassengerPosition();
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
    public BusinessmanPassengerDialogue GetBusinessmanPassengerDialogue()
    {
        return businessman_dialogue_data;
    }
}
