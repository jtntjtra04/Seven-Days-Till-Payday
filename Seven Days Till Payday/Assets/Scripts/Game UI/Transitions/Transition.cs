using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public bool on_transition = false;

    public void TransitionActive()
    {
        on_transition = true;
    }
    public void TransitionDeactive()
    {
        on_transition = false;
    }
}
