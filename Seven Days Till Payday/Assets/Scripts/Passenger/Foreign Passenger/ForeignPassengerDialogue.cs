using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForeignPassengerDialogue
{
    [Header("Allow Train Dialogue")]
    public string[] allow_train_name;
    [TextArea(3, 10)]
    public string[] allow_train_line;

    [Header("Allow Fruit Dialogue")]
    public string[] allow_fruit_name;
    [TextArea(3, 10)]
    public string[] allow_fruit_line;

    [Header("Allow Sus Dialogue")]
    public string[] allow_sus_name;
    [TextArea(3, 10)]
    public string[] allow_sus_line;

    [Header("Deny Train Dialogue")]
    public string[] deny_train_name;
    [TextArea(3, 10)]
    public string[] deny_train_line;

    [Header("Deny Fruit Dialogue")]
    public string[] deny_fruit_name;
    [TextArea(3, 10)]
    public string[] deny_fruit_line;

    [Header("Deny Sus Dialogue")]
    public string[] deny_sus_name;
    [TextArea(3, 10)]
    public string[] deny_sus_line;

    [Header("Detain Train Dialogue")]
    public string[] detain_train_name;
    [TextArea(3, 10)]
    public string[] detain_train_line;

    [Header("Detain Fruit Dialogue")]
    public string[] detain_fruit_name;
    [TextArea(3, 10)]
    public string[] detain_fruit_line;

    [Header("Detain Sus Dialogue")]
    public string[] detain_sus_name;
    [TextArea(3, 10)]
    public string[] detain_sus_line;
}
