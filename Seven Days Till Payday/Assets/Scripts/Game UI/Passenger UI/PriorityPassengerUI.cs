using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriorityPassengerUI : MonoBehaviour
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

    // Priority Passenger Dialogue Data
    private PriorityPassengerDialogue priority_dialogue_data;

    // Priority Passenger Minigame
    public GameObject priority_options;
    private string passenger_class;
    private bool problem_solved = false;

    // References
    private PriorityPassenger curr_passenger;
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
            if (!dialogue_on)
            {
                NextDialogue();
                return;
            }
            if (dialogue_on)
            {
                text_speed = 0f;
            }
        }
    }
    public void StartDialogue(Dialogue dialogue, PriorityPassenger passenger)
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
    private void StartPriorityDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Correct":
                foreach (string name in priority_dialogue_data.correct_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in priority_dialogue_data.correct_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Wrong":
                foreach (string name in priority_dialogue_data.wrong_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in priority_dialogue_data.wrong_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Detain":
                foreach (string name in priority_dialogue_data.detain_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in priority_dialogue_data.detain_line)
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
        priority_options.SetActive(true);
        player_movement.DisableMovement();
        problem_solved = true;

        passenger_class = curr_passenger.GetPassengerClass();
        priority_dialogue_data = curr_passenger.GetPriorityPassengerDialogue();

        Debug.Log("Passenger Class : " + passenger_class);
    }
    public void AllowSitButton()
    {
        priority_options.SetActive(false);

        StartCoroutine(AllowButtonLoading());
    }
    private IEnumerator AllowButtonLoading()
    {
        yield return new WaitForSeconds(1f);

        if (passenger_class == "old")
        {
            StartPriorityDialogue("Correct");
        }
        else
        {
            StartPriorityDialogue("Wrong");
        }
    }
    public void DenySitButton()
    {
        priority_options.SetActive(false);

        StartCoroutine(DenyButtonLoading());
    }
    private IEnumerator DenyButtonLoading()
    {
        yield return new WaitForSeconds(1f);

        if (passenger_class == "young")
        {
            StartPriorityDialogue("Correct");
        }
        else
        {
            StartPriorityDialogue("Wrong");
        }
    }
    public void DetainButton()
    {
        priority_options.SetActive(false);

        StartCoroutine(DetainButtonLoading());
    }
    private IEnumerator DetainButtonLoading()
    {
        yield return new WaitForSeconds(1f);

        StartPriorityDialogue("Detain");
    }
}