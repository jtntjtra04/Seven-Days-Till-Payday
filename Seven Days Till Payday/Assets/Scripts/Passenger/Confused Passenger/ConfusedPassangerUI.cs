using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfusedPassangerUI : MonoBehaviour
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

    // Check Ticket Minigame
    public GameObject ticket_check_button;
    public GameObject options;
    public Image ticket_image;
    public Sprite[] ticket_sprite;
    private int ticket_type;
    private int passenger_type;
    private bool ticket_showed = false;
    private bool ticket_checked = false;

    // Judgement Dialogue
    public ConfusedPassengerDialogue confused_dialogue_data;

    // References
    private ConfusedPassanger curr_passenger;
    public PlayerMovement player_movement;
    public Tutorial tutorial;
    public GameController game_controller;

    private void Start()
    {
        ticket_checked = false;
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
    public void StartDialogue(Dialogue dialogue, ConfusedPassanger passenger)
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
    private void StartConfusedDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Allow":
                foreach (string name in confused_dialogue_data.correct_ticket_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in confused_dialogue_data.correct_ticket_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Deny":
                foreach (string name in confused_dialogue_data.wrong_ticket_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in confused_dialogue_data.wrong_ticket_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Detain":
                foreach (string name in confused_dialogue_data.detained_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in confused_dialogue_data.detained_line)
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
        dialoguebox_on = false;
        player_movement.EnableMovement();

        if(curr_passenger != null && !ticket_checked)
        {
            ShowTicketCheckButton();
        }
        else if(curr_passenger != null && ticket_checked)
        {
            ticket_checked = false;
            if (tutorial != null && tutorial.is_training)
            {
                tutorial.MinusPassengerCount();
            }
            else
            {
                if(passenger_type == 0)
                {
                    game_controller.DecreasePassengerCount("MetroTrain");
                }
                else if(passenger_type == 1)
                {
                    game_controller.DecreasePassengerCount("CommuterTrain");
                }
                else if(passenger_type == 2)
                {
                    game_controller.DecreasePassengerCount("HighSpeedTrain");
                }
            }
            Destroy(curr_passenger.gameObject);
            
        }
    }
    private void ShowTicketCheckButton()
    {
        ticket_check_button.SetActive(true);
        options.SetActive(true);
        player_movement.DisableMovement();
        ticket_checked = true;

        ticket_type = curr_passenger.GetTicketType();
        passenger_type = curr_passenger.GetPassengerType();

        Debug.Log("ticket : " + ticket_type);
        Debug.Log("passenger" + passenger_type);
    }
    public void ShowTicket()
    {
        AudioManager.instance.PlaySFX("Click");
        if (!ticket_showed)
        {
            ticket_image.gameObject.SetActive(true);
            ticket_image.sprite = ticket_sprite[ticket_type];
            ticket_showed = true;
        }
        else
        {
            ticket_image.gameObject.SetActive(false);
            ticket_showed = false;
        }
        
    }
    public void AllowButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        ticket_image.gameObject.SetActive(false);
        ticket_check_button.SetActive(false);
        options.SetActive(false);

        StartCoroutine(AllowButtonLoading());
    }
    private IEnumerator AllowButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (ticket_type == passenger_type)
        {
            MoneyAndReputation.Instance.AddReputation(50);
            AudioManager.instance.PlaySFX("Correct");
            StartConfusedDialogue("Allow");
        }
        else
        {
            MoneyAndReputation.Instance.MinusReputation(25);
            AudioManager.instance.PlaySFX("Wrong");
            StartConfusedDialogue("Allow");
        }
    }
    public void DenyButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        ticket_image.gameObject.SetActive(false);
        ticket_check_button.SetActive(false);
        options.SetActive(false);

        StartCoroutine(DenyButtonLoading());
    }
    private IEnumerator DenyButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (ticket_type != passenger_type)
        {
            MoneyAndReputation.Instance.AddReputation(50);
            AudioManager.instance.PlaySFX("Correct");
            StartConfusedDialogue("Deny");
        }
        else
        {
            MoneyAndReputation.Instance.MinusReputation(25);
            AudioManager.instance.PlaySFX("Wrong");
            StartConfusedDialogue("Deny");
        }
    }
    public void DetainButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        ticket_image.gameObject.SetActive(false);
        ticket_check_button.SetActive(false);
        options.SetActive(false);

        StartCoroutine(DetainButtonLoading());
    }
    private IEnumerator DetainButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        MoneyAndReputation.Instance.MinusReputation(25);
        AudioManager.instance.PlaySFX("Wrong");
        StartConfusedDialogue("Detain");
    }
    public void ClearCurrentMinigame()
    {
        StopAllCoroutines();

        dialogue_box.SetActive(false);
        ticket_check_button.SetActive(false);
        options.SetActive(false);
        ticket_image.gameObject.SetActive(false);

        dialogue_on = false;
        dialoguebox_on = false;
        ticket_showed = false;
    }
}
