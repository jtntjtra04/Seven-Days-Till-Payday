using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform station_point;
    [SerializeField] private float game_duration = 300f;

    // Countdown and Timer
    public GameObject countdown_ui;
    public GameObject timer_ui;
    public TMP_Text timer_text;

    // References
    private PlayerMovement player_movement;
    public Animator transition_anim;
    private void Awake()
    {
        player_movement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        countdown_ui.SetActive(false);
        timer_ui.SetActive(false);
    }
    public void GoToStation()
    {
        player_movement.DisableMovement();
        StartCoroutine(TransitionToStation());
    }
    private IEnumerator TransitionToStation()
    {
        transition_anim.Play("StartTransition");
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector2(station_point.position.x, station_point.position.y);
        transition_anim.Play("EndTransition");
        player_movement.EnableMovement();
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartCountdown());
        StartCoroutine(StartGame());
    }
    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(3f);
    }
    private IEnumerator StartGame()
    {
        timer_ui.SetActive(true);
        float timer = game_duration;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            UpdateTimer(timer);
        }
        timer_ui.SetActive(false);
    }
    private void UpdateTimer(float time)
    {
        if(time < 0) time = 0;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
