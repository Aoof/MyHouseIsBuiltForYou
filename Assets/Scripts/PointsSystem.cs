using System;
using System.Collections.Generic;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    public enum ObjectState
    {
        Unchanged,
        Destroyed,
        Changed
    }

    public Dictionary<string, List<ObjectState>> stateHistory;
    public int tasksQuantity;
    public int tasksDone;

    public GameObject[] interactables;

    void Awake()
    {
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }
}
