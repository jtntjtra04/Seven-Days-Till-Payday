using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrunkPassengerDialogue
{
    [Header("First Allow Dialogue")]
    public string[] first_allow_name;
    [TextArea(3, 10)]
    public string[] first_allow_line;

    [Header("Second Allow Dialogue")]
    public string[] second_allow_name;
    [TextArea(3, 10)]
    public string[] second_allow_line;

    [Header("Safe Deny Dialogue")]
    public string[] safe_deny_name;
    [TextArea(3, 10)]
    public string[] safe_deny_line;

    [Header("Wrong Detain Dialogue")]
    public string[] wrong_detain_name;
    [TextArea(3, 10)]
    public string[] wrong_detain_line;

    [Header("Angry Dialogue")]
    public string[] angry_name;
    [TextArea(3, 10)]
    public string[] angry_line;

    [Header("Danger Deny Dialogue")]
    public string[] danger_deny_name;
    [TextArea(3, 10)]
    public string[] danger_deny_line;

    [Header("Correct Detain Dialogue")]
    public string[] correct_detain_name;
    [TextArea(3, 10)]
    public string[] correct_detain_line;
}
