using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SwitchPlace : MonoBehaviour
{
    private bool can_switch_place = false;
    public Transform destination;
    private bool is_switching = false;
    public Animator transition_anim;

    // Icon
    [SerializeField] private GameObject interact_icon;

    // References
    public PlayerMovement player_movement;

    private void Start()
    {
        //interact_icon.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && can_switch_place && !is_switching)
        {
            StartCoroutine(TransitionPlace());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_switch_place = true;
            interact_icon.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_switch_place = false;
            interact_icon.SetActive(false);
        }
    }
    private IEnumerator TransitionPlace()
    {
        is_switching = true;
        player_movement.DisableMovement();
        transition_anim.Play("StartTransition");
        yield return new WaitForSeconds(1.5f);
        player_movement.transform.position = new Vector2(destination.position.x, destination.position.y);
        transition_anim.Play("EndTransition");
        player_movement.EnableMovement();
        is_switching = false;
    }
}
