using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    public GameObject bill;

    // References
    public PlayerMovement player_movement;
    private void Start()
    {
        player_movement.DisableMovement();
        StartCoroutine(ShowBill());
    }
    private IEnumerator ShowBill()
    {
        yield return new WaitForSeconds(5);
        bill.SetActive(true);
    }
    public void CloseBill()
    {
        bill.SetActive(false);
        player_movement.EnableMovement();
    }
}
