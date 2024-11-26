using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BusinessmanPassengerUI : MonoBehaviour
{
    // Queue
    private Queue<string> lines;
    private Queue<string> names;

    // UI
    public GameObject dialogue_box;
    public TMP_Text name_text;
    public TMP_Text dialogue_text;

    // Dialogue System
    public float text_speed = 0.02f;
    private bool dialogue_on = false;
    public bool dialoguebox_on = false;

    // Businessman Passenger Dialogue Data
    private BusinessmanPassengerDialogue businessman_dialogue_data;

    // Businessman Passenger Minigame
    public GameObject businessman_options;
    private int passenger_type;
    private bool problem_solved = false;

    // References
    private BusinessmanPassenger curr_passenger;
    public PlayerMovement player_movement;
    public Tutorial tutorial;
    public GameController game_controller;

    private void Start()
    {
        problem_solved = false;
        lines = new Queue<string>();
        names = new Queue<string>();
    }
    private void Update()
    {
        if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space)) && dialoguebox_on)
        {
            if (dialogue_on)
            {
                text_speed = 0f;
            }
            else
            {
                NextDialogue();
            }
        }
    }
    public void StartDialogue(Dialogue dialogue, BusinessmanPassenger passenger)
    {
        curr_passenger = passenger;

        dialogue_box.SetActive(true);
        dialoguebox_on = true;
        player_movement.DisableMovement();
        //name_text.text = dialogue.name;
        //npc_image.sprite = dialogue.image;

        names.Clear();
        lines.Clear();

        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }
        foreach (string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        NextDialogue();
    }
    private void StartBusinessmanDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Approve":
                foreach (string name in businessman_dialogue_data.approve_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in businessman_dialogue_data.approve_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Deny":
                foreach (string name in businessman_dialogue_data.deny_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in businessman_dialogue_data.deny_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Detain":
                foreach (string name in businessman_dialogue_data.detain_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in businessman_dialogue_data.detain_line)
                {
                    lines.Enqueue(line);
                }
                break;
        }
        NextDialogue();
    }
    public void NextDialogue()
    {
        text_speed = 0.02f;
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        string name = names.Dequeue();
        string line = lines.Dequeue();

        dialogue_on = true;
        name_text.text = name;
        StopAllCoroutines();
        StartCoroutine(TypeLines(line));
    }
    private IEnumerator TypeLines(string sentence)
    {
        dialogue_text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogue_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }
        dialogue_on = false;
    }
    public void EndDialogue()
    {
        dialogue_box.SetActive(false);
        dialoguebox_on = false;
        player_movement.EnableMovement();

        if (curr_passenger != null && !problem_solved)
        {
            ShowOptions();
        }
        else if (curr_passenger != null && problem_solved)
        {
            problem_solved = false;
            if (tutorial != null && tutorial.is_training)
            {
                tutorial.MinusPassengerCount();
            }
            else
            {
                if (passenger_type == 0)
                {
                    game_controller.DecreasePassengerCount("MetroTrain");
                }
                else if (passenger_type == 1)
                {
                    game_controller.DecreasePassengerCount("CommuterTrain");
                }
                else if (passenger_type == 2)
                {
                    game_controller.DecreasePassengerCount("HighSpeedTrain");
                }
            }
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowOptions()
    {
        businessman_options.SetActive(true);
        player_movement.DisableMovement();
        problem_solved = true;

        passenger_type = curr_passenger.GetPassengerType();
        businessman_dialogue_data = curr_passenger.GetBusinessmanPassengerDialogue();
    }
    public void ApproveButton()
    {
        AudioManager.instance.PlaySFX("Click");
        businessman_options.SetActive(false);

        StartCoroutine(ApproveDialogueLoading());
    }
    private IEnumerator ApproveDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        MoneyAndReputation.Instance.AddMoney(100);
        MoneyAndReputation.Instance.AddReputation(50);
        AudioManager.instance.PlaySFX("Correct");
        StartBusinessmanDialogue("Approve");
    }
    public void DenyButton()
    {
        AudioManager.instance.PlaySFX("Click");
        businessman_options.SetActive(false);

        StartCoroutine(DenyDialogueLoading());
    }
    private IEnumerator DenyDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        MoneyAndReputation.Instance.MinusReputation(25);
        AudioManager.instance.PlaySFX("Wrong");
        StartBusinessmanDialogue("Deny");
    }
    public void DetainButton()
    {
        AudioManager.instance.PlaySFX("Click");
        businessman_options.SetActive(false);

        StartCoroutine(DetainDialogueLoading());
    }
    private IEnumerator DetainDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        MoneyAndReputation.Instance.MinusReputation(25);
        AudioManager.instance.PlaySFX("Wrong");
        StartBusinessmanDialogue("Detain");
    }
    public void ClearCurrentMinigame()
    {
        StopAllCoroutines();

        dialogue_box.SetActive(false);
        businessman_options.SetActive(false);

        dialogue_on = false;
        dialoguebox_on = false;
    }
}