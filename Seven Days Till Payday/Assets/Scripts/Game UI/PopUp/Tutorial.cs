using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial_panel;
    public Sprite[] tutorial_sprites;
    public Image tutorial_image;
    private int curr_page;
    public bool already_tutor = false;
    public bool is_training = false;
    public int train_passenger_count;
    public bool get_converted_money = false;

    // Dialogue
    public List<Dialogue> training_dialogues;
    public Dialogue ending_dialogue;
    public GameObject dialogue_box;
    public TMP_Text dialogue_text;

    // Trains
    [Header("Trains")]
    public GameObject metro_train;
    public GameObject commuter_train;
    public GameObject highspeed_train;

    // Train Arrive Points
    [Header("Train Arrive Points")]
    public Transform metro_arrive_point;
    public Transform commuter_arrive_point;
    public Transform highspeed_arrive_point;

    // Transition
    [Header("Transition")]
    public Animator transition_anim;

    // Station 
    [Header("Station Platform")]
    public Transform metro_platform;
    public Transform commuter_platform;
    public Transform highspeed_platform;

    // Day 7 Tutorial
    [Header("Day 7 Tutorial")]
    public GameObject confused_passanger;
    public GameObject priority_passanger;
    public GameObject dirt;
    public Transform confused_passanger_spawn;
    public Transform dirt_spawn;
    public Transform priority_passanger_spawn;

    // Day 6 Tutorial
    [Header("Day 6 Tutorial")]
    public GameObject rich_passanger;
    public GameObject student_passanger;
    public GameObject businessman_passenger;
    public Transform rich_passanger_spawn;
    public Transform student_passanger_spawn;
    public Transform businessman_passenger_spawn;

    // Day 5 Tutorial
    [Header("Day 5 Tutorial")]
    public GameObject disability_passanger;
    public GameObject drunk_passanger;
    public Transform disability_passanger_spawn;
    public Transform drunk_passanger_spawn;

    // Day 4 Tutorial
    [Header("Day 4 Tutorial")]
    public GameObject error_passanger;
    public Transform error_passanger_spawn;

    // Day 3 Tutorial
    [Header("Day 3 Tutorial")]
    public GameObject foreign_passanger;
    public GameObject bigbag_passanger;
    public Transform foreign_passanger_spawn;
    public Transform bigbag_passanger_spawn;

    // Money And Reputation for Tutorial
    [Header("Temp Money and Reputation Container")]
    public int temp_money;
    public int temp_reputation;

    [Header("Ending Scene")]
    public GameObject ending_panel;
    public GameObject player_happy;
    public GameObject player_sad;
    public TMP_Text ending_text;
    public TMP_Text ending_description;

    // References
    [Header("References")]
    public PlayerMovement player_movement;
    public Player player;
    public TimeSystem time_system;
    private DialogueManager dialogue_manager;

    private void Awake()
    {
        dialogue_manager = GetComponent<DialogueManager>();
    }
    private void Start()
    {
        get_converted_money = false;
        already_tutor = false;
        tutorial_panel.SetActive(false);
    }
    public void ShowTutorial()
    {
        tutorial_panel.SetActive(true);
        if (tutorial_sprites != null)
        {
            curr_page = 0;
            tutorial_image.sprite = tutorial_sprites[curr_page];
        }
        player_movement.DisableMovement();
    }
    public void CloseTutorial()
    {
        AudioManager.instance.PlaySFX("Click");
        tutorial_panel.SetActive(false);
        //player_movement.EnableMovement();
        StartCoroutine(ShowTrainingDialogue());
    }
    private IEnumerator ShowTrainingDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        int curr_day = time_system.day;

        if(curr_day > 0 && curr_day <= training_dialogues.Count)
        {
            Dialogue today_showcase_dialogue = training_dialogues[curr_day - 1];
            already_tutor = true;
            dialogue_manager.StartDialogue(today_showcase_dialogue);
        }
        else
        {
            get_converted_money = true;
            Dialogue today_dialogue = ending_dialogue;
            dialogue_manager.StartDialogue(today_dialogue);
        }
    }
    public void SetAlreadyTutorialFalse()
    {
        already_tutor = false;
    }
    public void SpawnTutorialTrain(int day)
    {
        if (day == 7 || day == 6)
        {
            Instantiate(metro_train, metro_arrive_point);
        }
        else if (day == 5 || day == 4)
        {
            Instantiate(commuter_train, commuter_arrive_point);
        }
        else if (day == 3)
        {
            Instantiate(highspeed_train, highspeed_arrive_point);
        }
        is_training = true;

    }
    public void SpawnTutorialPassanger(int day)
    {
        if (day == 7)
        {
            train_passenger_count = 3;

            Instantiate(confused_passanger, confused_passanger_spawn);
            Instantiate(priority_passanger, priority_passanger_spawn);
            Instantiate(dirt, dirt_spawn);
        }
        else if (day == 6)
        {
            train_passenger_count = 3;

            Instantiate(rich_passanger, rich_passanger_spawn);
            Instantiate(student_passanger, student_passanger_spawn);
            Instantiate(businessman_passenger, businessman_passenger_spawn);
        }
        else if (day == 5)
        {
            train_passenger_count = 2;

            Instantiate(disability_passanger, disability_passanger_spawn);
            Instantiate(drunk_passanger, drunk_passanger_spawn);
        }
        else if (day == 4)
        {
            train_passenger_count = 1;
            
            Instantiate(error_passanger, error_passanger_spawn);
        }
        else if (day == 3)
        {
            train_passenger_count = 2;

            Instantiate(foreign_passanger, foreign_passanger_spawn);
            Instantiate(bigbag_passanger, bigbag_passanger_spawn);
        }
        StartCoroutine(StoreMoneyAndReputation());
    }
    public void MinusPassengerCount()
    {
        train_passenger_count--;

        if(train_passenger_count <= 0)
        {
            if (player.player_on_metro)
            {
                StartCoroutine(ThrowPlayerOut(metro_platform));
            }
            else if (player.player_on_commuter)
            {
                StartCoroutine(ThrowPlayerOut(commuter_platform));
            }
            else if (player.player_on_highspeed)
            {
                StartCoroutine(ThrowPlayerOut(highspeed_platform));
            }
            else
            {
                is_training = false;
            }
        }
    }
    private IEnumerator ThrowPlayerOut(Transform platform)
    {
        player_movement.DisableMovement();
        transition_anim.Play("StartTransition");
        yield return new WaitForSeconds(1.5f);
        player_movement.transform.position = new Vector2(platform.position.x, platform.position.y);
        transition_anim.Play("EndTransition");
        player_movement.EnableMovement();
        is_training = false;
    }
    private IEnumerator StoreMoneyAndReputation()
    {
        yield return new WaitForSeconds(1f);

        // Store Money and reputation for tutorial purpose
        temp_money = MoneyAndReputation.Instance.money;
        temp_reputation = MoneyAndReputation.Instance.reputation;

        Debug.Log("Temp money : " + temp_money);
        Debug.Log("Temp Reputation : " + temp_reputation);
    }
    public void ShowResult()
    {
        player_movement.DisableMovement();
        StartCoroutine(ShowResultUI());
    }
    private IEnumerator ShowResultUI()
    {
        yield return new WaitForSeconds(0.5f);

        int reputation = MoneyAndReputation.Instance.reputation;
        int converted_reputation = reputation * 10;

        dialogue_box.SetActive(true);
        dialogue_text.text = "(" + reputation.ToString() + " amount of Reputation converted to " + converted_reputation + " amount of money)";

        AudioManager.instance.PlaySFX("Buy");
        yield return new WaitForSeconds(2f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        MoneyAndReputation.Instance.ConvertReputationToMoney();
        StartCoroutine(ShowTrainingDialogue());
    }
    public void EndGame()
    {
        int money = MoneyAndReputation.Instance.money;
        if(money < 30000)
        {
            StartCoroutine(HappyEndingCutscene());
        }
        else
        {
            StartCoroutine(SadEndingCutscene());
        }
    }
    private IEnumerator HappyEndingCutscene()
    {
        AudioManager.instance.music_source.Stop();
        transition_anim.Play("StartLongTransition");
        yield return new WaitForSeconds(7f);

        ending_panel.SetActive(true);
        player_happy.SetActive(true);
        ending_text.text = "Happy Ending";
        ending_description.text = "You've made it through the week. With rent paid and your reputation intact, you've proven yourself in a job nobody else wanted. The station is better off thanks to you, but the struggle continues. For now, you can rest easy... until next time.";

        transition_anim.Play("EndTransition");
        AudioManager.instance.PlayMusic("Ending");

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        AudioManager.instance.music_source.Stop();
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator SadEndingCutscene()
    {
        AudioManager.instance.music_source.Stop();
        transition_anim.Play("StartLongTransition");
        yield return new WaitForSeconds(7f);

        ending_panel.SetActive(true);
        player_happy.SetActive(true);
        ending_text.text = "Sad Ending";
        ending_description.text = "The week is over, but so is your chance to pay the rent. With your reputation tarnished and no place to call home, you're left wondering if the job was worth it after all. The station moves on, but you’re left behind, another casualty of the daily grind.";

        transition_anim.Play("EndTransition");
        AudioManager.instance.PlayMusic("Ending");

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        AudioManager.instance.music_source.Stop();
        SceneManager.LoadScene("MainMenu");
    }
}
