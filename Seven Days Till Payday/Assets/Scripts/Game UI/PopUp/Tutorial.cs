using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial_panel;
    public Sprite[] tutorial_sprites;
    public Image tutorial_image;
    private int curr_page;
    public bool already_tutor = false;

    // Dialogue
    public List<Dialogue> training_dialogues;

    // Day 7 Tutorial
    [Header("Day 7 Tutorial")]
    public GameObject confused_passanger;
    public GameObject priority_passanger;
    public GameObject dirt;
    public Transform confused_passanger_spawn;
    public Transform dirt_spawn;
    public Transform priority_passanger_spawn;

    // Day 6 Tutorial
    [Header("Day 6 Tutorial")]
    public GameObject rich_passanger;
    public GameObject student_passanger;
    public Transform rich_passanger_spawn;
    public Transform student_passanger_spawn;

    // Day 5 Tutorial
    [Header("Day 5 Tutorial")]
    public GameObject disability_passanger;
    public GameObject drunk_passanger;
    public Transform disability_passanger_spawn;
    public Transform drunk_passanger_spawn;

    // Day 4 Tutorial
    [Header("Day 4 Tutorial")]
    public GameObject error_passanger;
    public Transform error_passanger_spawn;

    // Day 3 Tutorial
    [Header("Day 3 Tutorial")]
    public GameObject foreign_passanger;
    public GameObject bigbag_passanger;
    public Transform foreign_passanger_spawn;
    public Transform bigbag_passanger_spawn;

    // References
    [Header("References")]
    public PlayerMovement player_movement;
    public TimeSystem time_system;
    private DialogueManager dialogue_manager;

    private void Awake()
    {
        dialogue_manager = GetComponent<DialogueManager>();
    }
    private void Start()
    {
        already_tutor = false;
        tutorial_panel.SetActive(false);
    }
    public void ShowTutorial()
    {
        tutorial_panel.SetActive(true);
        if (tutorial_sprites != null)
        {
            curr_page = 0;
            tutorial_image.sprite = tutorial_sprites[curr_page];
        }
        player_movement.DisableMovement();
    }
    public void CloseTutorial()
    {
        tutorial_panel.SetActive(false);
        player_movement.EnableMovement();
        ShowTrainingDialogue();
    }
    private void ShowTrainingDialogue()
    {
        int curr_day = time_system.day;

        if(curr_day > 0 && curr_day <= training_dialogues.Count)
        {
            Dialogue today_showcase_dialogue = training_dialogues[curr_day - 1];
            already_tutor = true;
            dialogue_manager.StartDialogue(today_showcase_dialogue);
        }
    }
    public void SetAlreadyTutorialFalse()
    {
        already_tutor = false;
    }
    public void SpawnTutorialPassanger(int day)
    {
        if (day == 7)
        {
            Instantiate(confused_passanger, confused_passanger_spawn);
            Instantiate(priority_passanger, priority_passanger_spawn);
            Instantiate(dirt, dirt_spawn);
        }
        else if (day == 6)
        {
            Instantiate(rich_passanger, rich_passanger_spawn);
            Instantiate(student_passanger, student_passanger_spawn);
        }
        else if (day == 5)
        {
            Instantiate(disability_passanger, disability_passanger_spawn);
            Instantiate(drunk_passanger, drunk_passanger_spawn);
        }
        else if (day == 4)
        {
            Instantiate(error_passanger, error_passanger_spawn);
        }
        else if (day == 3)
        {
            Instantiate(foreign_passanger, foreign_passanger_spawn);
            Instantiate(bigbag_passanger, bigbag_passanger_spawn);
        }
    }
}
