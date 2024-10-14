using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial_panel;
    public Sprite[] tutorial_sprites;
    public Image tutorial_image;
    private int curr_page;

    // References
    public PlayerMovement player_movement;

    private void Start()
    {
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
        tutorial_panel.SetActive(false);
        player_movement.EnableMovement();
    }
}
