using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    // Spawn points
    private Transform spawn_point;
    private Transform depart_point;
    private Transform metro_point;
    private Transform commuter_point;
    private Transform highspeed_point;

    // References
    private TrainPointsReferences TrainPointsReferences;

    private void Start()
    {
        TrainPointsReferences = FindAnyObjectByType<TrainPointsReferences>();
    }
}
