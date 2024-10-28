using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfusedPassengerDialogue
{
    [Header("Correct Ticket Dialogue")]
    public string[] correct_ticket_name;
    [TextArea(3, 10)]
    public string[] correct_ticket_line;

    [Header("Wrong Ticket Dialogue")]
    public string[] wrong_ticket_name;
    [TextArea(3, 10)]
    public string[] wrong_ticket_line;

    [Header("Detained Dialogue")]
    public string[] detained_name;
    [TextArea(3, 10)]
    public string[] detained_line;
}
