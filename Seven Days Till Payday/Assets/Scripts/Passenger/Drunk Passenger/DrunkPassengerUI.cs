using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrunkPassengerUI : MonoBehaviour
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

    // Drunk Passenger Dialogue Data
    private DrunkPassengerDialogue drunk_dialogue_data;

    // Drunk Passenger Minigame
    public GameObject drunk_options;
    private float angry_chance;
    private bool is_angry = false;
    private bool passenger_solved = false;

    // References
    private DrunkPassenger curr_passenger;
    public PlayerMovement player_movement;

    private void Start()
    {
        is_angry = false;
        passenger_solved = false;
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
    public void StartDialogue(Dialogue dialogue, DrunkPassenger passenger)
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
    private void StartDrunkDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Allow":
                foreach(string name in drunk_dialogue_data.allow_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.allow_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "SafeDeny":
                foreach(string name in drunk_dialogue_data.safe_deny_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.safe_deny_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "WrongDetain":
                foreach(string name in drunk_dialogue_data.wrong_detain_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.wrong_detain_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Angry":
                foreach(string name in drunk_dialogue_data.angry_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.angry_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DangerDeny":
                foreach(string name in drunk_dialogue_data.danger_deny_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.danger_deny_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "CorrectDetain":
                foreach(string name in drunk_dialogue_data.correct_detain_name)
                {
                    names.Enqueue(name);
                }
                foreach(string line in drunk_dialogue_data.correct_detain_line)
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

        if (curr_passenger != null && !passenger_solved)
        {
            ShowOptions();
        }
        else if (curr_passenger != null && passenger_solved)
        {
            passenger_solved = false;
            is_angry = false;
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowOptions()
    {
        drunk_options.SetActive(true);
        player_movement.DisableMovement();

        drunk_dialogue_data = curr_passenger.GetDrunkPassengerDialogue();
    }
    public void AllowButton()
    {
        drunk_options.SetActive(false);
        passenger_solved = true;

        StartCoroutine(AllowDialogueLoading());
    }
    private IEnumerator AllowDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("Allow");
    }
    public void DenyButton()
    {
        if (!is_angry)
        {
            angry_chance = Random.value;
            if (angry_chance <= 0.5f)
            {
                drunk_options.SetActive(false);
                is_angry = true;

                StartCoroutine(AngryDialogueLoading());
            }
            else
            {
                drunk_options.SetActive(false);
                passenger_solved = true;

                StartCoroutine(SafeDenyDialogueLoading());
            }
        }
        else
        {
            drunk_options.SetActive(false);
            passenger_solved = true;

            StartCoroutine(DangerDenyDialogueLoading());
        }
    }
    private IEnumerator AngryDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("Angry");
    }
    private IEnumerator SafeDenyDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("SafeDeny");
    }
    private IEnumerator DangerDenyDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("DangerDeny");
    }
    public void DetainButton()
    {
        if (!is_angry)
        {
            drunk_options.SetActive(false);
            passenger_solved = true;

            StartCoroutine(WrongDetainDialogueLoading());
        }
        else
        {
            drunk_options.SetActive(false);
            passenger_solved = true;

            StartCoroutine(CorrectDetainDialogueLoading());
        }
    }
    private IEnumerator WrongDetainDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("WrongDetain");
    }
    private IEnumerator CorrectDetainDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDrunkDialogue("CorrectDetain");
    }
}
