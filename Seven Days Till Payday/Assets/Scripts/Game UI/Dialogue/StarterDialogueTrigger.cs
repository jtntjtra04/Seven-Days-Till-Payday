using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterDialogueTrigger : MonoBehaviour
{
    // Dialogue
    public Dialogue dialogue;

    // Dialogue Trigger
    private bool CanTriggerDialogue = false;
    private DialogueManager dialogue_manager;

    // References
    public Tutorial tutorial;
    public TimeSystem time_system;

    private void Start()
    {
        dialogue_manager = GetComponent<DialogueManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && dialogue_manager.dialoguebox_on == false && !tutorial.is_training)
        {
            if(time_system.day > 0)
            {
                dialogue_manager.StartDialogue(dialogue);
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
