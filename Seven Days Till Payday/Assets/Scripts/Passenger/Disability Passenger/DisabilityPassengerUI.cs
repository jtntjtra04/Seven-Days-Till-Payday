using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisabilityPassengerUI : MonoBehaviour
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

    // Disability Passenger Dialogue Data
    private DisabilityPassengerDialogue disability_dialogue_data;

    // Easy Access Minigame
    public GameObject deploy_ramp;
    private bool passenger_solved = false;

    // References
    private DisabilityPassenger curr_passenger;
    public PlayerMovement player_movement;

    private void Start()
    {
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
    public void StartDialogue(Dialogue dialogue, DisabilityPassenger passenger)
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
    private void StartDisabilityDialogue()
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        foreach(string name in disability_dialogue_data.succeed_name)
        {
            names.Enqueue(name);
        }
        foreach(string line in disability_dialogue_data.succeed_line)
        {
            lines.Enqueue(line);
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
            ShowDeployRamp();
        }
        else if (curr_passenger != null && passenger_solved)
        {
            passenger_solved = false;
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowDeployRamp()
    {
        deploy_ramp.SetActive(true);
        player_movement.DisableMovement();
        passenger_solved = true;

        disability_dialogue_data = curr_passenger.GetDisabilityPassengerDialogue();
    }
    public void OnCompleteMinigame()
    {
        deploy_ramp.SetActive(false);

        StartCoroutine(EndMinigameLoading());
    }
    private IEnumerator EndMinigameLoading()
    {
        yield return new WaitForSeconds(0.5f);

        StartDisabilityDialogue();
    }
}
