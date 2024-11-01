using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    // Dialogue
    public List<Dialogue> dialogues_daily;

    // Dialogue Trigger
    private bool can_dialogue;
    private DialogueManager dialogue_manager;

    // Time System
    public TimeSystem time_system;

    private void Awake()
    {
        dialogue_manager = GetComponent<DialogueManager>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && can_dialogue && !dialogue_manager.dialoguebox_on)
        {
            int curr_day = time_system.day;
            Debug.Log(dialogues_daily.Count);

            if (curr_day > 0 && curr_day <= dialogues_daily.Count)
            {
                Dialogue today_dialogue = dialogues_daily[curr_day - 1];
                dialogue_manager.StartDialogue(today_dialogue);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_dialogue = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_dialogue = false;
        }
    }
}
