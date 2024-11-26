using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabilityPassenger : MonoBehaviour
{
    public DisabilityPassengerDialogue disability_dialogue_data;

    private void OnDestroy()
    {
        DisabilityPassengerUI disabilityPassengerUI = FindAnyObjectByType<DisabilityPassengerUI>();
        if(disabilityPassengerUI != null)
        {
            disabilityPassengerUI.ClearCurrentMinigame();
        }
    }
    public DisabilityPassengerDialogue GetDisabilityPassengerDialogue()
    {
        return disability_dialogue_data;
    }
}
