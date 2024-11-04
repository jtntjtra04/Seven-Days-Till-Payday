using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeignPassenger : MonoBehaviour
{
    // Start Dialogue
    private const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // End Dialogue
    public ForeignPassengerDialogue foreign_dialogue_data;

    private int question_type;

    private void Start()
    {
        question_type = Random.Range(1, 3);
        Debug.Log("Question Type : " +  question_type);
    }

    public string GenerateRandomSentences(int word_count, int min_word_length, int max_word_length)
    {
        List<string> words = new List<string>();

        for (int i = 0; i < word_count; i++)
        {
            int word_length = Random.Range(min_word_length, max_word_length + 1);
            string word = "";

            for (int j = 0; j < word_length; j++)
            {
                word += characters[Random.Range(0, characters.Length)];
            }
            words.Add(word);
        }
        string sentence = string.Join(" ", words);

        return sentence.Substring(1) + ".";
    }
    public ForeignPassengerDialogue GetForeignPassengerDialogue()
    {
        return foreign_dialogue_data;
    }
    public int GetQuestionType()
    {
        return question_type;
    }
}
