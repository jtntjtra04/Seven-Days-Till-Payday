using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanerUI : MonoBehaviour
{
    public GameObject cleaner_prefab;
    public int cleaner_count = 2;
    private GameObject cleaner_instance;
    private bool dragging_cleaner = false;

    // Cleaner Count UI
    [SerializeField] private TMP_Text count_text;

    // References
    //public PlayerMovement player_movement;
    private void Start()
    {
        count_text.text = cleaner_count.ToString();
    }
    private void Update()
    {
        if(dragging_cleaner && cleaner_instance != null)
        {
            Vector3 new_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            new_position.z = 0;
            cleaner_instance.transform.position = new_position;

            if (Input.GetMouseButtonDown(0))
            {
                MouseReleased();
            }
/*            if (player_movement.is_moving)
            {
                MouseReleased();
            }*/
        }
    }
    public void CleanerButtonClicked()
    {
        if(cleaner_count > 0 && cleaner_instance == null)
        {
            AudioManager.instance.PlaySFX("Click");
            Vector3 spawn_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawn_position.z = 0;
            cleaner_instance = Instantiate(cleaner_prefab, spawn_position, Quaternion.identity);
            dragging_cleaner = true;
        }
        else
        {
            Debug.Log("No cleaners left");
        }
    }
    public void MouseReleased()
    {
        if(cleaner_instance != null)
        {
            Collider2D dirt_collider = GetDirtCollider();

            if(dirt_collider != null)
            {
                cleaner_count--;
                UpdateCountText();
                cleaner_instance.GetComponent<Cleaner>().StartCleaning(dirt_collider.gameObject, this);
            }
            else
            {
                Destroy(cleaner_instance);
            }
            /*RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.CompareTag("Dirt"))
            {
                cleaner_count--;
                cleaner_instance.GetComponent<Cleaner>().StartCleaning(hit.collider.gameObject, this);
            }
            else
            {
                Destroy(cleaner_instance);
            }*/
            dragging_cleaner = false;
            cleaner_instance = null;
        }
    }
    private Collider2D GetDirtCollider()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(cleaner_instance.transform.position);
        foreach(var hit in hits)
        {
            if (hit.CompareTag("Dirt"))
            {
                return hit;
            }
        }
        return null;
    }
    public void RestoreCleanerCount()
    {
        if(cleaner_count < 2)
        {
            AudioManager.instance.PlaySFX("Pickup");
            cleaner_count++;
            UpdateCountText();
        }
    }
    private void UpdateCountText()
    {
        count_text.text = cleaner_count.ToString();
    }
}
