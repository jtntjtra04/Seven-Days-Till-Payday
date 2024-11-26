using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private bool passenger_destroyed = false;
    private bool play_audio = false;
    private bool play_sfx = false;

    // Spawn points
    private Transform spawn_point;
    private Transform depart_point;
    private Transform arrive_point;

    // Doors
    private GameObject train_door;

    // Train Movement
    private float speed = 12f;
    private bool train_arrived = false;
    private bool train_departed = false;
    private bool passengers_done = false;

    // Train Timer
    private float train_timer;
    private float curr_timer;

    // References
    private TrainPointsReferences TrainPointsReferences;
    private TrainDoorsReferences TrainDoorsReferences;
    private Tutorial tutorial;
    private GameController game_controller;
    private TimeSystem time_system;

    private void Start()
    {
        TrainPointsReferences = FindAnyObjectByType<TrainPointsReferences>();
        TrainDoorsReferences = FindAnyObjectByType<TrainDoorsReferences>();
        tutorial = FindAnyObjectByType<Tutorial>();
        game_controller = FindAnyObjectByType<GameController>();
        time_system = FindAnyObjectByType<TimeSystem>();

        spawn_point = TrainPointsReferences.spawn_point;
        depart_point = TrainPointsReferences.depart_point;

        if (gameObject.CompareTag("MetroTrain"))
        {
            arrive_point = TrainPointsReferences.metro_point;
            train_door = TrainDoorsReferences.metro_doors;
        }
        else if (gameObject.CompareTag("CommuterTrain"))
        {
            arrive_point = TrainPointsReferences.commuter_point;
            train_door = TrainDoorsReferences.commuter_doors;
        }
        else if (gameObject.CompareTag("HighSpeedTrain"))
        {
            arrive_point = TrainPointsReferences.highspeed_point;
            train_door = TrainDoorsReferences.highspeed_doors;
        }
        string train_type = gameObject.tag;
        game_controller.RegisterTrain(train_type, this);
    }
    private void Update()
    {
        if (game_controller.on_game)
        {
            if (!train_arrived)
            {
                transform.position = Vector3.MoveTowards(transform.position, arrive_point.position, speed * Time.deltaTime);
                if (!play_audio)
                {
                    AudioManager.instance.PlayHybrid("TrainMove");
                    play_audio = true;
                }

                if(Vector3.Distance(transform.position, arrive_point.position) < 0.1f)
                {
                    AudioManager.instance.hybrid_source.Stop();
                    play_audio = false;
                    train_arrived = true;
                    train_door.SetActive(true);
                    StartTrainTimer();
                    game_controller.SpawnPassengers(gameObject.tag);
                }
            }
            else if(train_timer <= 0 || passengers_done)
            {
                if(train_timer <= 0 && !passenger_destroyed)
                {
                    game_controller.NotifyThrowPassenger(gameObject.tag);
                    game_controller.DestroyPassengersByTrainType(gameObject.tag);
                    passenger_destroyed = true;
                }
                train_door.SetActive(false);
                play_sfx = false;

                if (!train_departed)
                {
                    transform.position = Vector3.MoveTowards(transform.position, depart_point.position, speed * Time.deltaTime);

                    if (!play_audio)
                    {
                        AudioManager.instance.PlayHybrid("TrainMove");
                        play_audio = true;
                    }

                    if (Vector3.Distance(transform.position, depart_point.position) < 0.1f)
                    {
                        AudioManager.instance.hybrid_source.Stop();
                        play_audio = false;
                        train_departed = true;
                        StartCoroutine(IntervalNewTrain());
                    }
                }
            }
            else if(train_timer > 0)
            {
                train_timer -= Time.deltaTime;
                if(train_timer <= 30f && train_timer >= 25f)
                {
                    if (gameObject.CompareTag("MetroTrain"))
                    {
                        if (!play_sfx)
                        {
                            AudioManager.instance.PlayTrain("MetroHorn");
                            play_sfx = true;
                            Debug.Log("Play metro horn");
                        }
                    }
                    else if (gameObject.CompareTag("CommuterTrain"))
                    {
                        if (!play_sfx)
                        {
                            AudioManager.instance.PlayTrain("CommuterHorn");
                            play_sfx = true;
                            Debug.Log("Play commuter horn");
                        }
                    }
                    else if (gameObject.CompareTag("HighSpeedTrain"))
                    {
                        if (!play_sfx)
                        {
                            AudioManager.instance.PlayTrain("HighspeedHorn");
                            play_sfx = true;
                            Debug.Log("Play highspeed horn");
                        }
                    }
                }
            }
        }
        else
        {
            if (tutorial != null && !tutorial.is_training)
            {
                train_door.SetActive(false);

                if (!train_departed)
                {
                    transform.position = Vector3.MoveTowards(transform.position, depart_point.position, speed * Time.deltaTime);

                    if (!play_audio)
                    {
                        AudioManager.instance.PlayHybrid("TrainMove");
                        play_audio = true;
                    }

                    if (Vector3.Distance(transform.position, depart_point.position) < 0.1f)
                    {
                        AudioManager.instance.hybrid_source.Stop();
                        play_audio = false;
                        train_departed = true;
                        Destroy(gameObject, 1f);
                    }
                }
            }
            else
            {
                if(Vector3.Distance(transform.position, arrive_point.position) < 0.1f)
                {
                    train_door.SetActive(true);
                }
            }
        }
    }
    private void StartTrainTimer()
    {
        float[] timer_option = { 45f, 60f, 90f, 120f };
        train_timer = timer_option[Random.Range(0, timer_option.Length)];
        curr_timer = train_timer;
        Debug.Log("Train timer : " +  curr_timer);
    }
    private IEnumerator IntervalNewTrain()
    {
        game_controller.UnregisterTrain(gameObject.tag);

        if(time_system.day == 7 || time_system.day == 6)
        {
            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(10f);
        }

        train_arrived = false;
        train_departed = false;
        passengers_done = false;
        passenger_destroyed = false;

        game_controller.SpawnTrain(gameObject.tag);
        Destroy(gameObject);
    }
    public void DepartTrainLoading()
    {
        StartCoroutine(DepartTrain());
    }
    private IEnumerator DepartTrain()
    {
        Debug.Log("Depart train in 3 seconds");
        yield return new WaitForSeconds(3f);

        if(curr_timer == 45f)
        {
            MoneyAndReputation.Instance.AddReputation(150);
            Debug.Log("Special train departed! Added 150 reputations");
        }

        passengers_done = true;
    }
}
