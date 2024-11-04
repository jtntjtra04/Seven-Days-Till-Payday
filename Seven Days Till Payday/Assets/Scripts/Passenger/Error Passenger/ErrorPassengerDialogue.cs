using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ErrorPassengerDialogue
{
    [Header("Allow Dialogue")]
    public string[] allow_name;
    [TextArea(3, 10)]
    public string[] allow_line;

    [Header("Deny Dialogue")]
    public string[] deny_name;
    [TextArea(3, 10)]
    public string[] deny_line;

    [Header("Detain Dialogue")]
    public string[] detain_name;
    [TextArea(3, 10)]
    public string[] detain_line;
}
