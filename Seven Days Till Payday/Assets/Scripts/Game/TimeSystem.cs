using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    // Day
    public int day;
    public TMP_Text day_number;

    // Tutorial
    public Tutorial tutorial;

    private void Start()
    {
        day = 6;

        if (tutorial != null)
        {
            tutorial.SpawnTutorialPassanger(day);
        }
    }
    public void UpdateDay()
    {
        if(day > 0)
        {
            day--;
            if (day > 2)
            {
                tutorial.SpawnTutorialPassanger(day);
            }
        }
    }
}
