using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition_anim;
    public void PlayButton()
    {
        AudioManager.instance.PlaySFX("Click");
        StartCoroutine(StartGameTransition());
    }
    public void SettingsButton()
    {

    }
    public void ExitButton()
    {
        AudioManager.instance.PlaySFX("Click");
        Application.Quit();
    }
    private IEnumerator StartGameTransition()
    {
        transition_anim.Play("StartTransition");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameScene");
        transition_anim.Play("EndTransition");
    }
}
