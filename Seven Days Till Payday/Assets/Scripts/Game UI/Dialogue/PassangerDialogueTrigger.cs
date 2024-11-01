using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassangerDialogueTrigger : MonoBehaviour
{
    // Dialogue Data
    public Dialogue dialogue;

    // Dialogue UI
    private bool CanTriggerDialogue = false;
    private ConfusedPassangerUI confused_passenger_dialogue;
    private PriorityPassengerUI priority_passenger_dialogue;
    private DilemaPassengerUI dilema_passenger_dialogue;
    private DisabilityPassengerUI disability_passenger_dialogue;
    private DrunkPassengerUI drunk_passenger_dialogue;

    // Passenger References
    private ConfusedPassanger confused_passenger;
    private PriorityPassenger priority_passenger;
    private DilemaPassenger dilema_passenger;
    private DisabilityPassenger disability_passenger;
    private DrunkPassenger drunk_passenger;
    private void Start()
    {
        confused_passenger_dialogue = FindAnyObjectByType<ConfusedPassangerUI>();
        confused_passenger = GetComponent<ConfusedPassanger>();

        priority_passenger_dialogue = FindAnyObjectByType<PriorityPassengerUI>();
        priority_passenger = GetComponent<PriorityPassenger>();

        dilema_passenger_dialogue = FindAnyObjectByType<DilemaPassengerUI>();
        dilema_passenger = GetComponent<DilemaPassenger>();

        disability_passenger_dialogue = FindAnyObjectByType<DisabilityPassengerUI>();
        disability_passenger = GetComponent<DisabilityPassenger>();

        drunk_passenger_dialogue = FindAnyObjectByType<DrunkPassengerUI>();
        drunk_passenger = GetComponent<DrunkPassenger>();
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
        if(disability_passenger != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && !disability_passenger_dialogue.dialoguebox_on)
            {
                disability_passenger_dialogue.StartDialogue(dialogue , disability_passenger);
            }
        }
        if(drunk_passenger != null)
        {
            if(Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && !drunk_passenger_dialogue.dialoguebox_on)
            {
                drunk_passenger_dialogue.StartDialogue(dialogue, drunk_passenger);
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
