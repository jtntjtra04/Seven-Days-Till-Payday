using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorPassengerUI : MonoBehaviour
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

    // Error Passenger Dialogue Data
    private ErrorPassengerDialogue error_dialogue_data;

    // Error Passenger Minigame
    public GameObject error_options;
    private bool problem_solved = false;

    // References
    private ErrorPassenger curr_passenger;
    public PlayerMovement player_movement;

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
    public void StartDialogue(Dialogue dialogue, ErrorPassenger passenger)
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
    private void StartErrorDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Allow":
                foreach (string name in error_dialogue_data.allow_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in error_dialogue_data.allow_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Deny":
                foreach (string name in error_dialogue_data.deny_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in error_dialogue_data.deny_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Detain":
                foreach (string name in error_dialogue_data.detain_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in error_dialogue_data.detain_line)
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
        Debug.Log("End dialogue");
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
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowOptions()
    {
        error_options.SetActive(true);
        player_movement.DisableMovement();
        problem_solved = true;

        error_dialogue_data = curr_passenger.GetErrorPassengerDialogue();
    }
    public void AllowButton()
    {
        error_options.SetActive(false);

        StartCoroutine(AllowDialogueLoading());
    }
    private IEnumerator AllowDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartErrorDialogue("Allow");
    }
    public void DenyButton()
    {
        error_options.SetActive(false);

        StartCoroutine(DenyDialogueLoading());
    }
    private IEnumerator DenyDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartErrorDialogue("Deny");
    }
    public void DetainButton()
    {
        error_options.SetActive(false);

        StartCoroutine(DetainDialogueLoading());
    }
    private IEnumerator DetainDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartErrorDialogue("Detain");
    }
}