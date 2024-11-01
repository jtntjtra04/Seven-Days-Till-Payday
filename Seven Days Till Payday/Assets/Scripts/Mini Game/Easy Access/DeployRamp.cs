using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployRamp : MonoBehaviour
{
    public List<Image> arrow_images;
    public Sprite up_arrow_yellow, down_arrow_yellow, left_arrow_yellow, right_arrow_yellow;
    public Sprite up_arrow_green, down_arrow_green, left_arrow_green, right_arrow_green;
    public Sprite up_arrow_red, down_arrow_red, left_arrow_red, right_arrow_red;

    private List<KeyCode> arrow_sequence;
    private int curr_arrow_index;

    // References
    public DisabilityPassengerUI disability_passenger;
    private void OnEnable()
    {
        GenerateArrowSequence();
        DisplayArrows();
        curr_arrow_index = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleInput(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleInput(KeyCode.RightArrow);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleInput(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleInput(KeyCode.DownArrow);
        }
    }
    private void GenerateArrowSequence()
    {
        arrow_sequence = new List<KeyCode>();
        for(int i = 0; i < 5; i++)
        {
            int random_direction = Random.Range(0, 4);
            switch (random_direction)
            {
                case 0: arrow_sequence.Add(KeyCode.LeftArrow); break;
                case 1: arrow_sequence.Add(KeyCode.RightArrow); break;
                case 2: arrow_sequence.Add(KeyCode.UpArrow); break;
                case 3: arrow_sequence.Add(KeyCode.DownArrow); break;
            }
        }
    }
    private void DisplayArrows()
    {
        for(int i = 0; i < arrow_images.Count; i++)
        {
            arrow_images[i].sprite = GetArrowSprite(arrow_sequence[i], "Yellow");
        }
    }
    private Sprite GetArrowSprite(KeyCode arrow_key, string color)
    {
        switch (arrow_key)
        {
            case KeyCode.LeftArrow:
                return color == "Green" ? left_arrow_green : color == "Red" ? left_arrow_red : left_arrow_yellow;
            case KeyCode.RightArrow:
                return color == "Green" ? right_arrow_green : color == "Red" ? right_arrow_red : right_arrow_yellow;
            case KeyCode.UpArrow:
                return color == "Green" ? up_arrow_green : color == "Red" ? up_arrow_red : up_arrow_yellow;
            case KeyCode.DownArrow:
                return color == "Green" ? down_arrow_green : color == "Red" ? down_arrow_red : down_arrow_yellow;
            default:
                return null;
        }
    }
    private void HandleInput(KeyCode input_key)
    {
        if (input_key == arrow_sequence[curr_arrow_index])
        {
            // Correct
            arrow_images[curr_arrow_index].sprite = GetArrowSprite(arrow_sequence[curr_arrow_index], "Green");
            curr_arrow_index++;

            if(curr_arrow_index >= arrow_sequence.Count)
            {
                StartCoroutine(SequenceComplete());
            }
        }
        else
        {
            // Incorrect
            StartCoroutine(ResetSequence());
        }
    }
    private IEnumerator ResetSequence()
    {
        arrow_images[curr_arrow_index].sprite = GetArrowSprite(arrow_sequence[curr_arrow_index], "Red");

        yield return new WaitForSeconds(1f);

        // Reset arrows
        DisplayArrows();
        curr_arrow_index = 0;
    }
    private IEnumerator SequenceComplete()
    {
        Debug.Log("Sequence Complete");
        yield return new WaitForSeconds(1.5f);

        curr_arrow_index = 0;
        arrow_sequence.Clear();

        disability_passenger.OnCompleteMinigame();
    }
}
