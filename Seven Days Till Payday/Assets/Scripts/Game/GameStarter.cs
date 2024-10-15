using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject confirmation_buttons;

    // References
    public PlayerMovement player_movement;
    public GameController game_controller;
    private void Start()
    {
        confirmation_buttons.SetActive(false);
    }
    public void ShowConfirmation()
    {
        confirmation_buttons.SetActive(true);
        player_movement.DisableMovement();
    }
    public void StartButton()
    {
        confirmation_buttons.SetActive(false);
        game_controller.GoToStation();
    }
    public void CancelButton()
    {
        confirmation_buttons.SetActive(false);
        player_movement.EnableMovement();
    }
}
