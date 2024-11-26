using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkPassenger : MonoBehaviour
{
    public DrunkPassengerDialogue drunk_dialogue_data;

    private void OnDestroy()
    {
        DrunkPassengerUI drunkPassengerUI = FindAnyObjectByType<DrunkPassengerUI>();
        if(drunkPassengerUI != null)
        {
            drunkPassengerUI.ClearCurrentMinigame();
        }
    }
    public DrunkPassengerDialogue GetDrunkPassengerDialogue()
    {
        return drunk_dialogue_data;
    }
}
