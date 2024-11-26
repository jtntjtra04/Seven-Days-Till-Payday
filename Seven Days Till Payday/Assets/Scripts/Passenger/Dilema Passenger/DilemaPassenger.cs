using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilemaPassenger : MonoBehaviour
{
    private string passenger_status;
    private int passenger_type;

    public DilemaPassengerDialogue dilema_dialogue_data;

    private void OnDestroy()
    {
        DilemaPassengerUI dilemaPassengerUI = FindAnyObjectByType<DilemaPassengerUI>();

        if (dilemaPassengerUI != null)
        {
            dilemaPassengerUI.ClearCurrentMinigame();
        }
    }
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
        GetPassengerPosition();
    }
    public string GetPassengerStatus()
    {
        return passenger_status;
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
    public DilemaPassengerDialogue GetDilemaPassengerDialogue()
    {
        return dilema_dialogue_data;
    }
}
