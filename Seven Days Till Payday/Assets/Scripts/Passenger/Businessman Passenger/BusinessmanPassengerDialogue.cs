using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BusinessmanPassengerDialogue
{
    [Header("Approve Dialogue")]
    public string[] approve_name;
    [TextArea(3, 10)]
    public string[] approve_line;

    [Header("Deny Dialogue")]
    public string[] deny_name;
    [TextArea(3, 10)]
    public string[] deny_line;

    [Header("Detain Dialogue")]
    public string[] detain_name;
    [TextArea(3, 10)]
    public string[] detain_line;
}
