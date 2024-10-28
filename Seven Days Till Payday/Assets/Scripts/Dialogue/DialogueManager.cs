using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Queue
    private Queue<string> lines;
    private Queue<string> names;

    // UI
    public GameObject dialogue_box;
    public TMP_Text name_text;
    public TMP_Text dialogue_text;

    public float text_speed = 0.02f;
    private bool dialogue_on = false;
    public bool dialoguebox_on = false;

    private PlayerMovement player_movement;

    private void Awake()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
    }
    private void Start()
    {
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
            text_speed = 0f;
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
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

        Tutorial tutorial = GetComponent<Tutorial>();
        if (tutorial != null)
        {
            if (!tutorial.already_tutor)
            {
                tutorial.ShowTutorial();
            }
            else
            {
                tutorial.SetAlreadyTutorialFalse();
            }
        }

        GameStarter gameStarter = GetComponent<GameStarter>();
        if (gameStarter != null)
        {
            gameStarter.ShowConfirmation();
        }
    }
}
