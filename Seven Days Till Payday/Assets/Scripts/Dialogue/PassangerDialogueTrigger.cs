using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassangerDialogueTrigger : MonoBehaviour
{
    // Dialogue Data
    public Dialogue dialogue;

    // Dialogue Trigger
    private bool CanTriggerDialogue = false;
    private ConfusedPassangerUI passanger_dialogue;

    // References
    private ConfusedPassanger confused_passenger;
    private void Start()
    {
        passanger_dialogue = FindAnyObjectByType<ConfusedPassangerUI>();
        confused_passenger = GetComponent<ConfusedPassanger>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && passanger_dialogue.dialoguebox_on == false)
        {
            passanger_dialogue.StartDialogue(dialogue, confused_passenger);
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
