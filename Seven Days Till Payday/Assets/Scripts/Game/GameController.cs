using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Track active train
    private Dictionary<string, Train> active_train = new Dictionary<string, Train>();

    // To store spawned passengers by Train type
    private Dictionary<string, List<GameObject>> passenger_by_train_type = new Dictionary<string, List<GameObject>>();

    //public Transform station_point;
    private float game_duration;
    public bool on_game = false;

    // Countdown and Timer
    [Header("Countdown and Timer")]
    public GameObject countdown_ui;
    public TMP_Text countdown_text;
    public GameObject timer_ui;
    public TMP_Text timer_text;
    public TMP_Text day_text;

    // Trains
    [Header("Trains")]
    public GameObject metro_train;
    public GameObject commuter_train;
    public GameObject highspeed_train;

    // Train Spawn Points
    [Header("Train Spawn Points")]
    public Transform spawn_point;

    // Station
    [Header("Station Platform")]
    public Transform metro_platform;
    public Transform commuter_platform;
    public Transform highspeed_platform;

    // Passengers
    [Header("Passengers")]
    public GameObject confused_passanger;
    public GameObject old_passanger;
    public GameObject young_passenger;
    public GameObject dirt;
    public GameObject rich_passenger;
    public GameObject student_passenger;
    public GameObject businessman_passenger;
    public GameObject disability_passanger;
    public GameObject drunk_passenger;
    public GameObject error_passenger;
    public GameObject foreign_passenger;
    public GameObject bag_passenger;

    // Metro Passenger Spawn Points
    [Header("Metro Passenger Spawn Points")]
    public Transform[] metro_interior_spawns;
    public Transform[] metro_outdoor_spawns;

    // Commuter Passenger Spawn Points
    [Header("Commuter Passenger Spawn Points")]
    public Transform[] commuter_interior_spawns;
    public Transform[] commuter_outdoor_spawns;

    // HighSpeed Passenger Spawn Points
    [Header("HighSpeed Passenger Spawn Points")]
    public Transform[] highspeed_interior_spawns;
    public Transform[] highspeed_outdoor_spawns;

    // Trains Passenger Count
    [Header("Trains Passenger Count")]
    public int metro_passenger_count;
    public int commuter_passenger_count;
    public int highspeed_passenger_count;

    // References
    [Header("References")]
    private PlayerMovement player_movement;
    public Animator transition_anim;
    public Animator transition_day_anim;
    public TimeSystem time_system;
    private Player player;
    public Tutorial tutorial;
    private void Awake()
    {
        player_movement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }
    private void Start()
    {
        countdown_ui.SetActive(false);
        timer_ui.SetActive(false);
    }
    public void LoadingGame()
    {
        player_movement.DisableMovement();
        StartCoroutine(StartGameTransition());
    }
    private IEnumerator StartGameTransition()
    {
        if(time_system.day == 7)
        {
            day_text.text = "Day-" + time_system.day.ToString();
            transition_day_anim.Play("StartTransition");
            yield return new WaitForSeconds(5f);
            transition_day_anim.Play("EndTransition");
        }
        else
        {
            transition_anim.Play("StartTransition");
            yield return new WaitForSeconds(1.5f);
            transition_anim.Play("EndTransition");
        }

        tutorial.gameObject.SetActive(false);
        player_movement.EnableMovement();

        if (time_system.day > 2)
        {
            MoneyAndReputation.Instance.ResetMoneyAndReputation(tutorial.temp_money, tutorial.temp_reputation); // Reset money after tutorial
        }

        AudioManager.instance.DecreaseMusicVolume();
        AudioManager.instance.PlayBGM("Crowd");
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        countdown_ui.SetActive(true);
        int countdown = 5;
        while(countdown > 0)
        {
            countdown_text.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdown_text.text = "Start";
        yield return new WaitForSeconds(1f);
        countdown_ui.SetActive(false);

        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame()
    {
        timer_ui.SetActive(true);
        on_game = true;

        if(time_system.day == 7 || time_system.day == 6)
        {
            game_duration = 120f;
            SpawnTrain("MetroTrain");
        }
        else if(time_system.day == 5 || time_system.day == 4)
        {
            game_duration = 180f;
            SpawnTrain("MetroTrain");
            SpawnTrain("CommuterTrain");
        }
        else
        {
            game_duration = 240f;
            SpawnTrain("MetroTrain");
            SpawnTrain("CommuterTrain");
            SpawnTrain("HighSpeedTrain");
        }
        float timer = game_duration;
        bool sfx_played = false;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            UpdateTimer(timer);
            if(timer <= 30f && !sfx_played)
            {
                AudioManager.instance.PlaySFX("Clock");
                sfx_played = true;
            }
        }
        on_game = false;
        sfx_played = false;
        timer_ui.SetActive(false);
        AudioManager.instance.IncreaseVolumeMusic();
        AudioManager.instance.bgm_source.Stop();

        time_system.UpdateDay();
    }
    private void UpdateTimer(float time)
    {
        if(time < 0) time = 0;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SpawnTrain(string train_type)
    {
        if (on_game)
        {
            if (train_type == "MetroTrain")
            {
                Instantiate(metro_train, spawn_point);
            }
            else if (train_type == "CommuterTrain")
            {
                Instantiate(commuter_train, spawn_point);
            }
            else if (train_type == "HighSpeedTrain")
            {
                Instantiate(highspeed_train, spawn_point);
            }
        }
    }
    public void SpawnPassengers(string train_type)
    {
        if (!passenger_by_train_type.ContainsKey(train_type))
        {
            passenger_by_train_type[train_type] = new List<GameObject>();
        }

        int day = time_system.day;
        List<GameObject> available_passengers = new List<GameObject>();

        if(train_type == "MetroTrain")
        {
            available_passengers.Add(confused_passanger);
            available_passengers.Add(old_passanger);
            available_passengers.Add(young_passenger);
            available_passengers.Add(dirt);

            if(day == 6 || day == 5)
            {
                available_passengers.Add(rich_passenger);
                available_passengers.Add(student_passenger);
                available_passengers.Add(businessman_passenger);
            }
            else if(day <= 4 &&  day >= 1)
            {
                available_passengers.Add(rich_passenger);
                available_passengers.Add(student_passenger);
                available_passengers.Add(businessman_passenger);
                available_passengers.Add(error_passenger);
            }
        }
        else if(train_type == "CommuterTrain")
        {
            available_passengers.Add(confused_passanger);
            available_passengers.Add(old_passanger);
            available_passengers.Add(young_passenger);
            available_passengers.Add(student_passenger);
            available_passengers.Add(businessman_passenger);
            available_passengers.Add(drunk_passenger);
            available_passengers.Add(disability_passanger);

            if(day <= 4 && day >= 1)
            {
                available_passengers.Add(error_passenger);
            }
        }
        else if(train_type == "HighSpeedTrain")
        {
            available_passengers.Add(confused_passanger);
            available_passengers.Add(rich_passenger);
            available_passengers.Add(businessman_passenger);
            available_passengers.Add(error_passenger);
            available_passengers.Add(foreign_passenger);
            available_passengers.Add(bag_passenger);
        }
        System.Random random = new System.Random();

        int passenger_count = 5;
        InitializePassengerCount(train_type, passenger_count);

        passenger_count = 0;

        // Interior spawn
        foreach(Transform passenger_spawn_point in 
            train_type == "MetroTrain" ? metro_interior_spawns:
            train_type == "CommuterTrain" ? commuter_interior_spawns:
            highspeed_interior_spawns)
        {
            GameObject passenger_to_spawn;
            do
            {
                passenger_to_spawn = available_passengers[random.Next(available_passengers.Count)];
            }
            while (passenger_to_spawn == disability_passanger || passenger_to_spawn == rich_passenger || passenger_to_spawn == student_passenger);

            GameObject spawned_passenger = Instantiate(passenger_to_spawn, passenger_spawn_point);
            passenger_by_train_type[train_type].Add(spawned_passenger);
            passenger_count++;
        }
        // Outdoor Spawn
        foreach(Transform passenger_spawn_point in 
            train_type == "MetroTrain" ? metro_outdoor_spawns:
            train_type == "CommuterTrain" ? commuter_outdoor_spawns:
            highspeed_outdoor_spawns)
        {
            GameObject passenger_to_spawn;
            do
            {
                passenger_to_spawn = available_passengers[random.Next(available_passengers.Count)];
            } while (passenger_to_spawn == old_passanger || passenger_to_spawn == young_passenger);

            GameObject spawned_passenger = Instantiate(passenger_to_spawn, passenger_spawn_point);
            passenger_by_train_type[train_type].Add(spawned_passenger);
            passenger_count++;

            if(passenger_count >= 5)
            {
                break;
            }
        }
    }
    public void RegisterTrain(string train_type, Train train)
    {
        Debug.Log("Register Train");
        active_train[train_type] = train;
    }
    public void UnregisterTrain(string train_type)
    {
        Debug.Log("Unregister Train");
        active_train.Remove(train_type);
    }
    public void InitializePassengerCount(string train_type, int count)
    {
        switch (train_type)
        {
            case "MetroTrain":
                metro_passenger_count = count;
                break;
            case "CommuterTrain":
                commuter_passenger_count = count;
                break;
            case "HighSpeedTrain":
                highspeed_passenger_count = count;
                break;
        }
    }
    public void DecreasePassengerCount(string train_type)
    {
        switch (train_type)
        {
            case "MetroTrain":

                metro_passenger_count--;
                Debug.Log("Metro_passenger_count : " + metro_passenger_count);

                if(metro_passenger_count <= 0)
                {
                    if (player.player_on_metro)
                    {
                        StartCoroutine(ThrowPlayerOut(metro_platform));
                    }
                    NotifyTrainDeparture("MetroTrain");
                }
                break;
            case "CommuterTrain":

                commuter_passenger_count--;
                Debug.Log("Commuter_passenger_count : " + commuter_passenger_count);

                if (commuter_passenger_count <= 0)
                {
                    if (player.player_on_commuter)
                    {
                        StartCoroutine(ThrowPlayerOut(commuter_platform));
                    }
                    NotifyTrainDeparture("CommuterTrain");
                }
                break;
            case "HighSpeedTrain":

                highspeed_passenger_count--;
                Debug.Log("Highspeed_passenger_count : " + highspeed_passenger_count);

                if (highspeed_passenger_count <= 0)
                {
                    if (player.player_on_highspeed)
                    {
                        StartCoroutine(ThrowPlayerOut(highspeed_platform));
                    }
                    NotifyTrainDeparture("HighSpeedTrain");
                }
                break;
        }
    }
    private void NotifyTrainDeparture(string train_type)
    {
        if(active_train.TryGetValue(train_type, out Train train))
        {
            train.DepartTrainLoading();
        }
    }
    public void NotifyThrowPassenger(string train_type)
    {
        switch (train_type)
        {
            case "MetroTrain":
                if (player.player_on_metro)
                {
                    StartCoroutine(ThrowPlayerOut(metro_platform));
                }
                break;
            case "CommuterTrain":
                if (player.player_on_commuter)
                {
                    StartCoroutine(ThrowPlayerOut(commuter_platform));
                }
                break;
            case "HighSpeedTrain":
                if (player.player_on_highspeed)
                {
                    StartCoroutine(ThrowPlayerOut(highspeed_platform));
                }
                break;
        }
    }
    private IEnumerator ThrowPlayerOut(Transform platform)
    {
        player_movement.DisableMovement();
        transition_anim.Play("StartTransition");
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector2(platform.position.x, platform.position.y);
        transition_anim.Play("EndTransition");
        player_movement.EnableMovement();
    }
    public void DestroyPassengersByTrainType(string train_type)
    {
        if (on_game)
        {
            if (passenger_by_train_type.ContainsKey(train_type))
            {
                foreach (GameObject passenger in passenger_by_train_type[train_type])
                {
                    if (passenger != null)
                    {
                        Debug.Log("Passenger name : " +  passenger.name);
                        switch (passenger.name)
                        {
                            case "ConfusedPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(25);
                                break;
                            case "Dirt(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(10);
                                break;
                            case "OldPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(25);
                                break;
                            case "YoungPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(25);    
                                break;
                            case "RichPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(40);
                                break;
                            case "StudentPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(25);
                                break;
                            case "BusinessmanPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(25);
                                break;
                            case "DrunkPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(40);
                                break;
                            case "DisabilityPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(40);
                                break;
                            case "ErrorPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(80);
                                break;
                            case "ForeignPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(40);
                                break;
                            case "BagPassenger(Clone)":
                                MoneyAndReputation.Instance.MinusReputation(40);
                                break;
                        }
                        Destroy(passenger);
                    }
                }
                passenger_by_train_type[train_type].Clear();
            }
        }
        else
        {
            if (passenger_by_train_type.ContainsKey(train_type))
            {
                foreach (GameObject passenger in passenger_by_train_type[train_type])
                {
                    if (passenger != null)
                    {
                        Destroy(passenger);
                    }
                }
                passenger_by_train_type[train_type].Clear();
            }
        }
    }
    public void ResetGame()
    {
        AudioManager.instance.hybrid_source.Stop();

        foreach (var train in active_train.Values)
        {
            if (train != null)
            {
                Destroy(train.gameObject);
            }
        }
        active_train.Clear();

        foreach(var passenger_list in passenger_by_train_type.Values)
        {
            foreach(GameObject passenger in passenger_list)
            {
                if(passenger != null)
                {
                    Destroy(passenger);
                }
            }
        }
        passenger_by_train_type.Clear();

/*        DestroyPassengersByTrainType("MetroTrain");
        DestroyPassengersByTrainType("CommuterTrain");
        DestroyPassengersByTrainType("HighSpeedTrain");*/

        metro_passenger_count = 0;
        commuter_passenger_count = 0;
        highspeed_passenger_count = 0;

        on_game = false;
        timer_ui.SetActive(false);
        timer_text.text = "00.00";

        Debug.Log("Resetting Game");
    }
}
