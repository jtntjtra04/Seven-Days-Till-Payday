using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause_menu;
    public static bool game_paused;

    // References
    public Transition transition;
    public TransitionDay transition_day;
    private void Start()
    {
        ResumeGame();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !transition.on_transition && !transition_day.on_transition_day)
        {
            if (game_paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        pause_menu.SetActive(false);
        Time.timeScale = 1.0f;
        game_paused = false;
    }
    public void PauseGame()
    {
        pause_menu.SetActive(true);
        Time.timeScale = 0f;
        game_paused = true;
    }
    public void MainMenu()
    {
        AudioManager.instance.PlaySFX("Click");
        pause_menu.SetActive(false);
        Time.timeScale = 1.0f;
        game_paused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
