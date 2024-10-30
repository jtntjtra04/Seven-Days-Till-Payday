using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassangerDialogueTrigger : MonoBehaviour
{
    // Dialogue Data
    public Dialogue dialogue;

    // Dialogue Trigger
    private bool CanTriggerDialogue = false;
    private ConfusedPassangerUI confused_passenger_dialogue;
    private PriorityPassengerUI priority_passenger_dialogue;
    private DilemaPassengerUI dilema_passenger_dialogue;

    // Passenger References
    private ConfusedPassanger confused_passenger;
    private PriorityPassenger priority_passenger;
    private DilemaPassenger dilema_passenger;
    private void Start()
    {
        confused_passenger_dialogue = FindAnyObjectByType<ConfusedPassangerUI>();
        confused_passenger = GetComponent<ConfusedPassanger>();

        priority_passenger_dialogue = FindAnyObjectByType<PriorityPassengerUI>();
        priority_passenger = GetComponent<PriorityPassenger>();

        dilema_passenger_dialogue = FindAnyObjectByType<DilemaPassengerUI>();
        dilema_passenger = GetComponent<DilemaPassenger>();
    }
    private void Update()
    {
        if(confused_passenger != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && !confused_passenger_dialogue.dialoguebox_on)
            {
                confused_passenger_dialogue.StartDialogue(dialogue, confused_passenger);
            }
        }
        if(priority_passenger != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && !priority_passenger_dialogue.dialoguebox_on)
            {
                priority_passenger_dialogue.StartDialogue(dialogue, priority_passenger);
            }
        }
        if(dilema_passenger != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && !dilema_passenger_dialogue.dialoguebox_on)
            {
                dilema_passenger_dialogue.StartDialogue(dialogue, dilema_passenger);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanTriggerDialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanTriggerDialogue = false;
        }
    }
}
