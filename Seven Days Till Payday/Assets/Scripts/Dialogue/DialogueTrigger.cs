using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Dialogue
    public Dialogue dialogue;

    // Dialogue Trigger
    private bool CanTriggerDialogue = false;
    private DialogueManager dialogue_manager;
    private void Start()
    {
        dialogue_manager = GetComponent<DialogueManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && dialogue_manager.dialoguebox_on == false)
        {
            dialogue_manager.StartDialogue(dialogue);
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
