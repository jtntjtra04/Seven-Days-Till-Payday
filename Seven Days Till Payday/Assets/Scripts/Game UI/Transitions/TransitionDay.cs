using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDay : MonoBehaviour
{
    public bool on_transition_day = false;

    public void TransitionDayActive()
    {
        on_transition_day = true;
    }
    public void TransitionDayDeactive()
    {
        on_transition_day = false;
    }
}
