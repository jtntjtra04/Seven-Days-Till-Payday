using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DilemaPassengerUI : MonoBehaviour
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

    // Dilema Passenger Dialogue Data
    private DilemaPassengerDialogue dilema_dialogue_data;

    // Dilema Passenger Minigame
    public GameObject dilema_options;
    private string passenger_status;
    private int passenger_type;
    private bool passenger_solved = false;

    // References
    private DilemaPassenger curr_passenger;
    public PlayerMovement player_movement;
    public Tutorial tutorial;
    public GameController game_controller;

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
    public void StartDialogue(Dialogue dialogue, DilemaPassenger passenger)
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
    private void StartDilemaDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "Accept":
                foreach (string name in dilema_dialogue_data.accept_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in dilema_dialogue_data.accept_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Deny":
                foreach (string name in dilema_dialogue_data.deny_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in dilema_dialogue_data.deny_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "Detain":
                foreach (string name in dilema_dialogue_data.detain_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in dilema_dialogue_data.detain_line)
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

        if (curr_passenger != null && !passenger_solved)
        {
            ShowOptions();
        }
        else if (curr_passenger != null && passenger_solved)
        {
            passenger_solved = false;
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
        dilema_options.SetActive(true);
        player_movement.DisableMovement();
        passenger_solved = true;

        passenger_status = curr_passenger.GetPassengerStatus();
        dilema_dialogue_data = curr_passenger.GetDilemaPassengerDialogue();
        passenger_type = curr_passenger.GetPassengerType();

        Debug.Log("Passenger Status : " + passenger_status);
    }
    public void AcceptButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        dilema_options.SetActive(false);

        StartCoroutine(AcceptButtonLoading());
    }
    private IEnumerator AcceptButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (passenger_status == "rich")
        {
            MoneyAndReputation.Instance.AddMoney(300);
            MoneyAndReputation.Instance.MinusReputation(60);
            AudioManager.instance.PlaySFX("PickMoney");
            StartDilemaDialogue("Accept");
        }
        else if(passenger_status == "student")
        {
            MoneyAndReputation.Instance.MinusMoney(500);
            MoneyAndReputation.Instance.AddReputation(100);
            AudioManager.instance.PlaySFX("Buy");
            StartDilemaDialogue("Accept");
        }
    }
    public void DenyButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        dilema_options.SetActive(false);

        StartCoroutine(DenyButtonLoading());
    }
    private IEnumerator DenyButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (passenger_status == "rich")
        {
            MoneyAndReputation.Instance.AddReputation(50);
            AudioManager.instance.PlaySFX("Correct");
            StartDilemaDialogue("Deny");
        }
        else if(passenger_status == "student")
        {
            MoneyAndReputation.Instance.MinusReputation(25);
            AudioManager.instance.PlaySFX("Wrong");
            StartDilemaDialogue("Deny");
        }
    }
    public void DetainButton()
    {
        AudioManager.instance.PlaySFX("Click");
        dialogue_box.SetActive(false);
        dilema_options.SetActive(false);

        StartCoroutine(DetainButtonLoading());
    }
    private IEnumerator DetainButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        MoneyAndReputation.Instance.MinusReputation(40);
        AudioManager.instance.PlaySFX("Wrong");
        StartDilemaDialogue("Detain");
    }
    public void ClearCurrentMinigame()
    {
        StopAllCoroutines();

        dialogue_box.SetActive(false);
        dilema_options.SetActive(false);

        dialogue_on = false;
        dialoguebox_on = false;
    }
}
