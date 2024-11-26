using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    private GameObject target_dirt;
    private CleanerUI cleaner_ui;

    public void StartCleaning(GameObject dirt, CleanerUI ui)
    {
        target_dirt = dirt;
        cleaner_ui = ui;
        StartCoroutine(CleanDirt());
    }
    private IEnumerator CleanDirt()
    {
        transform.position = target_dirt.transform.position;
        AudioManager.instance.PlaySFX("Cleaner");
        yield return new WaitForSeconds(10f);
        AudioManager.instance.sfx_source.Stop();
        Destroy(target_dirt);
        MoneyAndReputation.Instance.AddReputation(20);

        Tutorial tutorial = FindAnyObjectByType<Tutorial>();
        GameController game_controller = FindAnyObjectByType<GameController>();
        if (tutorial != null && tutorial.is_training)
        {
            tutorial.MinusPassengerCount();
        }
        else
        {
            game_controller.DecreasePassengerCount("MetroTrain");
        }

        cleaner_ui.RestoreCleanerCount();
        Destroy(gameObject);
    }
}
