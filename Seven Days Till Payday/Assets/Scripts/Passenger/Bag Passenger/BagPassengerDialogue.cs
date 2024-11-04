using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BagPassengerDialogue
{
    [Header("Allow Safe Dialogue")]
    public string[] allow_safe_name;
    [TextArea(3, 10)]
    public string[] allow_safe_line;

    [Header("Allow Danger Dialogue")]
    public string[] allow_danger_name;
    [TextArea(3, 10)]
    public string[] allow_danger_line;

    [Header("Detain Safe Dialogue")]
    public string[] detain_safe_name;
    [TextArea(3, 10)]
    public string[] detain_safe_line;

    [Header("Detain Danger Dialogue")]
    public string[] detain_danger_name;
    [TextArea(3, 10)]
    public string[] detain_danger_line;
}
