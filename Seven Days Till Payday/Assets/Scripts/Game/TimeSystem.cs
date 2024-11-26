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

    // Transition
    public Animator transition_anim;
    public Animator transition_day_anim;

    // Apartment Spawn
    public Transform station_door;

    // References
    public Tutorial tutorial;
    public GameController game_controller;
    public PlayerMovement player_movement;
    public Barrier barrier;

    private void Start()
    {
        day = 7;

        if (tutorial != null && day > 2)
        {
            tutorial.SpawnTutorialTrain(day);
            tutorial.SpawnTutorialPassanger(day);
        }
    }
    public void UpdateDay()
    {
        if(day > 0)
        {
            day--;
        }
        StartCoroutine(StartTransitionDay());
    }
    private IEnumerator StartTransitionDay()
    {
        player_movement.DisableMovement();
        AudioManager.instance.PlaySFX("Chime");
        yield return new WaitForSeconds(3f);

        if(day > 0)
        {
            day_number.text = "Day-" + day.ToString();
        }
        else
        {
            day_number.text = "Payday";
        }
        
        transition_anim.Play("StartLongTransition");
        yield return new WaitForSeconds(3f);
        transition_day_anim.Play("StartTransition");
        game_controller.ResetGame();
        yield return new WaitForSeconds(5f);
        player_movement.transform.position = new Vector2(station_door.position.x, station_door.position.y);
        transition_anim.Play("EndTransition");
        transition_day_anim.Play("EndTransition");

        if (day > 2)
        {
            tutorial.SpawnTutorialTrain(day);
            tutorial.SpawnTutorialPassanger(day);
            barrier.PlaceBarrier();
        }

        player_movement.EnableMovement();
    }
}
