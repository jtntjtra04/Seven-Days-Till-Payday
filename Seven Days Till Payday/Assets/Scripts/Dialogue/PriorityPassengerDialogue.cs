using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PriorityPassengerDialogue
{
    [Header("Correct Dialogue")]
    public string[] correct_name;
    [TextArea(3, 10)]
    public string[] correct_line;

    [Header("Wrong Dialogue")]
    public string[] wrong_name;
    [TextArea(3, 10)]
    public string[] wrong_line;

    [Header("Detain Dialogue")]
    public string[] detain_name;
    [TextArea(3, 10)]
    public string[] detain_line;
}
