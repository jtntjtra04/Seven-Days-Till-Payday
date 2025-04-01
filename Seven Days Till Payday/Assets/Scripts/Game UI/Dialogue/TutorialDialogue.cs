using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    // Dialogue
    public List<Dialogue> dialogues_daily;
    public Dialogue ending_dialogue;

    // Dialogue Trigger
    private bool can_dialogue;
    private DialogueManager dialogue_manager;

    public bool game_ending = false;

    // Time System
    public TimeSystem time_system;

    // Icon
    [SerializeField] private GameObject visual_icon;

    // References
    private Tutorial tutorial;

    private void Awake()
    {
        dialogue_manager = GetComponent<DialogueManager>();
        tutorial = GetComponent<Tutorial>();
    }
    private void Start()
    {
        game_ending = false;
        visual_icon.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && can_dialogue && !dialogue_manager.dialoguebox_on)
        {
            int curr_day = time_system.day;

            if (curr_day > 0 && curr_day <= dialogues_daily.Count && tutorial.is_training)
            {
                Dialogue today_dialogue = dialogues_daily[curr_day - 1];
                dialogue_manager.StartDialogue(today_dialogue);
            }
            else if(curr_day <= 0)
            {
                game_ending = true;
                Dialogue today_dialogue = ending_dialogue;
                dialogue_manager.StartDialogue(today_dialogue);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_dialogue = true;
            visual_icon.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_dialogue = false;
            visual_icon.SetActive(false);
        }
    }
}
