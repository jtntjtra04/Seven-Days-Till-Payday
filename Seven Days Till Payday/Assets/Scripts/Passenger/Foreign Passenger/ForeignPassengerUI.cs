using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForeignPassengerUI : MonoBehaviour
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

    // Foreign Passenger Dialogue Data
    private ForeignPassengerDialogue foreign_dialogue_data;

    // Foreign Passenger Minigame
    public GameObject foreign_options;
    public GameObject translator_button;
    public GameObject translator_panel;
    public Slider translator_slider; // Slider to change text based on the slider value
    public TMP_Text line_text; // Text in UI scene that will change
    private string initial_line; // to store the initial line which is still randomized
    private string correct_line; // to store the correct final line
    private int question_type;
    private bool problem_solved = false;

    // Slider Mechanic System
    private int correct_min_range; // Minimum correct slider range
    private int correct_max_range; // Maximum correct slider range
    private string random_line_11_40;
    private string random_line_41_70;
    private string random_line_71_100;

    // References
    private ForeignPassenger curr_passenger;
    public PlayerMovement player_movement;
    public Tutorial tutorial;
    public GameController game_controller;

    private void Start()
    {
        translator_panel.SetActive(false);
        translator_slider.value = 0f;
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
        CheckSliderValue();
    }
    public void StartDialogue(string name, string line, ForeignPassenger passenger)
    {
        curr_passenger = passenger;
        initial_line = line;

        dialogue_box.SetActive(true);
        dialoguebox_on = true;
        player_movement.DisableMovement();
        //name_text.text = dialogue.name;
        //npc_image.sprite = dialogue.image;

        names.Clear();
        lines.Clear();

        names.Enqueue(name);
        lines.Enqueue(line);

        NextDialogue();
    }
    private void StartForeignDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "AllowTrain":
                foreach (string name in foreign_dialogue_data.allow_train_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.allow_train_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "AllowFruit":
                foreach (string name in foreign_dialogue_data.allow_fruit_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.allow_fruit_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "AllowSus":
                foreach (string name in foreign_dialogue_data.allow_sus_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.allow_sus_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DenyTrain":
                foreach (string name in foreign_dialogue_data.deny_train_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.deny_train_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DenyFruit":
                foreach (string name in foreign_dialogue_data.deny_fruit_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.deny_fruit_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DenySus":
                foreach (string name in foreign_dialogue_data.deny_sus_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.deny_sus_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DetainTrain":
                foreach (string name in foreign_dialogue_data.detain_train_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.detain_train_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DetainFruit":
                foreach (string name in foreign_dialogue_data.detain_fruit_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.detain_fruit_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DetainSus":
                foreach (string name in foreign_dialogue_data.detain_sus_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in foreign_dialogue_data.detain_sus_line)
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
            ShowTranslatorButton();
        }
        else if (curr_passenger != null && problem_solved)
        {
            problem_solved = false;
            translator_slider.value = 0f;
            if (tutorial != null && tutorial.is_training)
            {
                tutorial.MinusPassengerCount();
            }
            else
            {
                game_controller.DecreasePassengerCount("HighSpeedTrain");
            }
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowTranslatorButton()
    {
        translator_button.SetActive(true);
        foreign_options.SetActive(true);
        player_movement.DisableMovement();
        problem_solved = true;

        question_type = curr_passenger.GetQuestionType();
        foreign_dialogue_data = curr_passenger.GetForeignPassengerDialogue();

        switch (question_type) // To get the correct line based on the question type
        {
            case 1:
                correct_line = "Am i on the right train?";
                break;
            case 2:
                correct_line = "Can i bring this durian fruit on the train?";
                break;
            case 3:
                correct_line = "Yo, do you wanna buy some of the good stuff?";
                break;
        }
        // Randomize the range slider value of the correct line
        correct_min_range = Random.Range(1, 91);
        correct_max_range = correct_min_range + 9;
        Debug.Log("Min range = " + correct_min_range + " Max range = " + correct_max_range);

        // Generate random text for every range 1-30, 31-60, 61-90
        random_line_11_40 = curr_passenger.GenerateRandomSentences(5, 3, 8);
        random_line_41_70 = curr_passenger.GenerateRandomSentences(5, 3, 8);
        random_line_71_100 = curr_passenger.GenerateRandomSentences(5, 3, 8);

        // Show initial line in display
        line_text.text = initial_line;
        Debug.Log(line_text.text);
    }
    public void OpenTranslator()
    {
        translator_panel.SetActive(true);
    }
    public void CloseTranslator()
    {
        translator_panel.SetActive(false);
    }
    private void CheckSliderValue()
    {
        int slider_value = (int)translator_slider.value;

        if (slider_value >= correct_min_range && slider_value <= correct_max_range)
        {
            line_text.text = correct_line; // show correct line
        }
        else
        {
            if(slider_value >= 11 && slider_value <= 40)
            {
                line_text.text = random_line_11_40;
            }
            else if(slider_value >= 41 && slider_value <= 70)
            {
                line_text.text = random_line_41_70;
            }
            else if(slider_value >= 71 && slider_value <= 100)
            {
                line_text.text = random_line_71_100;
            }
            else
            {
                line_text.text = initial_line; // reset
            }
        }
    }
    public void AllowButton()
    {
        AudioManager.instance.PlaySFX("Click");
        translator_button.SetActive(false);
        foreign_options.SetActive(false);

        StartCoroutine(AllowDialogueLoading());
    }
    private IEnumerator AllowDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if(question_type == 1)
        {
            MoneyAndReputation.Instance.AddReputation(75);
            AudioManager.instance.PlaySFX("Correct");
            StartForeignDialogue("AllowTrain");
        }
        else if(question_type == 2)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("AllowFruit");
        }
        else if(question_type == 3)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("AllowSus");
        }
    }
    public void DenyButton()
    {
        AudioManager.instance.PlaySFX("Click");
        translator_button.SetActive(false);
        foreign_options.SetActive(false);

        StartCoroutine(DenyDialogueLoading());
    }
    private IEnumerator DenyDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (question_type == 1)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("DenyTrain");
        }
        else if (question_type == 2)
        {
            MoneyAndReputation.Instance.AddReputation(75);
            AudioManager.instance.PlaySFX("Correct");
            StartForeignDialogue("DenyFruit");
        }
        else if (question_type == 3)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("DenySus");
        }
    }
    public void DetainButton()
    {
        translator_button.SetActive(false);
        foreign_options.SetActive(false);

        StartCoroutine(DetainDialogueLoading());
    }
    private IEnumerator DetainDialogueLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (question_type == 1)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("DetainTrain");
        }
        else if (question_type == 2)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartForeignDialogue("DetainFruit");
        }
        else if (question_type == 3)
        {
            MoneyAndReputation.Instance.AddReputation(75);
            AudioManager.instance.PlaySFX("Correct");
            StartForeignDialogue("DetainSus");
        }
    }
    public void ClearCurrentMinigame()
    {
        StopAllCoroutines();

        dialogue_box.SetActive(false);
        translator_button.SetActive(false);
        translator_panel.SetActive(false);
        foreign_options.SetActive(false);

        dialogue_on = false;
        dialoguebox_on = false;
        translator_slider.value = 0f;
    }
}
