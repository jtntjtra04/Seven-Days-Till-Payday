using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfusedPassanger : MonoBehaviour
{
    private int ticket_type;
    private int passenger_type;

    private void OnDestroy()
    {
        ConfusedPassangerUI confusedPassangerUI = FindAnyObjectByType<ConfusedPassangerUI>();

        if( confusedPassangerUI != null)
        {
            confusedPassangerUI.ClearCurrentMinigame();
        }
    }

    private void Start()
    {
        ticket_type = Random.Range(0, 3);
        Debug.Log("Ticket Type : " + ticket_type);
        GetPassengerPosition();
    }
    private void Update()
    {
        GetPassengerPosition();
    }
    public int GetTicketType()
    {
        return ticket_type;
    }
    private void GetPassengerPosition()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Metro")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("MetroInterior")))
        {
            passenger_type = 0;
            Debug.Log("Passenger type = 0");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Commuter")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("CommuterInterior")))
        {
            passenger_type = 1;
            Debug.Log("Passenger type = 1");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Highspeed")) || Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("HighspeedInterior")))
        {
            passenger_type = 2;
            Debug.Log("Passenger type = 2");
        }
    }
    public int GetPassengerType()
    {
        return passenger_type;
    }

}
