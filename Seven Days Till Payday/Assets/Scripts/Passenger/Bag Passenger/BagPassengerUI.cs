using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagPassengerUI : MonoBehaviour
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

    // Bag Passenger Dialogue Data
    private BagPassengerDialogue bag_dialogue_data;

    // Bag Check Minigame
    public GameObject bag_panel;
    public RectTransform bag;
    private bool passenger_danger;
    private bool problem_solved = false;

    // Bag Items
    public GameObject[] safe_items;
    public GameObject[] dangerous_items;
    private List<GameObject> spawned_items = new List<GameObject>();

    // Spawn Area
    public Vector2 min_area = new Vector2(-0.5f, -0.5f);
    public Vector2 max_area = new Vector2(0.5f, 0.5f);

    // References
    private BagPassenger curr_passenger;
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
    public void StartDialogue(Dialogue dialogue, BagPassenger passenger)
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
    private void StartBagDialogue(string decision)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        names.Clear();
        lines.Clear();

        switch (decision)
        {
            case "AllowSafe":
                foreach (string name in bag_dialogue_data.allow_safe_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in bag_dialogue_data.allow_safe_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "AllowDanger":
                foreach (string name in bag_dialogue_data.allow_danger_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in bag_dialogue_data.allow_danger_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DetainSafe":
                foreach (string name in bag_dialogue_data.detain_safe_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in bag_dialogue_data.detain_safe_line)
                {
                    lines.Enqueue(line);
                }
                break;
            case "DetainDanger":
                foreach (string name in bag_dialogue_data.detain_danger_name)
                {
                    names.Enqueue(name);
                }
                foreach (string line in bag_dialogue_data.detain_danger_line)
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
            ShowBag();
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
                game_controller.DecreasePassengerCount("HighSpeedTrain");
            }
            Destroy(curr_passenger.gameObject);
        }
    }
    private void ShowBag()
    {
        AudioManager.instance.PlaySFX("Backpack");
        bag_panel.SetActive(true);
        player_movement.DisableMovement();
        problem_solved = true;

        passenger_danger = curr_passenger.IsPassengerDanger();
        bag_dialogue_data = curr_passenger.GetBagPassengerDialogue();

        Debug.Log("Passenger danger : " + passenger_danger);

        SpawnItems();
    }
    private void SpawnItems()
    {
        int max_item = 12;

        List<GameObject> items = new List<GameObject>(safe_items);

        if (passenger_danger)
        {
            int danger_index = Random.Range(0, max_item);
            items[danger_index] = dangerous_items[Random.Range(0, dangerous_items.Length)];
        }
        for (int i = 0; i < max_item; i++)
        {
            GameObject item_to_spawn = items[i];
            Vector3 random_position = GetPositionInsideBag();

            GameObject spawned_item = Instantiate(item_to_spawn, random_position, Quaternion.identity, bag);
            spawned_items.Add(spawned_item);
        }
    }
    private Vector3 GetPositionInsideBag()
    {
        float half_width = bag.rect.width / 2;
        float half_height = bag.rect.height / 2;

        float random_x = Random.Range(min_area.x, max_area.x) * half_width;
        float random_y = Random.Range(min_area.y, max_area.y) * half_height;

        return bag.transform.TransformPoint(new Vector3(random_x, random_y, 0f));
    }
    private void OnDrawGizmos()
    {
        if(bag != null)
        {
            Gizmos.color = Color.green;

            Vector3 area_min = bag.transform.TransformPoint(new Vector3(min_area.x * bag.rect.width / 2, min_area.y * bag.rect.height / 2, 0));
            Vector3 area_max = bag.transform.TransformPoint(new Vector3(max_area.x * bag.rect.width / 2, max_area.y * bag.rect.height / 2, 0));

            // Draw wire cube
            Vector3 center = (area_min + area_max) / 2;
            Vector3 size = new Vector3(max_area.x - min_area.x, max_area.y - min_area.y, 0f);
            Gizmos.DrawWireCube(center, size);
        }
    }
    private void CloseBag()
    {
        // Mechanic to destroy all the spawned items
        foreach(GameObject item in spawned_items)
        {
            Destroy(item);
        }
        spawned_items.Clear();

        bag_panel.SetActive(false);
    }
    public void AllowButton()
    {
        AudioManager.instance.PlaySFX("Click");
        CloseBag();

        StartCoroutine(AllowButtonLoading());
    }
    private IEnumerator AllowButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (!passenger_danger)
        {
            MoneyAndReputation.Instance.AddReputation(75);
            AudioManager.instance.PlaySFX("Correct");
            StartBagDialogue("AllowSafe");
        }
        else
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartBagDialogue("AllowDanger");
        }
    }
    public void DetainButton()
    {
        AudioManager.instance.PlaySFX("Click");
        CloseBag();

        StartCoroutine(DetainButtonLoading());
    }
    private IEnumerator DetainButtonLoading()
    {
        yield return new WaitForSeconds(0.5f);

        if (!passenger_danger)
        {
            MoneyAndReputation.Instance.MinusReputation(40);
            AudioManager.instance.PlaySFX("Wrong");
            StartBagDialogue("DetainSafe");
        }
        else
        {
            MoneyAndReputation.Instance.AddReputation(75);
            AudioManager.instance.PlaySFX("Correct");
            StartBagDialogue("DetainDanger");
        }
    }
    public void ClearCurrentMinigame()
    {
        StopAllCoroutines();

        dialogue_box.SetActive(false);
        CloseBag();

        dialogue_on = false;
        dialoguebox_on = false;
    }
}
