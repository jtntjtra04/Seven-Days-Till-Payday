using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject barrier;
    private bool tutorial_interact = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && tutorial_interact)
        {
            if (barrier != null)
            {
                Destroy(barrier);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorial_interact = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorial_interact = false;
        }
    }
}
