using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPassenger : MonoBehaviour
{
    public BagPassengerDialogue bag_dialogue_data;

    public bool IsPassengerDanger()
    {
        float danger_chance = Random.value;
        if(danger_chance <= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public BagPassengerDialogue GetBagPassengerDialogue()
    {
        return bag_dialogue_data;
    }
}
