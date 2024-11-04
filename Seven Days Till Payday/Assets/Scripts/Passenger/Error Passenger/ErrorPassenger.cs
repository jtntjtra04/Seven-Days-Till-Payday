using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPassenger : MonoBehaviour
{
    public ErrorPassengerDialogue error_dialogue_data;

    public ErrorPassengerDialogue GetErrorPassengerDialogue()
    {
        return error_dialogue_data;
    }
}
