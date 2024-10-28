using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfusedPassanger : MonoBehaviour
{
    private int ticket_type;
    private int passenger_type;

    // References
    private PlayerMovement player_movement;

    private void Start()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
        ticket_type = Random.Range(0, 2);
        Debug.Log("Ticket Type : " + ticket_type);
        GetPassengerPosition();
    }
    public int GetTicketType()
    {
        return ticket_type;
    }
    private void GetPassengerPosition()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Metro")))
        {
            passenger_type = 0;
            Debug.Log("Passenger type = 0");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Commuter")))
        {
            passenger_type = 1;
            Debug.Log("Passenger type = 1");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Highspeed")))
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
