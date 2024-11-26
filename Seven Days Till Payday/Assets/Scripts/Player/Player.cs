using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool player_on_metro = false;
    public bool player_on_commuter = false;
    public bool player_on_highspeed = false;

    private void FixedUpdate()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("MetroInterior")))
        {
            player_on_metro = true;
            player_on_commuter = false;
            player_on_highspeed = false;
            Debug.Log("Player on metro interior");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("CommuterInterior")))
        {
            player_on_commuter = true;
            player_on_metro = false;
            player_on_highspeed = false;
            Debug.Log("Player on commuter interior");
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("HighspeedInterior")))
        {
            player_on_highspeed = true;
            player_on_metro = false;
            player_on_commuter = false;
            Debug.Log("Player on highspeed interior");
        }
        else
        {
            player_on_metro = false;
            player_on_commuter = false;
            player_on_highspeed = false;
        }
    }
}
