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
        yield return new WaitForSeconds(10f);
        Destroy(target_dirt);
        cleaner_ui.RestoreCleanerCount();
        Destroy(gameObject);
    }
}
